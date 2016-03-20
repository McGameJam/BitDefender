using System;
namespace TowerDefense.Data.Models.Viruses
{
    public abstract class Virus
    {
        public Position Position { get;  set; }
        public VirusType Type { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Speed { get; set; }
        public int Money { get; set; }
        public int Health { get; set; }
        public int MaxHealth;
        public int Level { get; set; }
        public int MovementSpeed { get; set; }
        public int Step { get; private set; }
        public string Surface { get; set; }
        public Directions Direction = Directions.DOWN;
        public byte AnimStep;

        private int LastAnimStep;

        public int xOffset { get; private set; }
        public int yOffset { get; private set; }

        public Virus() {
            this.Position = new Position();
            this.Speed = 500;
        }

        public Virus(int x, int y) {
            this.Position = new Position(x, y);
        }


        private int LastMove;

        // Make the virus take damage and check if it died.
        public void takeDamage(int dmg)
        {
            Health = Health - dmg;

            if(Health <= 0)
            {
                DataManager.Map.OnVirusDeath(this);
            }
        }

        public void Move(Directions dir) {

            this.Direction = dir;

            int AnimStepTick = this.Speed / 4;

            if (AnimStepTick < 100) {
                AnimStepTick = 100;
            }

            if (LastAnimStep + AnimStepTick < Environment.TickCount) {
                this.AnimStep += 1;
                this.AnimStep %= 4;

                LastAnimStep = Environment.TickCount + AnimStepTick;
            }
            

            // Check to see if we can move again.
            if (LastMove + Speed < Environment.TickCount) {

                this.xOffset = 0;
                this.yOffset = 0;

                switch (dir)
                {
                    case Directions.UP:
                        this.Position = new Position(this.Position.X, this.Position.Y - 1);
                        break;
                    case Directions.DOWN:
                        this.Position = new Position(this.Position.X, this.Position.Y + 1);
                        break;
                    case Directions.RIGHT:
                        this.Position = new Position(this.Position.X + 1, this.Position.Y);
                        break;
                    case Directions.LEFT:
                        this.Position = new Position(this.Position.X - 1, this.Position.Y);
                        break;
                }

                this.Step++;

                // Set the current tickcount as our last move time.
                LastMove = Environment.TickCount;

                // Check if the virus is standing on a teleporter. Hardcoded for now
                if(Step == 28)
                {
                    DataManager.Map.MapAnimations.Add(new Anim.Animation() {
                        FrameHeight = 59,
                        FrameWidth = 60,
                        MaxState = 8,
                        Position = new Position(Position.X * 60, Position.Y * 59),
                        Surface = "teleOut",
                        UpdateTick = 32,
                        Overlay = false
                    });
                    this.Position = new Position(1, 1);
                    DataManager.Map.MapAnimations.Add(new Anim.Animation() {
                        FrameHeight = 59,
                        FrameWidth = 60,
                        MaxState = 8,
                        Position = new Position(Position.X * 60, Position.Y * 59),
                        Surface = "teleIn",
                        UpdateTick = 32,
                        Overlay = false
                    });
                }
                if(Step == 40)
                {
                    DataManager.Map.MapAnimations.Add(new Anim.Animation() {
                        FrameHeight = 59,
                        FrameWidth = 60,
                        MaxState = 8,
                        Position = new Position((Position.X + 1) * 60, (Position.Y + 1) * 59),
                        Surface = "teleOut",
                        UpdateTick = 32,
                        Rotation = 180f,
                        Overlay = false
                    });
                    this.Position = new Position(9, 1);
                    DataManager.Map.MapAnimations.Add(new Anim.Animation() {
                        FrameHeight = 59,
                        FrameWidth = 60,
                        MaxState = 8,
                        Position = new Position((Position.X) * 60, (Position.Y + 1) * 59),
                        Surface = "teleIn",
                        UpdateTick = 32,
                        Rotation = 270f,
                        Overlay = false
                    });
                }
                
            } else {
                int timeSince = Environment.TickCount - LastMove;
                int x = (int)(((float)timeSince / Speed) * 60);
                int y = (int)(((float)timeSince / Speed) * 59);

                switch (dir) {
                    case Directions.UP:
                        this.yOffset = -y;
                        this.xOffset = 0;
                        break;
                    case Directions.DOWN:
                        this.yOffset = y;
                        this.xOffset = 0;
                        break;
                    case Directions.LEFT:
                        this.xOffset = -x;
                        this.yOffset = 0;
                        break;
                    case Directions.RIGHT:
                        this.xOffset = x;
                        this.yOffset = 0;
                        break;
                }
            }
        }

        public virtual int GetXOffset() {
            return 0;
        }
    }
}
