using SFML.Graphics;

namespace TowerDefense.Graphics.Sfml.Scenes.Objects
{
    public class Label : SceneObject
    {
        public string Caption;
        public Color TextColor = SFML.Graphics.Color.Black;
        public uint FontSize = 12;

        public override void Draw() {
            // Draw the surface if we have one.
            base.Draw();

            // Draw the caption for the label.
            base.RenderCaption(this.Caption, this.FontSize, this.TextColor);
        }
    }
}
