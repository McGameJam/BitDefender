namespace TowerDefense.Data.Models
{
    public class Board
    {
        public int Score { private set; get; }
        public int Money { private set; get; }
        public int Wave { get; set; }

        private const int STARTING_MONEY = 250;

        public Board() {
            // When the game starts, you'll have a certain
            // amount of money, and a score of 0.
            this.Score = 0;
            this.Money = STARTING_MONEY;
        }

        public void AddMoney(int amount) {
            this.Money += amount;
        }

        public void RemoveMoney(int amount) {
            this.Money -= amount;
            if (this.Money < 0) {
                this.Money = 0;
            }
        }

        public void AddScore(int amount) {
            this.Score += amount;
        }
    }
}
