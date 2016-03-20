namespace TowerDefense.Data.Models.Viruses
{
    class Tindrider:Virus
    {
        public Tindrider()
        {
            this.Name = "TinRider";
            this.Speed = 200;
            this.Type = VirusType.TINRIDER;
            this.Health = 135;
            this.MaxHealth = 150;
            this.Level = 1;
            this.Money = 10;
            this.Score = 20;
            this.MovementSpeed = 1;
            this.Surface = "tindrider";
        }

        public Tindrider(int x, int y) : this()
        {
            this.Position.X = x;
            this.Position.Y = y; 
        }

        public override int GetXOffset() {

            int value = 128;

            switch (this.Direction) {
                case Directions.DOWN:
                    switch (this.AnimStep) {
                        case 0:
                        case 2:
                            value *= 1;
                            break;
                        case 3:
                        case 1:
                            value = 0;
                            break;
                    }
                    break;
                case Directions.UP:
                    switch (this.AnimStep) {
                        case 0:
                        case 2:
                            value *= 3;
                            break;
                        case 3:
                        case 1:
                            value *= 2;
                            break;
                    }
                    break;
                case Directions.LEFT:
                    switch (this.AnimStep) {
                        case 0:
                        case 2:
                            value *= 9;
                            break;
                        case 1:
                            value *= 8;
                            break;
                        case 3:
                            value *= 10;
                            break;
                    }
                    break;
                case Directions.RIGHT:
                    switch (this.AnimStep) {
                        case 0:
                        case 2:
                            value *= 4;
                            break;
                        case 1:
                            value *= 5;
                            break;
                        case 3:
                            value *= 7;
                            break;
                    }
                    break;
            }

            return value;
            
        }
    }
}
