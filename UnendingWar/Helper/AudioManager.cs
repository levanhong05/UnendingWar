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
    public class AudioManager : Microsoft.Xna.Framework.GameComponent
    {
        public SoundEffect BuildingDestroy, BuildingTakeDamage, Updated;
        public AudioManager(Game game)
            : base(game)
        {
            BuildingDestroy = game.Content.Load<SoundEffect>("Audio/BuildingDestroying");
            BuildingTakeDamage = game.Content.Load<SoundEffect>("Audio/BuidingTakeDamage");
            Updated = game.Content.Load<SoundEffect>("Audio/Updated");
        }

        private SoundEffect getSound(string s, int rd)
        {
            return Game.Content.Load<SoundEffect>("Audio/" + s + Helper.GetRandomInt(rd));
        }
        public void PlayTakeDamageSound()
        {
            getSound("TakeDamage", 4).Play();
        }
        public void PlayDeadSound()
        {
            getSound("Dead", 5).Play();
        }
        public void PlayRangeAttackSound()
        {
            getSound("RangeAttack", 5).Play();
        }
        public void PlayProtectorAttackSound()
        {
            getSound("ProtectorAttack", 5).Play();
        }
        public void PlayMeleeAttackSound()
        {
            getSound("MeleeAttack", 5).Play();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}