namespace TowerDefense.Data.Models
{
    public class Home
    {
        //public int Health {
        //    set { this.Health = value; }
        //    get { return this.Health; }
        //}
        public int Health; 
        public Home()
        {
            // Default health value
            Health = 10;
        }

        // Invoked by the Map class to inform the Base that it has taken damage.
        // Return boolean value indicating whether the game is over or not.
        public bool takeDamage()
        {
            Health--;
            if (Health > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
