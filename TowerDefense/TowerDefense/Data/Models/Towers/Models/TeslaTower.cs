using System;
using TowerDefense.Data.Models.Viruses;

namespace TowerDefense.Data.Models.Towers.Models
{
    public class TeslaTower : Tower
    {
        public const int TowerCost = 100;
        public const string TowerName = "Ez Tower";
        public const string TowerDescription = "Fires a single, powerful shot";
        public const string ShopIcon = "icon1";
        public const string SurfaceName = "tower1";

        public TeslaTower()
        {
            this.Type = TowerType.TESLA;
            this.DamageDealt = 100;
            this.AttackSpeed = 1500;
            this.Range = 1;
            this.DamageType = TowerDamageType.SINGLE;
            this.VirusType = Viruses.VirusType.TINRIDER;
            this.Level = 1;
            this.UpgradeCost = 75;
            this.Surface = SurfaceName;

            this.AnimationStepTick = 350;
            this.AnimationState = -1;
        }

        public TeslaTower(int x, int y) : this() {
            this.X = x;
            this.Y = y;
        }

        public override void upgrade()
        {
            this.DamageDealt += 5;
            this.Level++;
            this.UpgradeCost += 75;
        }

        public override void CustomDraw() {

        }

        public override sbyte GetAnimation() {

            if (LastAnimation + AnimationStepTick < Environment.TickCount) {
                switch (AnimationState) {
                    case -1:
                        AnimationState = 0;
                        break;
                    case 0:
                        AnimationState = 1;
                        break;
                    case 1:
                        NextStep = 1;
                        AnimationState += NextStep;
                        break;
                    case 2:
                        AnimationState += NextStep;
                        break;
                    case 3:
                        NextStep = -1;
                        AnimationState += NextStep;
                        break;
                }

                LastAnimation = Environment.TickCount;
            }



            return this.AnimationState;
        }

        private void AddAnimation(float rotation, Position pos) {
            DataManager.Map.MapAnimations.Add(new Anim.Animation() {
                Surface = "lazer",
                FrameHeight = 192,
                FrameWidth = 64,
                MaxState = 8,
                Position = pos,
                UpdateTick = 25,
                Overlay = true,
                Rotation = rotation
            });
        }

        public override void AttackTarget(Virus victim) {
            victim.takeDamage(DamageDealt);

            var pos = new Position(this.X * 60, this.Y * 59);


            if (victim.Position.X < this.X) {
                if (victim.Position.Y < this.Y) {
                    // Top Left
                    AddAnimation(135f, new Position(pos.X + 96, pos.Y + 16));
                } else if (victim.Position.Y > this.Y) {
                    // Bottom Left
                    AddAnimation(45f, new Position(pos.X + 56, pos.Y - 72));
                } else {
                    // Absolute Left
                    AddAnimation(90f, new Position(pos.X + 96, pos.Y - 32));
                }
            } else if (victim.Position.X > this.X) {
                if (victim.Position.Y < this.Y) {
                    // Top Right
                    AddAnimation(225f, new Position(pos.X + 8, pos.Y + 64));
                } else if (victim.Position.Y > this.Y) {
                    // Bottom Right
                    AddAnimation(315f, new Position(pos.X - 32 , pos.Y - 24));
                } else {
                    // Absolute Right
                    AddAnimation(270f, new Position(pos.X - 32, pos.Y + 32));
                }
            } else {
                if (victim.Position.Y < this.Y) {
                    // Absolute Top
                    AddAnimation(180f, new Position(pos.X + 64, pos.Y + 64));
                } else if (victim.Position.Y > this.Y) {
                    // Absolute Bottom
                    AddAnimation(0f, new Position(pos.X, pos.Y - 64));
                }
            }
        }
    }
}
