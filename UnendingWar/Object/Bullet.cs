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
    public class Bullet : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D texture;
        Animation run;
        AnimationPlayer player;

        Vector2 position,velocity;
        int speed;
        bool isAnimatedBullet = true;

        SpriteBatch sp;
        BasicObject owner;
        //For animate bullet
        public Bullet(Game game, Vector2 position, string type,BasicObject owner)
            : base(game)
        {
            this.owner = owner;
            this.position = position;
            run = new Animation(Game.Content.Load<Texture2D>("Spell/" + type), 0.08f, true);
            player.PlayAnimation(run);
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            speed = 5;
        }
        //For static texture
        public Bullet(Game game, Vector2 position, Texture2D text, BasicObject owner)
            : base(game)
        {
            this.owner = owner;
            this.position = position;
            this.texture = text;
            isAnimatedBullet = false;
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            speed = 5;
        }
        float theta = 0f;
        float angel = 0f;
        public void Chase(BasicObject sp)
        {
            if (sp.hp<=0)
                return;
            float dx = sp.Center.X - position.X;
            float dy = sp.Center.Y - position.Y;
            theta = (float)Math.Atan(dy / dx);
            
            if (dx < 0)
            {
                theta -= MathHelper.Pi;
            }
            angel = theta + MathHelper.PiOver2;
            velocity = new Vector2((float)Math.Cos(theta) * speed, (float)Math.Sin(theta) * speed);
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            player.scale = 0.1f;

            base.Initialize();
        }

        public void Remove()
        {
            velocity = Vector2.Zero;
            position = -500 * Vector2.One;
            Enabled = Visible = false;
            Dispose(true);
        }
        BasicObject aimSprite;
        public override void Update(GameTime gameTime)
        {
            if (owner!=null&&owner.hp<=0)
            {
                Remove();
                return;
            }
            if (owner != null && owner.aimSprite != null)
            {
                Chase(owner.aimSprite);
                this.aimSprite = owner.aimSprite;
                position += velocity;
                if (this.aimSprite.CheckCollides(position, this.aimSprite.Center, 30))
                {

                    Remove();
                    if (owner.effect != null)
                        owner.effect.AddParticles(this.aimSprite.Center);

                    this.aimSprite.TakeDamage(owner.damage);
                    if (this.aimSprite.hp <= 0 && owner != null && owner.castle != null)
                    {
                        owner.castle.gold += (int)(this.aimSprite.gold * 0.5f);
                        owner.castle.exp += this.aimSprite.exp;
                        owner.castle.checkLevel();
                    }


                }
            }
            

            
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteEffects effectS = SpriteEffects.None;
            if (owner != null && owner.aimSprite != null)
            if (position.X<0||position.X>1250)
            {
                Remove();
                return;
            }
            if (isAnimatedBullet)
            {
                player.scale += 0.005f;
                

                if (velocity.X > 0)
                    effectS = SpriteEffects.FlipHorizontally;
                player.Draw(gameTime, sp, position, effectS,Color.White,0.5f);
            }
            else
            {
                sp.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, angel, Vector2.Zero, 1f, effectS,0.5f);
            }
            base.Draw(gameTime);
        }
    }
}