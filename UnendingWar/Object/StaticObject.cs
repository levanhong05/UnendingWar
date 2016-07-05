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
    public class Gem : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D t;
        public int hp;
        public Vector2 position, center;
        SpriteBatch sp;
        public Gem(Game game,Vector2 pos)
            : base(game)
        {
            position = pos;
            t = Game.Content.Load<Texture2D>("Gem/Coin");
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            center = position + new Vector2(t.Width / 2, t.Height / 2);
            hp = 1000;
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
            sp.Draw(t, position, Color.White);
            base.Draw(gameTime);
        }
    }
}