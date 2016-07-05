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
        public class ExplosionParticleSystem : ParticleSystem
        {
            public ExplosionParticleSystem(UnendingWar game, int howManyEffects, Texture2D t)
                : base(game, howManyEffects, t)
            {
            }
            protected override void InitializeConstants()
            {
                minInitialSpeed = 05;
                maxInitialSpeed = 30;
                minAcceleration = 0;
                maxAcceleration = 0;
                minLifetime = .2f;
                maxLifetime = 0.7f;
                minScale = .3f;
                maxScale = 1.0f;
                minNumParticles = 5;
                maxNumParticles = 10;
                minRotationSpeed = -MathHelper.PiOver4;
                maxRotationSpeed = MathHelper.PiOver4;
                blendState = BlendState.Additive;
                DrawOrder = AdditiveDrawOrder;
            }

            protected override void InitializeParticle(Particle p, Vector2 where)
            {
                base.InitializeParticle(p, where);
                p.acceleration = -p.velocity / p.lifetime;
            }
        }
}
