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
        public class ExplosionSmokeParticleSystem : ParticleSystem
        {
            public ExplosionSmokeParticleSystem(UnendingWar game, int howManyEffects, Texture2D t)  
                : base(game, howManyEffects, t)
            {
            }
            protected override void InitializeConstants()
            {
                minInitialSpeed = 20;
                maxInitialSpeed = 200;
                minAcceleration = -10;
                maxAcceleration = -50;
                minLifetime = 1.0f;
                maxLifetime = 2.5f;
                minScale = 1.0f;
                maxScale = 2.0f;
                minNumParticles = 6;
                maxNumParticles = 12;
                minRotationSpeed = -MathHelper.PiOver4;
                maxRotationSpeed = MathHelper.PiOver4;
                blendState = BlendState.AlphaBlend;
                DrawOrder = AlphaBlendDrawOrder;
            }
        }
}
