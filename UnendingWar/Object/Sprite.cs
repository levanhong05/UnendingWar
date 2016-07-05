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
    public class Sprite : BasicObject
    {

        protected Bar hpBar;
        Color color;
        
        protected enum Status
        {
            Run,
            Attack,
            Die
        }
        List<BasicObject> enemys;
        public List<BasicObject> Enemys
        {
            get { return enemys; }
            set { enemys = value; }
        }
        protected Status status = Status.Run;

        internal Animation attack, run, die;

        internal AnimationPlayer animationPlayer;
        internal AnimationPlayer AnimationPlayer
        {
            get { return animationPlayer; }
        }
        protected UnitType.Unit type;


        public int rangeChase;

        public BasicObject Enemy
        {
            get { return aimSprite; }
            set { aimSprite = value; }
        }
        protected bool isFire = false;

        protected float speed = 0f;

        protected bool isWorker = false;
        public Sprite(Game game)
            : base(game)
        {
        }
        public Sprite(Game game, UnitType.Unit type, Castle castle, ParticleSystem effect)
            : base(game, castle)
        {
            this.effect = effect;
            attackTime = UnitType.AttackTime[(int)status];
            
            speed = UnitType.Speed[(int)type];
            if (castle.position.X > 625)
            {
                this.position = new Vector2(1200, 200 + Helper.GetRandomInt(400));
                color = new Color(150, 200, 250, 225);
                velocity = new Vector2(-speed, 0);
            }
            else
            {
                this.position = new Vector2(50, 200 + Helper.GetRandomInt(400));
                color = new Color(250, 200, 150, 255);
                velocity = new Vector2(speed, 0);
            }
            
            rangeChase = UnitType.RangeChase[(int)type];

            run = new Animation(game.Content.Load<Texture2D>("Object/" + type.ToString() + "Run"), 0.15f, true);
            //run.FrameTime = attackTime / (1000 * run.FrameCount);
            attack = new Animation(game.Content.Load<Texture2D>("Object/" + type.ToString() + "Attack"), 0.15f, true);
           // attack.FrameTime = attackTime / (1000 * attack.FrameCount);
            die = new Animation(game.Content.Load<Texture2D>("Object/" + type.ToString() + "Die"), 0.15f, false);
           // die.FrameTime = attackTime / (1000 * die.FrameCount);
            hp = UnitType.HP[(int)type];
            timeTraning = UnitType.TimeTraning[(int)type];
            damage = UnitType.Damage[(int)type];
            exp = UnitType.EXP[(int)type];
            range = UnitType.attackRange[(int)type];
            gold = UnitType.Gold[(int)type];
            hpBar = new Bar(game, game.Content.Load<Texture2D>("LiveHp"), game.Content.Load<Texture2D>("LostHp"));
            hpBar.Max = hp;
            this.type = type;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public BasicObject getEnemy()
        {
            float min = 10000f;
            int index = 0;


            for (int i = 0; i < enemys.Count; i++)
                if (Vector2.Distance(enemys[i].position, position) < min && enemys[i].hp > 0)
                {
                    min = Vector2.Distance(enemys[i].position, position);
                    index = i;
                }
            return enemys[index];
        }
        protected virtual void Chase(Vector2 pos)
        {
            if (aimSprite.hp <= 0)
            {
                velocity = new Vector2((castle.position.X < 600) ? speed : -speed, 0);
                return;
            }
            animationPlayer.PlayAnimation(run);
            float dx = pos.X - position.X;
            float dy = pos.Y - position.Y;
            float theta = (float)Math.Atan(dy / dx);

            if (dx < 0)
            {
                theta -= MathHelper.Pi;
            }

            velocity = new Vector2((float)Math.Cos(theta) * speed, (float)Math.Sin(theta) * speed);
            position+= velocity;
        }
        public void Run()
        {
            animationPlayer.PlayAnimation(run);
            Vector2 lastPos = position;
            position += velocity;

            velocity = new Vector2((castle.position.X < 600) ? speed : -speed, 0);
        }
        protected int time;
        public virtual void Attack(GameTime gameTime)
        {
            animationPlayer.PlayAnimation(attack);
            if (!isFire)
            {
                audio.PlayMeleeAttackSound();
                effect.AddParticles(aimSprite.Center);
                isFire = true;
            }
            time += gameTime.ElapsedGameTime.Milliseconds;
            if (time > attackTime)
            {
                time -= attackTime;
                isFire = false;
                if (aimSprite != null)
                {
                    aimSprite.TakeDamage(damage);
                    if (aimSprite.hp <= 0)
                    {
                        castle.gold += (int)(aimSprite.gold*1.5);
                        castle.exp += aimSprite.exp;
                        castle.checkLevel();
                    }
                }
            }
        }
        
        public void Dead(GameTime gameTime)
        {
            animationPlayer.PlayAnimation(die);
            time += gameTime.ElapsedGameTime.Milliseconds;
            if (time > 5000)
            {
                time -= 5000;
                Remove();
            }
        }
        public virtual void GoForGold(GameTime gameTime){}
        public override void Update(GameTime gameTime)
        {
            if (enemys != null)
                aimSprite = getEnemy();

            Center = position;

            if (hp <= 0)
            {
                Dead(gameTime);
            }
            else if (aimSprite != null && aimSprite.hp > 0 && CheckCollides(Center, aimSprite.Center, range))
            {
                Attack(gameTime);
            }
            else if (aimSprite != null && aimSprite.hp > 0 && CheckCollides(Center, aimSprite.Center, rangeChase))
            {
                Chase(aimSprite.Center);
            }

            else
            {
                Run();
            }

            hpBar.remain = hp;
            hpBar.Update(gameTime);
            hpBar.Position = position + new Vector2(0, -80);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteEffects effect = SpriteEffects.None;

            if (velocity.X > 0)
                effect = SpriteEffects.FlipHorizontally;

            if(hp>0)
            hpBar.Draw(gameTime);
            sp.Draw(Game.Content.Load<Texture2D>("CharacterShadow"), position + new Vector2(-20, +45), Color.White);
            animationPlayer.Draw(gameTime, sp, position, effect,color,position.Y/700);

            base.Draw(gameTime);
        }
    }
}