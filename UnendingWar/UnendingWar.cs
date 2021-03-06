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
    /// This is the main type for your game
    /// </summary>
    public class UnendingWar : Microsoft.Xna.Framework.Game
    {
        Song BackSound;
        public AudioManager audio;

        Input input = new Input();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        Castle castle1, castle2;

        public ParticleSystem fire, ice;
        public ParticleSystem explosion;
        public ParticleSystem smoke;

        Vector2 lastPosition = Vector2.Zero;

        public UnendingWar()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1250;
            graphics.PreferredBackBufferHeight = 700;            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            Services.AddService(typeof(Input), input);
            
            audio = new AudioManager(this);
            Services.AddService(typeof(AudioManager), audio);
            SoundEffect.MasterVolume = 0.5f;
            Window.Title = "UnendingWar - Tran Chien Bat Tan";
            //graphics.IsFullScreen = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            castle2 = new Castle(this, new Vector2(1070, 350),true);
            castle1 = new Castle(this, new Vector2(0, 350),true);
                        
            Components.Add(castle1);
            Components.Add(castle2);

            smoke = new ExplosionSmokeParticleSystem(this, 1, this.Content.Load<Texture2D>("Effect/smoke"));
            ice = new FireSmokeParticleSystem(this, 1, this.Content.Load<Texture2D>("Effect/ice"));
            fire = new FireSmokeParticleSystem(this, 1, this.Content.Load<Texture2D>("Effect/fire"));
            explosion = new ExplosionParticleSystem(this, 1, this.Content.Load<Texture2D>("Effect/explosion"));
            
            Components.Add(smoke);
            Components.Add(ice);
            Components.Add(fire);
            Components.Add(explosion);

            BackSound = Content.Load<Song>("Audio/BackSound");

            this.IsMouseVisible = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
           
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(BackSound);

            input.Update();
            castle1.Enemy = castle2;
            castle2.Enemy = castle1;

            base.Update(gameTime);
        }
        
        float color = 0f;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            if (castle1.hp <= 0 || castle2.hp <= 0)
            {
                DrawEnd();
            }
            else
            {
                DrawGamePlay(gameTime);
            }
        }
        
        public void getEffect(BlendState blendState)
        {
            spriteBatch.End();
            spriteBatch.Begin(0, blendState);
        }
        public void DrawEnd()
        {
            string i = "";
            if (castle1.hp <= 0)
                i = "The DARGON EMPIRE";
            else
                i = "The ARKDEN EMPIRE";
            spriteBatch.Begin();
            spriteBatch.DrawString(Content.Load<SpriteFont>("font"), "Congratulations!"+ i + " Has Won !!!" + "\n" + "Thank you for your playing!" + "\nPress R to reset game!", new Vector2(300, 500), Color.Red);
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Components.Clear();

                castle2 = new Castle(this, new Vector2(1000, 350),true);
                castle1 = new Castle(this, new Vector2(0, 350),true);

                castle1.Enemy = castle2;
                castle2.Enemy = castle1;
                Components.Add(castle1);
                Components.Add(castle2);
            }
            spriteBatch.End();
        }
        public void DrawGamePlay(GameTime gameTime)
        {
            if (color <= 1)
                color += 0.01f;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(Content.Load<Texture2D>("battleGround"), Vector2.Zero, new Color(color, color, color, color));
            if (color >= 0.6f)
                base.Draw(gameTime);
            spriteBatch.DrawString(Content.Load<SpriteFont>("font"), "Gold: " + castle1.gold + "$" + "|" + "Exp: " + castle1.exp + "/" + castle1.Maxexp + "|" + "CastleLevel: " + castle1.level + "|" + "CastleHP: " + castle1.hp, new Vector2(0, 150), Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>("font"), "Gold: " + castle2.gold + "$" + "|" + "Exp: " + castle2.exp + "/" + castle2.Maxexp + "|" + "CastleLevel: " + castle2.level + "|" + "CastleHP: " + castle2.hp, new Vector2(650, 150), Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>("font"), castle1.Message, new Vector2(100, 5), Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>("font"), castle2.Message, new Vector2(700, 5), Color.White);
            spriteBatch.End();
        }
        
    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (UnendingWar game = new UnendingWar())
            {
                game.Run();
            }
        }
    }
}
