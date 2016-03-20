using TowerDefense.Data.Models.Maps;
using TowerDefense.Data.Models;

namespace TowerDefense.Data
{
    public static class DataManager
    {
        public static Map Map { private set; get; }
        public static Board Board { private set; get; }

        public static void LoadMap(int number) {

            // Make sure we can pick a map.
            if (Game.State != GameState.StageSelect) {
                return;
            }

            // Figure out which game we want to load.
            switch (number) {
                case 1:
                    DataManager.Map = new Map1();
                    DataManager.Board = new Board();
                    break;
            }

            // Set the state of the client to 'game'.
            Game.SetGameState(GameState.Game);
        }

        public static void Load() {
            // Inlcude all data-loading logic here
        }

        public static void Save() {
            // Include all data-saving logic here.
        }
    }
}
