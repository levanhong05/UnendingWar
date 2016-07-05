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
    public class Table : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D BG;
        SpriteBatch sp;
        Input input;
        Vector2 position;

        public int index = 0;

        List<Tab> tabs = new List<Tab>();
        public List<Tab> Tabs
        {
            get { return tabs; }
            set { tabs = value; }
        }

        public Table(Game game,Vector2 pos,string type)
            : base(game)
        {
            position = pos;
            BG = game.Content.Load<Texture2D>("GUI/GUIBG");
            sp = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            input = (Input)Game.Services.GetService(typeof(Input));
            //Tab mua quan
            Tab army = new Tab(game, position, "Army",type);
            int index = 0;
            Button b = new Button(game, position + new Vector2(100, 50),  index.ToString() + "Avatar");
            army.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100+index*130, 50),  index.ToString() + "Avatar");
            army.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100 + index * 130, 50),  index.ToString() + "Avatar");
            army.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100 + index * 130, 50),  index.ToString() + "Avatar");
            army.Buttons.Add(b);
            tabs.Add(army);
            //Tab mua protector
            Tab protector = new Tab(game, position + new Vector2(0, 50), "Build", type);
            index = 0;
            b = new Button(game, position + new Vector2(100 + index * 130, 50), (4+ index).ToString() + "Avatar");
            protector.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100 + index * 130, 50), (4 + index).ToString() + "Avatar");
            protector.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100 + index * 130, 50), (4 + index).ToString() + "Avatar");
            protector.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100 + index * 130, 50), (4 + index).ToString() + "Avatar");
            protector.Buttons.Add(b);
            tabs.Add(protector);
            //Tab Update
            Tab Update = new Tab(game, position + new Vector2(0, 100), "Update", type);
            index = 0;
            b = new Button(game, position + new Vector2(100, 50), (8 + index).ToString() + "Avatar");
            Update.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100 + index * 130, 50), (8 + index).ToString() + "Avatar");
            Update.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100 + index * 130, 50), (8 + index).ToString() + "Avatar");
            Update.Buttons.Add(b);
            index++;
            b = new Button(game, position + new Vector2(100 + index * 130, 50), (8 + index).ToString() + "Avatar");
            Update.Buttons.Add(b);
            tabs.Add(Update);

            this.type = type;
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

        string type;
        public override void Update(GameTime gameTime)
        { 
            //Dieu khien table
            if(type=="Human")
            {
                if (input.Release(Keys.W))
                    index--;
                else if (input.Release(Keys.S))
                    index++;
                index = (int)MathHelper.Clamp(index, 0, 2);
                
            }
            else
            {
                if (input.Release(Keys.Up))
                    index--;
                else if (input.Release(Keys.Down))
                    index++;

                index = (int)MathHelper.Clamp(index, 0, 2);

            }
            foreach (Tab t in tabs)
            {
                t.isSelected = false;
                foreach (Button b in t.Buttons)
                    b.isSelected = false;

            }

            tabs[index].isSelected = true;

            tabs[index].Update(gameTime);

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            sp.Draw(BG, position,new Rectangle(0,0,BG.Width,BG.Height), Color.White,0f,Vector2.Zero,1f,SpriteEffects.None,0.2f);

            foreach (Tab t in tabs)
                t.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}