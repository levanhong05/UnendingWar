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
    public class Castle : BasicObject
    {
        List<Sprite> ghosts = new List<Sprite>();
        bool isUpdatedWall = false;
        public UnitType.CastleStatus status;
        public int Maxexp = 500;
        Bar hpBar;
        int Gain;
        public int level = 0;
        public Texture2D castleTexture;

        string type;
        public List<Gem> gems = new List<Gem>();
        List<Worker> workers = new List<Worker>();
        int time;
        List<Sprite> army = new List<Sprite>();
        List<Protector> protector = new List<Protector>();
        Table GUI;
        Input input = new Input();
        public List<Protector> Protectors
        {
            get { return protector; }
            set { protector = value; }
        }
        public List<Sprite> Army
        {
            get { return army; }
            set { army = value; }
        }
        UnendingWar game;
        public string Message;
        Castle enemy;
        public Castle Enemy
        {
            get { return enemy; }
            set { enemy = value;
            
            }
        }
        //List<Bullet> bullets = new List<Bullet>();
        Bar process;
        int timeTraining = 0;
        bool buyable = true;
        bool haveGem = true;
        //Phim action
        Keys key;
        Protector p, pu;

        int WorkerNumber = 0;

        public List<BasicObject> wall = new List<BasicObject>();

        public Castle(UnendingWar game,Vector2 position,bool haveGem)
            : base(game)
        {
            this.haveGem = haveGem;
            gold = 200;
            this.game = game;
            this.type = "Human";
            this.position = position;
            
            this.status = UnitType.CastleStatus.NotUpdated;
            castleTexture = Game.Content.Load<Texture2D>("Castle/" + type + "Castle");

            hp = UnitType.CastleHp[(int)status];
            Gain = UnitType.Gain[(int)status];
            damage = UnitType.CastleDamage[(int)status];
            range = UnitType.CastleRange[(int)status];
            attackTime = UnitType.CastleAttackTime[(int)status];
            
            input = (Input)Game.Services.GetService(typeof(Input));
            this.Center = position + new Vector2(castleTexture.Width / 2, castleTexture.Height / 2);
            GUI = new Table(game, (position.X < 625) ? Vector2.Zero : new Vector2(650, 0),(position.X < 625) ? type:"Undead");

            process = new Bar(game, game.Content.Load<Texture2D>("LiveHp"), game.Content.Load<Texture2D>("LostHp"));
            process.Position = (position.X < 625) ? new Vector2(150, 35) : new Vector2(800, 35);
            process.Width = 300;
            process.Height = 10;

            hpBar = new Bar(game, game.Content.Load<Texture2D>("LiveHp"), game.Content.Load<Texture2D>("LostHp"));
            hpBar.Max = hp;
            hpBar.Position = position + new Vector2(position.X < 625?0:100, -80);
            //gem init()
            
            gems.Add(new Gem(game, position + new Vector2(position.X < 625 ? 573 : -427, 100)));
            gems.Add(new Gem(game, position + new Vector2(position.X < 625 ? 200 : -60, -200)));
            gems.Add(new Gem(game, position + new Vector2(position.X < 625 ? 0 : 120, -200)));
            key = position.X<625?Keys.Space:Keys.Enter;

            //init protector
            p = new Protector(game, UnitType.ProtectType.Human, this, game.ice);
            pu = new Protector(game, UnitType.ProtectType.HumanUpdated, this, game.fire);
            p.isBuy = pu.isBuy = false;
            //Init Wall
            for (int i = 3; i < 14; i++)
            {
                BasicObject w = new BasicObject(game, this);
                w.texture = Game.Content.Load<Texture2D>("Wall/Castle" + status.ToString());
                w.position = new Vector2(position.X < 625 ? Center.X + 170 : Center.X - 300, i * 50);
                w.Center = w.position + new Vector2(w.texture.Width / 2, w.texture.Height / 2);
                w.hp = 1000;
                wall.Add(w);
            }
        }
        public override void Initialize()
        {
            
            base.Initialize();
        }
        public void setPoint()
        {
            status = UnitType.CastleStatus.Updated;
            castleTexture = Game.Content.Load<Texture2D>("Castle/" + type + "CastleUpdated");
            this.Center = position + new Vector2(castleTexture.Width / 2, castleTexture.Height / 2);
            hp += UnitType.CastleHp[(int)status];
            Gain = UnitType.Gain[(int)status];
            damage = UnitType.CastleDamage[(int)status];
            range = UnitType.CastleRange[(int)status];
            attackTime = UnitType.CastleAttackTime[(int)status];
            
        }
        public void UpdateWall()
        {
            //Init Wall
            wall = new List<BasicObject>();
            for (int i = 3; i < 14; i++)
            {
                isUpdatedWall = true;
                BasicObject w = new BasicObject(game, this);
                w.texture = Game.Content.Load<Texture2D>("Wall/CastleUpdated");
                w.position = new Vector2(position.X < 625 ? Center.X + 170 : Center.X - 300, i * 50);
                w.Center = w.position + new Vector2(w.texture.Width / 2, w.texture.Height / 2);
                w.hp = 2000;
                wall.Add(w);
            }
        }
        
        void AddCatapult( ParticleSystem effect)
        {
            Sprite sp = null;
            sp = new Catapult(game, UnitType.Unit.Catapult, this, effect);
            if (input.Release(key))
            {
                if (this.gold >= sp.gold)
                {
                    timeTraining = sp.timeTraning;
                    buyable = false;
                    this.gold -= sp.gold;
                    this.army.Add(sp);
                }
            }

            Message = "Cost of Capapult" + " is " + sp.gold.ToString() + "$";
        }
        void AddWorker(ParticleSystem effect)
        {
            Worker sp = null;
            sp = new Worker(game,UnitType.Unit.Worker, this,game.smoke);
            if (input.Release(key))
            {
                if (this.gold >= sp.gold)
                {
                    timeTraining = sp.timeTraning;
                    buyable = false;
                    this.gold -= sp.gold;
                    workers.Add(sp);
                    army.Add(sp);
                }
            }

            Message = "Cost of Worker" + " is " + sp.gold.ToString() + "$";
        }
        void AddGhost(ParticleSystem effect)
        {
            Sprite sp = null;
            sp = new Sprite(game, UnitType.Unit.Ghost, this, effect);
            if (input.Release(key))
            {
                if (this.gold >= sp.gold)
                {
                    timeTraining = sp.timeTraning;
                    buyable = false;
                    this.gold -= sp.gold;
                    ghosts.Add(sp);
                }
            }

            Message = "Cost of Ghost" + " is " + sp.gold.ToString() + "$";
        }
        void AddObject( UnitType.Unit typeUnit, bool isRange, ParticleSystem effect)
        {
            Sprite sp = null;
            if (input.Release(key))
            {
                if (isRange)
                    sp = new RangeObject(Game, typeUnit, this, effect);
                else
                    sp = new Sprite(Game, typeUnit, this, effect);

                if (this.gold >= sp.gold)
                {
                    timeTraining = sp.timeTraning;
                    buyable = false;
                    this.gold -= sp.gold;
                    this.army.Add(sp);
                }
            }

                if (isRange)
                    sp = new RangeObject(Game, typeUnit, this, effect);
                else
                    sp = new Sprite(Game, typeUnit, this, effect);

                Message = "Cost of "+typeUnit.ToString() + " is " + sp.gold.ToString() + "$";
        }
        void AddBuilding( UnitType.ProtectType type, ParticleSystem effect, int build)
        {
            if (input.Release(key))
            {
                if (type == UnitType.ProtectType.Human)
                {
                    if (this.gold >= p.gold)
                    {
                        this.gold -= p.gold;
                        p = new Protector(game, UnitType.ProtectType.Human, this, effect);
                        p.isBuy = true;
                        protector.Add(p);
                    }
                }
                else
                {
                    if (this.gold >= pu.gold)
                    {
                        this.gold -= pu.gold;
                        pu = new Protector(game, UnitType.ProtectType.HumanUpdated, this,effect);
                        pu.isBuy = true;
                        protector.Add(pu);
                    }
                }
            }

           Message = "Cost of " + type.ToString() + "Protector" + " is " +(type== UnitType.ProtectType.Human? p.gold:pu.gold).ToString() + "$";
        }
        int timeAtt = 0;
        //KT DK len level cho castle
        public void checkLevel()
        {
            if (exp >= Maxexp)
            {
                level++;
                exp -= Maxexp;
                Maxexp = (int)(Maxexp*1.4f);
                hp += level * 1000;
            }
        }
        int training = 0;
        //tra ve` gem gan nhat
        
        public override void Update(GameTime gameTime)
        {
            if (timeTraining != 0 && !buyable)
            {
                training += gameTime.ElapsedGameTime.Milliseconds;
                if (training > timeTraining)
                {
                    training = 0;
                    buyable = true;
                }
            }
            if (hp <= 0)
            {
                if (!isDeadSound)
                    audio.BuildingDestroy.Play();
                isDeadSound = true;

                return;
            }
            //Update Castle 's point
            time += gameTime.ElapsedGameTime.Milliseconds;
            if (time > 1000)
            {
                time -= 1000;
                hp--;
                exp++;
                gold += Gain;
            }

            //Buoc khoi neu HP < 2000
            if (hp < 2000)
            {
                game.smoke.AddParticles(Center + new Vector2(Helper.GetRandomInt(80) - 40, Helper.GetRandomInt(80) - 40));
                game.smoke.AddParticles(Center + new Vector2(Helper.GetRandomInt(80) - 40, Helper.GetRandomInt(80) - 40));
                game.smoke.AddParticles(Center + new Vector2(Helper.GetRandomInt(80) - 40, Helper.GetRandomInt(80) - 40));
                if (hp < 1000)
                {
                    //Boc lua neu hp < 1000;
                    game.fire.AddParticles(Center + new Vector2(Helper.GetRandomInt(80) - 40, Helper.GetRandomInt(80) - 40));
                    game.fire.AddParticles(Center + new Vector2(Helper.GetRandomInt(80) - 40, Helper.GetRandomInt(80) - 40));
                }
            }
            //Up sl worker
            WorkerNumber = 0;

            foreach (Worker w in workers)
            {
                
                if (w.hp > 0)
                    WorkerNumber++;
            }
            //Update Castle 's GUI
            Message = "";
            for(int i = 0;i< 4;i++)
                for (int j = 0; j < 3; j++)
                {
                    if (GUI.Tabs[j].Buttons[i].isSelected)
                    {
                        switch (j)
                        {
                            case 0:
                                if (buyable)
                                    switch (i)
                                    {
                                        case 0:
                                            AddObject(  UnitType.Unit.Warrior , false, game.explosion);
                                            break;
                                        case 1:
                                            AddObject( UnitType.Unit.Gobin, true, game.fire);
                                            break;
                                        case 2:
                                            AddObject( UnitType.Unit.Orc , false,  game.smoke);
                                            break;
                                        case 3:
                                            if(!p.isBuy)
                                            AddBuilding(UnitType.ProtectType.Human, game.ice, 0);
                                            else
                                                Message = "You have Built protector already!";
                                            break;
                                    }
                                else
                                    Message = "Training..., please wait!";
                                break;
                            case 1:
                                if(buyable)
                                {
                                    if(status == UnitType.CastleStatus.Updated)
                                    {
                                    switch (i)
                                        {
                                         case 0:
                                            AddObject(UnitType.Unit.Titan, false, game.explosion);
                                            break;
                                         case 1:
                                            AddObject(  UnitType.Unit.Wizard ,true,  game.ice);
                                            break;
                                        case 2:
                                            AddCatapult(game.fire);
                                            break;
                                         case 3:
                                            if (!pu.isBuy)
                                                AddBuilding(UnitType.ProtectType.HumanUpdated, game.explosion, 1);
                                            else
                                                Message = "You have Built UpdatedProtector already!";
                                            break;
                                        }
                                    }
                                    else
                                        Message = "You Must Upgrade Your Castle!";
                                }
                                 else
                                    Message = "Training..., please wait!";
                                break;
                            case 2:
                                switch (i)
                                {
                                    case 0:
                                        if (buyable)
                                        {
                                            //toi da co 5 worker trong 1 castle
                                            if (WorkerNumber < 6)
                                                AddWorker(game.smoke);
                                            else
                                                Message = "Too many Worker! You can't create more!";
                                        }
                                        else
                                            Message = "Training..., please wait!";
                                        break;
                                    case 1:
                                        Message = "Cost of Upgrade my Castle is 1500$";
                                        if (input.Release((position.X<625)?Keys.Space:Keys.Enter)&&gold >= 1500 && status == UnitType.CastleStatus.NotUpdated)
                                        {
                                            audio.Updated.Play();
                                            gold -= 1500;
                                            status = UnitType.CastleStatus.Updated;
                                            setPoint();
                                        }
                                        if (status == UnitType.CastleStatus.Updated)
                                            Message = "You have upgraded your castle already!";
                                        break;
                                    case 2:
                                        Message = "Cost of Upgrade the Wall is 1200$";
                                        if (input.Release((position.X < 625) ? Keys.Space : Keys.Enter) && gold >= 1200 && !isUpdatedWall)
                                        {
                                            audio.Updated.Play();
                                            gold -= 1200;
                                            UpdateWall();
                                        }
                                        if (isUpdatedWall)
                                            Message = "You have upgraded the wall already!";
                                        break;
                                    case 3:
                                        if (buyable)
                                        {
                                            if (status == UnitType.CastleStatus.Updated)
                                                AddGhost(game.smoke);
                                            else
                                                Message = "You Must Upgrade Your Castle!";
                                        }
                                        else
                                            Message = "Training..., please wait!";
                                        break;
                                }
                                break;
                        }
                    }
                }
            GUI.Update(gameTime);

            //Castle Attack!
            foreach (BasicObject b in enemy.getTotal())
            {
                if (b.hp > 0 && b.CheckCollides(Center, b.Center, range))
                {
                    this.aimSprite = b;
                    this.effect = game.ice;
                    timeAtt += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeAtt > attackTime)
                    {
                        timeAtt -= attackTime;
                        Bullet bullet = new Bullet(game, Center, "CastleSpell", this);
                        Game.Components.Add(bullet);
                    }
                    break;
                }
            }
            //Update army

            foreach (Sprite p in army)
            {
                p.Enemys = enemy.getTotal();
                if (p.Enabled)
                    p.Update(gameTime);
               

            }
            //Update protector
            foreach (Protector p in protector)
            {
                if (p.hp <= 0)
                    p.isBuy = false;
                p.Enemys = enemy.getTotal();
                if (p.Enabled)
                    p.Update(gameTime);
            }
            //Update process Bar
                process.remain = training;
                if (timeTraining != 0)
                    process.Max = timeTraining;
                else
                    process.Max = 1000;

                process.Update(gameTime);
            //Update ghost
                foreach (Sprite sp in ghosts)
                {
                    if (sp.Enabled)
                    {
                        sp.Enemys = enemy.getArmy();
                        sp.Update(gameTime);
                    }
                }
            //Hp bar
                hpBar.remain = hp;
                hpBar.Update(gameTime);
            //Update wall
                foreach (BasicObject w in wall)
                {
                    if (w.Enabled)
                        w.Update(gameTime);
                    if (w.hp <= 0)
                        w.Remove();
                }
            base.Update(gameTime);
        }

        public override void TakeDamage(int Dam)
        {
            base.TakeDamage(Dam);
            gold += Dam / 3;
            audio.BuildingTakeDamage.Play();
        }
        public List<BasicObject> getTotal()
        {
            List<BasicObject> objects = new List<BasicObject>();
                    
                    foreach (BasicObject b in Army)
                        objects.Add(b);
                    foreach (Sprite g in ghosts)
                        objects.Add(g);
                    foreach (BasicObject w in wall)
                        objects.Add(w);
                    foreach (BasicObject b in Protectors)
                        objects.Add(b);
                    objects.Add(this);
                    return objects;
       }
        public List<BasicObject> getArmy()
        {
            List<BasicObject> objects = new List<BasicObject>();

            foreach (BasicObject b in Army)
                objects.Add(b);
            foreach (BasicObject b in Protectors)
                objects.Add(b);
            objects.Add(this);
            return objects;
        }
        public override void Draw(GameTime gameTime)
        {
           
            GUI.Draw(gameTime);
            process.Draw(gameTime);
            hpBar.Draw(gameTime);
            //Draw gem
            foreach (Gem g in gems)
                if(g.hp>0)
                g.Draw(gameTime);
            //Darw ghost
            foreach (Sprite s in ghosts)
            {
                if (s.Visible)
                {
                    s.Draw(gameTime);
                }
            }
            //Draw Army
            foreach (Sprite p in army)
                if (p.Visible)
                    p.Draw(gameTime);
            //Draw protector
            foreach (Protector p in protector)
                if (p.Visible)
                    p.Draw(gameTime);
            //Draw wall
            foreach (BasicObject w in wall)
                if (w.Visible)
                    w.Draw(gameTime);
            //Draw flag
            if (position.X < 600)
                sp.Draw(Game.Content.Load<Texture2D>("Object/1"), new Vector2(0, 300), Color.White);
            else
                sp.Draw(Game.Content.Load<Texture2D>("Object/2"), new Vector2(1100, 300), Color.White);
            //Draw Castle
            sp.Draw(castleTexture, position, new Rectangle(0, 0, castleTexture.Width, castleTexture.Height), Color.White, 0f, Vector2.Zero, 1f, (position.X < 625) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            
            base.Draw(gameTime);
         //   sp.DrawString(Game.Content.Load<SpriteFont>("font"), WorkerNumber.ToString(), new Vector2(400, 400), Color.Red);
        }
    }
}