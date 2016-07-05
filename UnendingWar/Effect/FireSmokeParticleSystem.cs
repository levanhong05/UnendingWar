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
        public class FireSmokeParticleSystem : ParticleSystem
        {
            public FireSmokeParticleSystem(UnendingWar game, int howManyEffects, Texture2D t)  
                : base(game, howManyEffects, t)
            {
            }
            protected override void InitializeConstants()
            {
                minInitialSpeed = 20;
                maxInitialSpeed = 60;
                minAcceleration = 0;
                maxAcceleration = 20;
                minLifetime = 0.7f;
                maxLifetime = 1.5f;
                minScale = 0.5f;
                maxScale = 1.0f;
                minNumParticles = 4;
                maxNumParticles = 11;
                minRotationSpeed = - MathHelper.Pi/8;
                maxRotationSpeed = + MathHelper.Pi / 8;
                blendState = BlendState.Additive;
                DrawOrder = AlphaBlendDrawOrder;
            }
        }
}
