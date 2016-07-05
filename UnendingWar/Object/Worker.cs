using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace UnendingWar
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
     
    public class Worker : Sprite
    {
        bool isReceiveGem;
        bool isDrawPoint = false;
        List<Gem> gems = new List<Gem>();
        Texture2D gemTexture;

        public Worker(Game game,UnitType.Unit type,Castle castle,ParticleSystem effect)
            : base(game,type,castle,effect)
        {
            isReceiveGem = false;
            position = castle.Center;
            velocity = new Vector2(0, -speed);
            this.gems = castle.gems;
        }
        protected override void Chase(Vector2 pos)
        {
            animationPlayer.PlayAnimation(run);
            float dx = pos.X - position.X;
            float dy = pos.Y - position.Y;
            float theta = (float)Math.Atan(dy / dx);

            if (dx < 0)
            {
                theta -= MathHelper.Pi;
            }

            velocity = new Vector2((float)Math.Cos(theta) * speed, (float)Math.Sin(theta) * speed);
            position += velocity;
        }
        public Gem getGem()
        {
            int index =0;
            float distance = 10000;
            for (int i = 1; i < gems.Count; i++)
            {
                if (gems[i].hp > 0)
                {
                    index = i;
                    distance = Vector2.Distance(castle.Center, gems[i].center);
                    break;
                }
            }

            for (int i = 1; i < gems.Count; i++)
            {
                if (Vector2.Distance(gems[i].center, castle.Center) < distance)
                {
                    if (gems[i].hp <= 0)
                        continue;
                
                    index = i;
                }
            }
            if (gems[index].hp > 0)
                return gems[index];
            else return null;

        }
        public override void GoForGold(GameTime gameTime)
        {
            Center = position;
            if (getGem() != null && Vector2.Distance(Center, getGem().center) > range && !isReceiveGem)
            {
                Chase(getGem().center);
            }
            else
            {
                animationPlayer.PlayAnimation(attack);
                time += gameTime.ElapsedGameTime.Milliseconds;
                if (time > attackTime)
                {
                    animationPlayer.PlayAnimation(run);
                    time -= attackTime;
                    isReceiveGem = true;
                    getGem().hp -= 5;
                    gemTexture = Game.Content.Load<Texture2D>("Gem/Coin" + Helper.GetRandomInt(5));
                }
            }
            
        }
        public void ReturnCastle()
        {
            if (isReceiveGem && Vector2.Distance(Center, castle.Center) > 70)
            {
                Chase(castle.Center);
            }
            else
            {
                
                audio.Updated.Play();
                isReceiveGem = false;
                if (castle.status == UnitType.CastleStatus.Updated)
                    castle.gold += 25;
                else
                    castle.gold += 15;
                isDrawPoint = true;
                color = 1f;
                timeDraw = 0;
                posY = castle.position.Y;
                isDrawPoint = true;

                if(getGem()==null)
                    Remove();
            }
            
        }
        public override void Update(GameTime gameTime)
        {

            if (getGem() != null)
            {
                Center = position;

                if (hp <= 0)
                {
                    Dead(gameTime);
                }
                else if (!isReceiveGem)
                {
                    GoForGold(gameTime);
                }
                else
                {

                    ReturnCastle();
                }
            }
            else
            {
                ReturnCastle();
            }


                hpBar.remain = hp;
                hpBar.Update(gameTime);
                hpBar.Position = position + new Vector2(0, -80);
            
            
        }
        int timeDraw; float color, posY;
        
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (isReceiveGem&&getGem()!=null)
            {
                sp.Draw(gemTexture, Center, Color.White);
            }
            if (isDrawPoint)
            {
                if (timeDraw < 4000 && castle != null)
                {
                    sp.DrawString(Game.Content.Load<SpriteFont>("point"), "+15 Gold!", new Vector2(castle.position.X+50, posY--), new Color(0f, 1f, 1f, color));
                    color -= 0.005f;
                   
                }
                else
                {
                    timeDraw += gameTime.ElapsedGameTime.Milliseconds;
                    isDrawPoint = false;
                }
            }
        }
    }
}