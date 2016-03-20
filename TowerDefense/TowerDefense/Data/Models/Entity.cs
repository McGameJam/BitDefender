namespace TowerDefense.Data.Models
{
    /// <summary>
    /// Possible state that an object may be in.
    /// </summary>
    public enum State
    {
        ACTIVE,
        INACTIVE
    }

    public class Entity: Position
    {
        public int Health { private set; get; }

        // Method for damaging entities.
        public void Damage(int amount) {
            this.Health -= amount;
            if (amount < 0) {
                this.Health = 0;
            }
        }

        // Method for healing entities.
        public void Heal(int amount) {
            this.Health += amount;
        }
    }
}
