namespace TowerDefense.Data.Models.Viruses
{
    public class TestVirus : Virus
    {
        public TestVirus(int x, int y) {
            this.Position = new Position(x, y);
            this.Surface = "1";
        }
    }
}
