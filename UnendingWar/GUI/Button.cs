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
    public class Button : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public bool isSelected = false;

        Texture2D FG, Select, NoSelect;
        SpriteBatch sp;
        public Vector2 position;

        public Button(Game game, Vector2 pos, String type)
            : base(game)

        {
            position = pos;
            Select = game.Content.Load<Texture2D>("GUI/ButtonSelected");
            NoSelect = game.Content.Load<Texture2D>("GUI/ButtonNoSelected");
            FG = game.Content.Load<Texture2D>("GUI/" + type);
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
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
        public override void Draw(GameTime gameTime)
        {
            sp.Draw(FG, position, Color.White);

            if (isSelected)
                sp.Draw(Select, position, Color.White);
            else
                sp.Draw(NoSelect, position, Color.White);
            base.Draw(gameTime);
        }
    }
}