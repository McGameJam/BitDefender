using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Data.Models.Towers
{
    class WaveTower : Tower
    {
        public WaveTower()
        {
            this.towerType = TowerType.WAVE;
            this.cost = 60;
            this.damage = 5;
            this.attackSpeed = 1500;
            this.range = 8;
            this.damageType = TowerDamageType.SPLASH;
            this.virusType = Viruses.VirusType.GROUND;
            this.level = 1;
            this.upgradeCost = 95;
        }

        public override void upgrade()
        {
            this.damage += 4;
            this.level++;
            this.upgradeCost += 100;
        }
    }
}
