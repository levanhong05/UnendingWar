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
    public class BasicObject : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Vector2 velocity;
        public int hp, exp, gold, damage, attackTime, range, timeTraning;
        public Texture2D texture;
        public Vector2 position;
        public bool isRemove;
        public Vector2 Center;
        public Castle castle;
        protected AudioManager audio;
        protected bool isDeadSound = false;
        protected SpriteBatch sp;
        public ParticleSystem effect;
        public BasicObject aimSprite;

        public BasicObject(Game game, Castle castle)
            : base(game)
        {
            this.castle = castle;
            audio = (AudioManager)Game.Services.GetService(typeof(AudioManager));
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }
        public BasicObject(Game game)
            : base(game)
        {
            audio = (AudioManager)Game.Services.GetService(typeof(AudioManager));
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        bool isDrawDamage = false;

        public virtual void TakeDamage(int Dam)
        {
            hp -= Dam;
            takeDamage = Dam;
            posY = (int)position.Y;
            time2 = 0;
            audio.PlayTakeDamageSound();
            color = 1f;
            isDrawDamage = true;
        }
        
        public bool CheckCollides(Vector2 p1, Vector2 p2, float eps)
        {
            return Vector2.Distance(p1, p2) < eps;
        }

        public void Remove()
        {

            isRemove = true;
            Visible = Enabled = false;
            position = new Vector2(500, -1000);
            velocity = Vector2.Zero;
            Dispose(true);
        }
        public override void Update(GameTime gameTime)
        {
            if (hp <= 0 && !isDeadSound)
            {
                color = 1f;
                audio.PlayDeadSound();
                isDeadSound = true;
                posY = (int)position.Y;
                time2 = 0;
            }

            base.Update(gameTime);
        }

        int posY;int time2;float color = 1f;int takeDamage = 0;
        public override void Draw(GameTime gameTime)
        {

            if (hp <= 0)
            {
                
                if (time2 < 4000)
                {
                    sp.DrawString(Game.Content.Load<SpriteFont>("point"), "+Gold: " + this.gold / 2 + "\n" + "+Exp: " + this.exp, new Vector2(position.X, posY--), castle.position.X < 625 ? new Color(0f, 0f, 0f, color) : new Color(1f, 1f, 1f, color));
                    color -= 0.005f;
                }
                else
                {
                    time2 += gameTime.ElapsedGameTime.Milliseconds;
                    color = 1f;
                }
            } else
                if (isDrawDamage)
                {

                    if (time2 < 4000&& castle!=null)
                    {
                        sp.DrawString(Game.Content.Load<SpriteFont>("point"), "-" + takeDamage + " HP!", new Vector2(position.X, posY--), castle.position.X < 625 ? new Color(1f, 0f, 0f, color) : new Color(0f, 1f, 0f, color));
                        color -= 0.005f;
                    }
                    else
                    {
                        isDrawDamage = false;
                        time2 += gameTime.ElapsedGameTime.Milliseconds;
                        takeDamage = 0;
                        color = 1f;
                    }
                }
            base.Draw(gameTime);
            if (texture != null)
                sp.Draw(texture, position, Color.White);
        }
    }
}