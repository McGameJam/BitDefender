namespace TowerDefense.Graphics
{
    public interface IGraphics : ISystem
    {
        void DrawObject(object surface);
        object GetFont();
    }
}
