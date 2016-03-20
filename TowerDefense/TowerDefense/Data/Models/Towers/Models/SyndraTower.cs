using System;
using TowerDefense.Data.Models.Viruses;

namespace TowerDefense.Data.Models.Towers.Models
{
    public class SyndraTower : Tower
    {
        public enum AnimationStates
        {
            Falling,
            Normal,
            LookLeft,
            LookRight
        }

        public const int TowerCost = 120;
        public const string TowerName = "Syndra Tower";
        public const string TowerDescription = "Fires balls of energy";
        public const string ShopIcon = "icon2";
        public const string SurfaceName = "tower2";

        public SyndraTower() {
            this.Type = TowerType.WAVE;
            this.DamageDealt = 50;
            this.AttackSpeed = 1500;
            this.Range = 2;
            this.DamageType = TowerDamageType.SPLASH;
            this.VirusType = Viruses.VirusType.TINRIDER;
            this.Level = 1;
            this.UpgradeCost = 95;
            this.Surface = SurfaceName;

            this.AnimationState = -1;
            this.AnimationStepTick = 350;
        }

        public SyndraTower(int x, int y) : this() {
            this.X = x;
            this.Y = y;
        }

        public override void upgrade() {
            
        }

        public override sbyte GetAnimation() {

            if (LastAnimation + AnimationStepTick < Environment.TickCount) {
                switch (AnimationState) {
                    case -1:
                        AnimationState = 0;
                        break;
                    case 0:
                        AnimationState = 2;
                        NextStep = 1;
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


        private SyndraBall.Ball Ball;
        public override void CustomDraw() {

            if (this.Ball == null || this.Ball.GotVictim) {
                this.Ball = new SyndraBall.Ball(this.X, this.Y);
            }

            Ball.Update();
            Ball.Draw();
        }

        public override void AttackTarget(Virus victim) {
            Ball.FollowTarget(ref victim);
        }
    }
}
