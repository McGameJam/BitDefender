using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TowerDefense.Data.Models.Towers;
using TowerDefense.Data.Models.Viruses;
using TowerDefense.Graphics;
using TowerDefense.Graphics.Sfml;

namespace TowerDefense.Data.Models.Maps
{
    public class Map
    {
        public delegate void virusDeathDel(Virus virus, bool isLifeLost);
        public event virusDeathDel virusDeath;
        public Tile[,] mapArray = new Tile[16, 11];
        // Map constants
        public const int TILE_WIDTH = 64;
        public const int TILE_HEIGHT = 64;

        public const int WIDTH = 16;
        public const int HEIGHT = 11;

        public Home Home;
        public List<Tower> Towers = new List<Tower>();
        public List<Virus> Viruses = new List<Virus>();
        public List<Virus> EnemiesInRange;
        public Position SpawnLocation { get; set; }
        public string SurfaceName;
        public PathFinding path = new PathFinding();
        public Directions[] dirs;
        public int MobCount;
        public int SpawnRate;

        //Tower Position
        Position pos = new Position();

        public List<Anim.Animation> MapAnimations = new List<Anim.Animation>();


        public Map() {
            Home = new Home();
            // add handler for virus deaths
            virusDeath += (virus, isLifeLost) => {
                if (isLifeLost == true) {
                    if (Home.takeDamage()) {

                        int highestScore;

                        //Check if the file exists.
                        if (!File.Exists("highScore.txt"))
                        {
                            //Create the file if it doesn't exist and save 0 as the high score.
                            using (StreamWriter sr = new StreamWriter("highScore.txt"))
                            {
                                sr.WriteLine("0");
                            }
                        }

                        //Read the score in the file, even if it was just created.
                        using (TextReader reader = File.OpenText("highScore.txt"))
                        {
                            highestScore = int.Parse(reader.ReadLine());
                        }

                        //Check if the user's final score is greater than the highscore saved in the file.
                        if (DataManager.Board.Score > highestScore)
                        {
                            //Update the score in the file if the user's final score is higher.
                            using (StreamWriter sr = new StreamWriter("highScore.txt"))
                            {
                                sr.WriteLine(DataManager.Board.Score + "");
                            }
                        }

                        Game.SetGameState(GameState.GameOver);
                    }
                }
                else
                {
                    DataManager.Board.AddMoney(virus.Money);
                    DataManager.Board.AddScore(virus.Score);
                }
                Viruses.Remove(virus);
                if (Viruses.Count <= 0) {
                    // TODO should determine virus type by wave number
                    int score = DataManager.Board.Score;
                    Tindrider tind = new Tindrider();

                    if (score >= 500)
                    {
                        //tind.Health = 200;
                        //tind.Money = 5;
                        tind.Speed -= 25;
                    }
                   /* if(score >= 1000)
                    {
                        tind.Health = 300;
                        
                    }
                    if(score >= 1500)
                    {
                        tind.Health = 450;
                        tind.Speed -= 50;
                    }
                    if (score >= 2000)
                    {
                        tind.Health = 600;
                    }
                    if (score >= 2000)
                    {
                        tind.Health = 750;
                    }*/
                    SpawnManager.spawnWave(1000, tind);
                }
            };
            // Spawn one wave on creation of the map
            SpawnManager.spawnWave(10, new Tindrider());
        }

        public void UpdateLogic() {
            for (int i = 0; i < Viruses.Count; i++) {
                var virus = Viruses[i];
                if (virus != null) {
                    // They got to the end
                    if (virus.Step >= dirs.Length) {
                        OnVirusDeath(virus, true);
                        //break;
                    } else {
                        virus.Move((Directions)dirs.GetValue(virus.Step));
                    }

                    for (int j = 0; j < DataManager.Map.Towers.Count; j++) {
                        var tower = DataManager.Map.Towers[j];

                        // Check for attack speed or something. Connor seems to know.
                        if (tower.lastAttack + tower.AttackSpeed < Environment.TickCount)
                        {
                            if (Math.Abs(virus.Position.X - tower.X) <= tower.Range)
                            {
                                if (Math.Abs(virus.Position.Y - tower.Y) <= tower.Range)
                                {

                                    // Attack the virus
                                    tower.AttackTarget(virus);

                                    // Update whatever we check above.
                                    tower.lastAttack = Environment.TickCount;
                                }
                            }
                        }
                        /*else
                        {
                            tower.lastAttack = ;
                        }*/
                        
                    }
                }
            }
        }

        public void UpdateAnimations(bool overlay = false) {
            for (int i = 0; i < MapAnimations.Count; i++) {
                var anim = MapAnimations[i];

                if (anim.Overlay == overlay) {
                    anim.Update();

                    if (anim.Disposable) {
                        MapAnimations.RemoveAt(i);
                        i--;
                    }

                    var surface = ((Sfml)GraphicsManager.Graphics).GetSurface(anim.Surface, SurfaceTypes.Animation);
                    if (surface != null) {
                        surface.Rotation = anim.Rotation;
                        surface.Position = new SFML.System.Vector2f(anim.Position.X, anim.Position.Y);
                        surface.TextureRect = new SFML.Graphics.IntRect(anim.State * anim.FrameWidth, 0, anim.FrameWidth, anim.FrameHeight);
                        GraphicsManager.Graphics.DrawObject(surface);
                    }
                }   
            }
        }

        public virtual void PopulateMap() {

        }

        public Virus prioritizeVirus(List<Virus> unsortedList) {
            List<Virus> sortedList = unsortedList.OrderBy(v => v.Step).ToList();

            return sortedList.LastOrDefault();
        }

        // Remove the virus from the list and spawn a new wave if none are left.
        public void OnVirusDeath(Virus virus, bool lifeLost = false) {
            if (virusDeath != null)
                virusDeath(virus, lifeLost);
        }
    }
}
