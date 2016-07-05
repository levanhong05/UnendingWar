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
    /// Game was created by huyetSat 
    /// </summary>
    public class Protector : BasicObject
    {
        Bar hpBar;

        UnitType.ProtectType type;

        List<BasicObject> listEnemy;

        List<Bullet> bullets = new List<Bullet>();

        public bool isBuy = false;

        public List<BasicObject> Enemys
        {
            get { return listEnemy; }
            set { listEnemy = value; }
        }

        UnendingWar game;
        public Protector(UnendingWar game, UnitType.ProtectType type, Castle castle, ParticleSystem effect)
            : base(game,castle)
        {

            this.game = game;
            this.effect = effect;
            this.hp = UnitType.ProtectorHP[(int)type];
            if(castle.position.X<625)
                this.position = castle.position + new Vector2(200, (type == UnitType.ProtectType.Human) ? 50 : -100);
            else
                this.position = castle.position + new Vector2(-100, (type == UnitType.ProtectType.Human) ? 50 : -100);
            attackTime = UnitType.ProtectorAttackTime[(int)type];
            range = UnitType.Range[(int)type];
            damage = UnitType.ProtectorDamage[(int)type];
            gold = UnitType.Cost[(int)type];
            exp = UnitType.ProtectorEXP[(int)type];
            texture = game.Content.Load<Texture2D>("Protector/" + type.ToString());
            Center = position + new Vector2(texture.Width / 2, texture.Height / 2);
            
            this.type = type;
            hpBar = new Bar(game, game.Content.Load<Texture2D>("LiveHp"), game.Content.Load<Texture2D>("LostHp"));
            hpBar.Max = hp;
            
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
        int time;
        bool isFire = false;
        public void Attack(GameTime gameTime)
        {
            
            if (!isFire)
            {
                audio.PlayProtectorAttackSound();
                Bullet b = new Bullet(Game, position+new Vector2(texture.Width/2,texture.Height/2), Game.Content.Load<Texture2D>("Bullet/protectorBullet" + type.ToString()),this);

                isFire = true;
                bullets.Add(b);
            }
            time += gameTime.ElapsedGameTime.Milliseconds;
            if (time > attackTime)
            {
                time -= attackTime;
                isFire = false;

            }
        }
        BasicObject getAimSprite(List<BasicObject> list)
        {
            BasicObject sp = new BasicObject(Game);
            foreach (BasicObject s in list)
            {
                if (CheckCollides(position, s.position, range)&&s.hp>0)
                    return s;
            }
            return sp;
        }
        public override void TakeDamage(int Dam)
        {
            base.TakeDamage(Dam);

            audio.BuildingTakeDamage.Play();
        }
        public override void Update(GameTime gameTime)
        {
            
            if (hp <= 0)
            {
                if (!isDeadSound)
                    audio.BuildingDestroy.Play();
                isDeadSound = true;

                return;
            }
            hpBar.Position = position;
            Center = position + new Vector2(texture.Width / 2, texture.Height / 2);

            if (listEnemy!=null)
                aimSprite = getAimSprite(listEnemy);
            else
                aimSprite = null;

            if (aimSprite != null&&CheckCollides(position,aimSprite.position,range))
                Attack(gameTime);

            
            hpBar.remain = hp;
            hpBar.Update(gameTime);
            if (hp < 1000)
            {
                game.smoke.AddParticles(Center + new Vector2(Helper.GetRandomInt(10) - 20, Helper.GetRandomInt(80) - 40));
                game.smoke.AddParticles(Center + new Vector2(Helper.GetRandomInt(10) - 20, Helper.GetRandomInt(80) - 40));
                if (hp < 500)
                    //Boc lua neu hp < 500;
                    game.fire.AddParticles(Center + new Vector2(Helper.GetRandomInt(10) - 20, Helper.GetRandomInt(80) - 40));
             
            }
            base.Update(gameTime);
            if (bullets != null)
                foreach (Bullet b in bullets)
                    if (b.Enabled)
                    {
                        
                        b.Update(gameTime);
                    }
        }

        public override void Draw(GameTime gameTime)
        {
            if (hp <= 0)
                return;

            sp.Draw(texture, position,new Rectangle(0,0,texture.Width,texture.Height), Color.White,0f,Vector2.Zero,1f,(position.X<625)?SpriteEffects.None:SpriteEffects.FlipHorizontally,1.0f);
            if (hp > 0)
                hpBar.Draw(gameTime);
            base.Draw(gameTime);
            if (bullets != null)
                foreach (Bullet b in bullets)
                    if (b.Visible)
                        b.Draw(gameTime);
        }
    }
}