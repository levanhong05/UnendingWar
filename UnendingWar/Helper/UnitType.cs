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
    public class UnitType
    {
        //Đơn vị chiến đấu
        public enum Unit
        {
            Worker = 0,
            Warrior,
            Orc,
            Titan,
            Wizard,
            Gobin,
            Catapult,
            Ghost
        }
        public static int[] HP =     {40,50 ,  110, 250, 200,  80 ,400,220};
        public static int[] Damage = {5,15 ,   18, 035, 030, 010,70,25};
        public static int[] Gold =   {50, 25 ,   70, 200, 350, 50,500,200};
        public static int[] EXP =    {10,15 ,35 , 100, 150, 25,300,100};
        public static int[] AttackTime = {4500,1500  , 1500, 1200, 1000, 1500,1000,1200};
        public static int[] attackRange = {40,50 , 60, 70, 350,  250,510,50};
        public static float[] Speed = {1f, 0.8f, 0.7f, 2f, 1.2f, 0.85f,1f,2.5f };
        public static int[] TimeTraning = {3000, 1000, 2100, 3000, 4500, 1500,7000,2000 };
        public static int[] RangeChase = { 0,300, 350, 400, 450, 350 ,1000,300};
        
        //Thành trì
        public enum CastleStatus
        {
            NotUpdated = 0,
            Updated
        }
        public static int[] Gain = { 2 , 5};
        public static int[] CastleHp = { 3000, 3000 };
        public static int[] CastleDamage = { 2, 5 };
        public static int[] CastleAttackTime = { 1500, 1000 };
        public static int[] CastleRange = { 500, 500 };
        
        //Đơn vị bảo vệ
        public enum ProtectType
        {
            Human = 0,
            HumanUpdated,
        }
        public static int[] Range = { 420,  500 };
        public static int[] ProtectorDamage = { 6,  15};
        public static int[] ProtectorAttackTime = { 1000, 700};
        public static int[] Cost = { 500, 1500};
        public static int[] ProtectorHP = { 2500, 5500};
        public static int[] ProtectorEXP = { 300, 1000 }; 

    }
}
