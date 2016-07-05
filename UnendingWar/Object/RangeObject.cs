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
    public class RangeObject : Sprite
    {

        List<Bullet> bullets = new List<Bullet>();

        public RangeObject(Game game, UnitType.Unit type, Castle castle, ParticleSystem effect)
            : base(game, type, castle, effect)
        {
            // TODO: Construct any child components here
        }


        public override void Attack(GameTime gameTime)
        {
            animationPlayer.PlayAnimation(attack);
            if (!isFire)
            {
                audio.PlayRangeAttackSound();
                
                Bullet b = new Bullet(Game, position, type.ToString()+"Spell",this);
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
                foreach (Bullet b in bullets)
                    if (b.Enabled)
                    {
                        b.Update(gameTime);
                    }

            
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);

            if (bullets != null)
                foreach (Bullet b in bullets)
                    if (b.Visible)
                        b.Draw(gameTime);
        }
    }
}