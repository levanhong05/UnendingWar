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
    public class Catapult : Sprite
    {

        List<Rock> bullets = new List<Rock>();

        public Catapult(Game game, UnitType.Unit type, Castle castle, ParticleSystem effect)
            : base(game, type, castle, effect)
        {

            this.castle = castle;
            // TODO: Construct any child components here
        }


        public override void Attack(GameTime gameTime)
        {
            animationPlayer.PlayAnimation(attack);
            if (!isFire)
            {
                audio.PlayRangeAttackSound();

                Rock b = new Rock(Game, this, castle.Enemy,position+new Vector2(-20,-20));
                isFire = true;
                bullets.Add(b);
            }
            time += gameTime.ElapsedGameTime.Milliseconds;
            if (time > attackTime)
            {
                time -= attackTime;
                isFire = false;

            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (bullets != null)
                foreach (Rock b in bullets)
                    if (b.Enabled)
                    {
                        b.Update(gameTime);
                    }

            Center = position;
            aimSprite = castle.Enemy;
            foreach (Protector p in castle.Enemy.Protectors)
            {
                if (CheckCollides(Center, p.Center, range))
                    aimSprite = p;
                break;
            }
            foreach (BasicObject w in castle.Enemy.wall)
            {
                if (CheckCollides(Center, w.Center, range))
                    aimSprite = w;
                break;
            }
            if (hp <= 0)
            {
                Dead(gameTime);
            }
            else if (aimSprite != null && aimSprite.hp > 0 && CheckCollides(Center, aimSprite.Center, range))
            {
                Attack(gameTime);

            }
            else if (aimSprite != null && aimSprite.hp > 0 && CheckCollides(Center, aimSprite.Center, rangeChase))
            {
                Chase(aimSprite.Center);
            }
            else
            {
                Run();
            }

            hpBar.remain = hp;
            hpBar.Update(gameTime);
            hpBar.Position = position + new Vector2(0, -80);
        }
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
           // sp.Draw(Game.Content.Load<Texture2D>("Object/Rock"), new Vector2(position.X-30, position.Y + 50), Color.White);
            if (bullets != null)
                foreach (Rock b in bullets)
                    if (b.Visible)
                        b.Draw(gameTime);
        }
    }
}