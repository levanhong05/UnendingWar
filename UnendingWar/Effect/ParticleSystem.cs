﻿using System;
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
        public abstract class ParticleSystem : DrawableGameComponent
        {
            private Texture2D texture;
            private Vector2 origin;
            private static Random random = new Random();
            public static Random Random
            {
                get { return random; }
            }
            private int howManyEffects;
            Particle[] particles;
            Queue<Particle> freeParticles;
            public int FreeParticleCount
            {
                get { return freeParticles.Count; }
            }
            public const int AlphaBlendDrawOrder = 100;
            public const int AdditiveDrawOrder = 200;
            protected BlendState blendState;

            UnendingWar game;
            SpriteBatch spriteBatch;
            #region constants to be set by subclasses
            protected int minNumParticles;
            protected int maxNumParticles;
            protected string textureFilename;
            protected float minInitialSpeed;
            protected float maxInitialSpeed;
            protected float minAcceleration;
            protected float maxAcceleration;
            protected float minRotationSpeed;
            protected float maxRotationSpeed;
            protected float minLifetime;
            protected float maxLifetime;
            protected float minScale;
            protected float maxScale;
            #endregion
            protected ParticleSystem(UnendingWar game, int howManyEffects, Texture2D t) : base(game)
            {
                this.game = game;
                this.howManyEffects = howManyEffects;

                spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                InitializeConstants();
                particles = new Particle[howManyEffects * maxNumParticles];
                freeParticles = new Queue<Particle>(howManyEffects * maxNumParticles);
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i] = new Particle();
                    freeParticles.Enqueue(particles[i]);
                }

                texture = t;
                origin.X = texture.Width / 2;
                origin.Y = texture.Height / 2;
            }
            protected abstract void InitializeConstants();

            public void AddParticles(Vector2 where)
            {
                int numParticles =
                    random.Next(minNumParticles, maxNumParticles);
                for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
                {
                    Particle p = freeParticles.Dequeue();
                    InitializeParticle(p, where);
                }
            }
            protected virtual Vector2 PickRandomDirection()
            {
                float angle = RandomBetween(0, MathHelper.TwoPi);
                return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            }
            public static float RandomBetween(float min, float max)
            {
                return min + (float)random.NextDouble() * (max - min);
            }
            protected virtual void InitializeParticle(Particle p, Vector2 where)
            {
                Vector2 direction = PickRandomDirection();
                float velocity = RandomBetween(minInitialSpeed, maxInitialSpeed);
                float acceleration = RandomBetween(minAcceleration, maxAcceleration);
                float lifetime = RandomBetween(minLifetime, maxLifetime);
                float scale = RandomBetween(minScale, maxScale);
                float rotationSpeed = RandomBetween(minRotationSpeed, maxRotationSpeed);
                p.Initialize(where, velocity * direction, acceleration * direction, lifetime, scale, rotationSpeed);
            }

            public override void Update(GameTime gameTime)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (Particle p in particles)
                {

                    if (p.Active)
                    {
                        p.Update(dt);
                        if (!p.Active)
                        {
                            freeParticles.Enqueue(p);
                        }
                    }
                }

                base.Update(gameTime);
            }
            public bool isPlayerExist = true;
            public override void Draw(GameTime gameTime)
            {
                game.getEffect(blendState);
                foreach (Particle p in particles)
                {
                    if (!p.Active) continue;
                    float normalizedLifetime = p.timeSinceStart / p.lifetime;
                    float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
                    float scale = p.scale * (.75f + .25f * normalizedLifetime);
                    Color color = new Color(new Vector4(1, 1, 1, alpha));
                    
                    spriteBatch.Draw(texture, p.position, null, color, p.rotation, origin, scale, SpriteEffects.None, 0.0f);
                }

                
            }
            
        }
}
