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
    public class Bar : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public int Width = 50;
        Rectangle source = new Rectangle(0, 0, 50, 8);
        public string Label;
        public Vector2 Position;
        public Texture2D live, lost;
        public int remain, Max;
        //service:
        SpriteBatch sBatch;

        public Bar(Game game,Texture2D live,Texture2D lost)
            : base(game)
        {
            this.live = live;
            this.lost = lost;
            // lay services tu RPG game
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            source = new Rectangle(0, 0, Width, Height);
            sBatch.Draw(lost, Position, source, Color.White);
            if (Max == 0)
                Max = 1000;
            sBatch.Draw(live, Position, new Rectangle(0,0,Width*(int)remain/Max,Height), Color.White);
            base.Draw(gameTime);
        }
        public int Height = 8;
    }
}