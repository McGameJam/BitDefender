namespace TowerDefense.Data.Models.Viruses
{
    public class PathFinding
    {
        public Directions[] getPath(int mapNumber) {
            switch (mapNumber) {
                case 1:
                    return new Directions[] { Directions.DOWN, Directions.RIGHT, Directions.RIGHT, Directions.DOWN,
            Directions.DOWN, Directions.RIGHT, Directions.RIGHT, Directions.DOWN, Directions.DOWN, Directions.RIGHT,
            Directions.DOWN, Directions.DOWN, Directions.LEFT, Directions.LEFT, Directions.LEFT, Directions.LEFT, Directions.DOWN,
            Directions.DOWN, Directions.RIGHT, Directions.RIGHT, Directions.DOWN, Directions.RIGHT, Directions.RIGHT, Directions.RIGHT,
            Directions.RIGHT, Directions.RIGHT, Directions.UP, Directions.UP, Directions.DOWN, Directions.RIGHT, Directions.DOWN,
            Directions.DOWN, Directions.RIGHT, Directions.DOWN, Directions.DOWN, Directions.LEFT, Directions.LEFT, Directions.DOWN,
            Directions.DOWN, Directions.DOWN, Directions.RIGHT, Directions.RIGHT, Directions.DOWN, Directions.RIGHT, Directions.RIGHT,
            Directions.RIGHT, Directions.DOWN, Directions.DOWN, Directions.DOWN, Directions.LEFT, Directions.DOWN, Directions.DOWN,
            Directions.RIGHT, Directions.DOWN, Directions.DOWN, Directions.DOWN };
            }

            return null;
        }
    }
}
