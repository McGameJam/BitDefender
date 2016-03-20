using TowerDefense.Data.Models.Maps;
﻿using System.Collections.Generic;
using TowerDefense.Data.Models.Maps;
using TowerDefense.Data.Models.Viruses;

namespace TowerDefense.Data.Models.Towers
{
    public abstract class Tower: Entity
    {
        //variables
        public TowerType Type { get; set; }
        public int DamageDealt { get; set; }
        public int AttackSpeed { get; set; }
        public int Range { get; set; }
        public TowerDamageType DamageType { get; set; } // Single or area splash
        public VirusType VirusType { get; set; } // Whether a ground or flying unit
        public int Level { get; set; } // 1-5 
        public int UpgradeCost { get; set; }
        public Virus CurrentTarget { get; set; }
        public Map map;
        public string Surface { get; set; }
        public int lastAttack;

        protected sbyte AnimationState;
        protected int LastAnimation;
        protected int AnimationStepTick;
        protected sbyte NextStep;

        public abstract void upgrade();
        public abstract sbyte GetAnimation();
        public abstract void CustomDraw();

        public Tower()
        {
            map = DataManager.Map;
        }

        public Tower(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public abstract void AttackTarget(Virus victim);
        
        //public void AttackTarget(Virus victim) {

        //    for (int y = 0; y < this.Range; y++) {
        //        for (int x = 0; x < this.Range; x++) {

        //        }
        //    }
        //    if(CurrentTarget != null)
        //    {
        //        //Check if enemy killed
        //        if (CurrentTarget.Health <= 0)
        //        {
        //            map.OnVirusDeath(CurrentTarget);
        //        }
        //    }

            
        //    return;

        //}
    }
}
