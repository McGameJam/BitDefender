using TowerDefense.Data;

namespace TowerDefense.Graphics.Sfml.Scenes.Objects
{
    public class Image : SceneObject
    {
        internal void cmdStage1_MouseDown(int arg1, int arg2) {
            DataManager.LoadMap(1);
        }
    }
}
