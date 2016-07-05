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
    public class Tab : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public bool isSelected = false;
        public int buttonIndexMax = 3;
        public int index = 0;

        Texture2D FG,Select,NoSelect;
        SpriteBatch sp;
        Input input;
        Vector2 position;

        List<Button> buttons = new List<Button>();
        public List<Button> Buttons
        {
            get { return buttons; }
            set { buttons = value; }
        }
        public String genre;
        public Tab(Game game,Vector2 pos,String type,String genre)
            : base(game)
        {
            this.genre = genre;
            position = pos;
            Select = game.Content.Load<Texture2D>("GUI/TabSelected");
            NoSelect = game.Content.Load<Texture2D>("GUI/TabNoSelected");
            FG = game.Content.Load<Texture2D>("GUI/"+type);
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            input = (Input)Game.Services.GetService(typeof(Input));
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
            foreach (Button b in buttons)
                b.isSelected = false;

            buttons[index].isSelected = true;

            if(genre=="Human")
            {
                if (input.Release(Keys.D))
                {
                    index++;
                    if (index == buttonIndexMax+1)
                        index = 0;
                }
                else
                    if (input.Release(Keys.A))
                    {
                        index--;
                        if (index == -1)
                            index = buttonIndexMax;
                    }
            }
            else
            {
                if (input.Release(Keys.Right))
                {
                    index++;
                    if (index == buttonIndexMax + 1)
                        index = 0;
                }
                else
                    if (input.Release(Keys.Left))
                    {
                        index--;
                        if (index == -1)
                            index = buttonIndexMax;
                    }
            }

            index = (int)MathHelper.Clamp(index, 0, buttonIndexMax);

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (isSelected)
            {
                foreach (Button b in buttons)
                    b.Draw(gameTime);
                sp.Draw(Select, position, Color.White);
            }
            else
                sp.Draw(NoSelect, position, Color.White);

            sp.Draw(FG, position, Color.White);
            base.Draw(gameTime);
        }
    }
}