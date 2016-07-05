using System;
using System.Collections.Generic;
using System.Text;

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
///
///
namespace UnendingWar
{
    static class Helper
    {
        static Random random = new Random();

        public static int GetRandomInt(int num)
        {
            random.GetHashCode();
            return random.Next(num);
        }        
    }
}
