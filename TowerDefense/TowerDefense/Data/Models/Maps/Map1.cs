namespace TowerDefense.Data.Models.Maps
{
    public class Map1 : Map
    {
        public Map1() : base() {
            this.SurfaceName = "stage1";
            this.SpawnLocation = new Position(4, 0);
            this.dirs = path.getPath(1);
            PopulateMap();
        }

        public override void PopulateMap() {
            int[,] simpleMapArray = new int[,] {
                { 0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,2,0,0,1,1,1,0,0,2,1,1,0,0,0,0 },
                { 0,1,1,0,0,0,1,0,0,0,0,1,1,1,1,0 },
                { 0,0,1,0,0,0,1,1,1,0,0,0,0,0,1,0 },
                { 0,0,1,1,0,0,0,0,1,0,0,0,0,0,1,0 },
                { 0,0,0,1,0,0,0,0,1,1,0,0,0,1,1,0 },
                { 0,1,1,1,0,0,0,0,0,1,0,0,0,1,0,0 },
                { 0,1,0,0,0,1,1,1,1,1,0,0,0,1,1,0 },
                { 0,1,0,0,0,1,0,0,0,0,0,0,2,0,1,0 },
                { 0,2,0,0,0,1,1,1,0,0,0,0,1,0,1,0 },
                { 0,0,0,0,0,0,0,1,1,1,1,1,1,0,1,0 }
            };

            for (int x = 0; x < 16; x++) {
                for (int y = 0; y < 11; y++) {
                    int tile = simpleMapArray[y, x];
                    mapArray[x, y] = new Tile();
                    mapArray[x, y].Placable = false;

                    // Can we place something on the tile?
                    if (tile == 0) {
                        mapArray[x, y].Placable = true;
                    }
                }
            }
        }
    }
}
