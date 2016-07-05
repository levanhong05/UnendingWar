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
    public class Rock : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch sp;
        Texture2D t;

        Vector2 center,velocity,position;

        BasicObject aim;
        BasicObject owner;
        int range;
        AudioManager audio;
        public Rock(Game game,BasicObject owner,Castle aim,Vector2 position)
            : base(game)
        {
            this.owner = owner;
            this.aim = aim;
            this.position = position;
            velocity = owner.position.X<625?  new Vector2(8, -19):new Vector2(-8, -19);
            range = 100;
            t = Game.Content.Load<Texture2D>("Object/Rock");
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            audio = (AudioManager)Game.Services.GetService(typeof(AudioManager));
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (position.Y > 700)
            {
                Enabled = Visible = false;
                Dispose(true);
            }

            position += velocity;
          
            velocity.Y += 0.5f;

            center = new Vector2(t.Width / 2, t.Height) + position;

            if (aim.hp > 0)
            {
                if (aim.CheckCollides(aim.Center, center, range))
                {
                    aim.hp -= owner.damage;
                    owner.effect.AddParticles(center+new Vector2(Helper.GetRandomInt(20)-10,Helper.GetRandomInt(20)-10));
                    owner.effect.AddParticles(center + new Vector2(Helper.GetRandomInt(20) - 10, Helper.GetRandomInt(20) - 10));
                    if (aim.hp <= 0)
                    {
                        owner.castle.exp += aim.exp;
                        owner.castle.gold += aim.gold;
                        audio.PlayDeadSound();
                        owner.castle.checkLevel();
                    }
                    position = -700 * Vector2.One;
                    Enabled = Visible = false;
                    Dispose(true); 
                }
            }
            
            base.Update(gameTime);
        }
        float angel = 0f;
        public override void Draw(GameTime gameTime)
        {
            sp.Draw(t, position,new Rectangle(0,0,t.Width,t.Height),  Color.White,angel,new Vector2 (t.Width/2,t.Height/2),1f,SpriteEffects.None,0f);
            angel+=0.5f;
            base.Draw(gameTime);
        }
    }
}