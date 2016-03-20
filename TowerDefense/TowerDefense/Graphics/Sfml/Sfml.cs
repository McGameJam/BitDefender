using TowerDefense.Graphics.Sfml.Scenes;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;
using System.IO;
using SFML.System;
using TowerDefense.Data;
using TowerDefense.Data.Models;
using TowerDefense.Data.Models.Viruses;
using TowerDefense.Data.Models.Towers.Models;

namespace TowerDefense.Graphics.Sfml
{
    public class Sfml : IGraphics
    {
        public SceneSystem SceneSystem { private set; get; }
        private RenderWindow DrawingSurface;
        private Font GameFont;
        private Color _backgroundColor;

        // Mouse coords
        public string HoverSurfaceName;
        private int MouseX;
        private int MouseY;

        // The collection of all the graphics.
        private List<GraphicalSurface>[] _surface;

        #region Core System
        public Sfml() {
            // Load all the surfaces and the font.
            this.LoadSurfaces();
            this.LoadFont();

            // Create a new renderwindow that we can render graphics onto.
            this.DrawingSurface = new RenderWindow(new VideoMode(1260, 649), "Bit Defender", Styles.Close);

            // Center it.
            var screen = VideoMode.DesktopMode;
            DrawingSurface.Position = new Vector2i(((int)screen.Width / 2) - 630, ((int)screen.Height / 2) - 355);

            // Set the default background color for the drawing surface.
            this._backgroundColor = new Color(25, 25, 25);

            // Create event handlers for the drawing surface.
            this.CreateEventHandlers();
        }

        public void Destroy() {
            // Destroy the scene system.
            this.SceneSystem.Destroy();
            this.SceneSystem = null;

            // Loop through every collection that stores surfaces.
            foreach (var collection in this._surface) {
                // Loop through every surface in the collection, and 
                // dispose of the sprite.
                foreach (var surface in collection) {
                    surface.Sprite.Dispose();
                }

                // Then clear the collection.
                collection.Clear();
            }

            // Dispose of the game font.
            this.GameFont.Dispose();

            // Dispose of the drawing surface.
            this.DrawingSurface.Dispose();
        }

        private void CreateEventHandlers() {
            // Create a new scene system to handle the events.
            this.SceneSystem = new SceneSystem();

            // For mouse related events, just pass off the coords to the scene system.
            this.DrawingSurface.MouseButtonPressed += (sender, e) => {

                // Is it the left mouse button?
                if (e.Button == Mouse.Button.Left) {

                    // Are we currently trying to place a tower?
                    if (this.HoverSurfaceName != null) {

                        int x = e.X / 60;
                        int y = e.Y / 59;

                        // Figure out if it's out of bounds.
                        if (x >= 0 && x <= 15) {
                            if (y >= 0 && y <= 10) {
                                for (int i = 0; i < DataManager.Map.Towers.Count; i++) {
                                    var tower = DataManager.Map.Towers[i];
                                    if (tower.X == x && tower.Y == y) {
                                        return;
                                    }
                                }

                                if (DataManager.Map.mapArray[x, y].Placable) {
                                    switch (this.HoverSurfaceName) {
                                        case "tower1":
                                            if (DataManager.Board.Money >= TeslaTower.TowerCost) {
                                                DataManager.Map.Towers.Add(new TeslaTower(x, y));
                                                DataManager.Board.RemoveMoney(TeslaTower.TowerCost);
                                            }
                                            break;
                                        case "tower2":
                                            if (DataManager.Board.Money >= SyndraTower.TowerCost) {
                                                DataManager.Map.Towers.Add(new SyndraTower(x, y));
                                                DataManager.Board.RemoveMoney(SyndraTower.TowerCost);
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }

                // Pass off the coords to the UI system
                this.SceneSystem.MouseDown(e.Button.ToString().ToLower(), e.X, e.Y);
            };
            this.DrawingSurface.MouseButtonReleased += (sender, e) => {
                this.SceneSystem.MouseUp(e.Button.ToString().ToLower(), e.X, e.Y);
            };
            this.DrawingSurface.MouseMoved += (sender, e) => {
                this.SceneSystem.MouseMove(e.X, e.Y);

                // Save the position of the mouse.
                this.MouseX = e.X;
                this.MouseY = e.Y;
            };

            // For key related events, pass off the filtered keyname to the scene system.
            this.DrawingSurface.KeyPressed += (sender, e) => {
                this.SceneSystem.KeyDown(this.FilterKey(e));
            };

            // When the close button is pressed, set the game flag to 'closing' so 
            // the application can begin to close.
            this.DrawingSurface.Closed += (sender, e) => {
                switch (Game.State) {
                    case GameState.Game:
                        Game.SetGameState(GameState.MainMenu);
                        break;
                    default:
                        Game.SetGameFlag(GameFlag.Closing);
                        break;
                }
            };
        }

        private void LoadSurfaces() {
            // Initialize the array of collections.
            this._surface = new List<GraphicalSurface>[(int)SurfaceTypes.Length];

            // Initialize each collection in the array.
            for (int i = 0; i < (int)SurfaceTypes.Length; i++) {
                this._surface[i] = new List<GraphicalSurface>();
            }

            // From this point onward, load graphics into their respective collections.
            foreach (string file in Directory.GetFiles(GraphicsManager.TowerPath, "*.png")) {
                this._surface[(int)SurfaceTypes.Tower].Add(new GraphicalSurface(file));
            }

            foreach (string file in Directory.GetFiles(GraphicsManager.MapPath, "*.png")) {
                this._surface[(int)SurfaceTypes.Map].Add(new GraphicalSurface(file));
            }

            foreach (string file in Directory.GetFiles(GraphicsManager.VirusPath, "*.png")) {
                this._surface[(int)SurfaceTypes.Virus].Add(new GraphicalSurface(file));
            }

            foreach (string file in Directory.GetFiles(GraphicsManager.AnimationPath, "*.png")) {
                this._surface[(int)SurfaceTypes.Animation].Add(new GraphicalSurface(file));
            }
        }

        private void LoadFont() {
            string fontFile = GraphicsManager.FontPath + "Kemco Pixel Bold.ttf";

            // Make sure that the ttf file exists.
            if (File.Exists(fontFile)) {
                this.GameFont = new Font(fontFile);
            } else {
                throw new FileNotFoundException("Sfml: " + fontFile);
            }
        }

        public void Draw() {
            // Invoke the DoEvents for SFML so we can capture mouse and keyboard events.
            this.DrawingSurface.DispatchEvents();

            // Clear the drawing surface with the background color.
            this.DrawingSurface.Clear(this._backgroundColor);

            // Draw the actual game.
            this.DrawGame();

            // Draw the user interface.
            this.SceneSystem.Draw();

            // Update the drawing surface with that what has been drawn.
            this.DrawingSurface.Display();
        }

        public void DrawObject(object obj) {
            // Convert the given object to an SFML drawable object, and 
            // have the drawing surface render it.
            var surface = (Drawable)obj;
            this.DrawingSurface.Draw(surface);
        }

        public object GetFont() {
            return this.GameFont;
        }

        private string FilterKey(KeyEventArgs e) {
            string key = e.Code.ToString();

            // Make sure it's a single character.
            if (key.Length == 1) {
                // Are we hitting shift?
                if (e.Shift) {
                    key = key.ToUpper();
                } else {
                    key = key.ToLower();
                }
            } else {
                // If it's not a single character, explore other options.
                switch (key.ToLower()) {
                    case "space":
                        key = " ";
                        break;
                    case "backspace":
                        // Just keep it as it is.
                        break;
                    default:
                        // Don't risk it.
                        return string.Empty;
                }
            }

            // Return the filtered key.
            return key.ToLower();
        }

        public Sprite GetSurface(string tagName, SurfaceTypes type) {
            // Loop through the collection of the specified type.
            for (int i = 0; i < this._surface[(int)type].Count; i++) {
                // If the surface tag name is equal to the tag name specified, return the surface.
                if (this._surface[(int)type][i].Tag == tagName?.ToLower()) {
                    return this._surface[(int)type][i].Sprite;
                }
            }

            // Return null if we couldn't find a surface with the tag specified.
            return null;
        }

        private int GetSurfaceIndex(string tagName, SurfaceTypes type) {
            // Loop through the collection of the specified type.
            for (int i = 0; i < this._surface[(int)type].Count; i++) {
                // If the surface tag name is equal to the tag name specified, return the surface.
                if (this._surface[(int)type][i].Tag == tagName.ToLower()) {
                    return i;
                }
            }

            // Return -1 if we couldn't find a surface with the tag specified.
            return -1;
        }
        #endregion

        private void DrawGame() {
            // All logic pertaining to drawing the game goes here.

            if (Game.State == GameState.Game) {
                // Draw the base map.
                var map = DataManager.Map;
                var mapSurface = GetSurface(map.SurfaceName, SurfaceTypes.Map);
                mapSurface.Position = new Vector2f(0, 0);
                mapSurface.Scale = new Vector2f((float)960 / mapSurface.Texture.Size.X, (float)649 / mapSurface.Texture.Size.Y);
                DrawObject(mapSurface);

                map.UpdateAnimations(false);

                // Draw the contents of the map
                for (int x = 0; x < 16; x++) {
                    for (int y = 0; y < 11; y++) {
                        var mapTile = map.mapArray[x, y];
                        
                        // Render enemies
                        for (int i = 0; i < map.Viruses.Count; i++) {
                            var virus = map.Viruses[i];
                            if (virus?.Position.X == x && virus?.Position.Y == y) {
                                var virusSurface = GetSurface(virus.Surface, SurfaceTypes.Virus);
                                virusSurface.TextureRect = new IntRect(virus.GetXOffset(), 0, 128, 256);
                                virusSurface.Scale = new Vector2f((float)60 / 128, (float)125 / virusSurface.Texture.Size.Y);
                                virusSurface.Position = new Vector2f(virus.Position.X * 60 + virus.xOffset, (virus.Position.Y * 59) - 60 + virus.yOffset);
                                DrawObject(virusSurface);

                                var redbar = GetSurface("redbar", SurfaceTypes.Virus);
                                var greenbar = GetSurface("greenbar", SurfaceTypes.Virus);
                                var scale = new Vector2f(64f / greenbar.Texture.Size.X, 1);
                                greenbar.TextureRect = new IntRect(0, 0, (100 * virus.Health) / virus.MaxHealth, 10);
                                redbar.Position = new Vector2f(virusSurface.Position.X, virusSurface.Position.Y - 50);
                                greenbar.Scale = scale;
                                redbar.Scale = scale;
                                greenbar.Position = redbar.Position;

                                DrawObject(redbar);
                                DrawObject(greenbar);
                            }
                        }

                        // Render towers
                        for (int i = 0; i < map.Towers.Count; i++) {
                            var tower = map.Towers[i];
                            if (tower.X == x && tower.Y == y) {
                                var towerSurface = GetSurface(tower.Surface, SurfaceTypes.Tower);
                                sbyte step = tower.GetAnimation();
                                towerSurface.Scale = new Vector2f(0.5f, 0.5f);
                                towerSurface.TextureRect = new IntRect(step * 128, 0, 128, 167);
                                towerSurface.Position = new Vector2f(x * 60, y * 59 - (towerSurface.Texture.Size.Y / 2 - 59));
                                towerSurface.Color = new Color(255, 255, 255, 255);
                                DrawObject(towerSurface);

                                tower.CustomDraw();
                                break;
                            }
                        }
                    }
                }

                map.UpdateAnimations(true);

                var tile = GetSurface("tile", SurfaceTypes.Map);

                // Draw the placing icon.
                var hoverTowerSurface = GetSurface(this.HoverSurfaceName, SurfaceTypes.Tower);
                //var tile = GetSurface("tile", SurfaceTypes.Map);
                if (hoverTowerSurface != null) {
                    if (this.MouseX < 960) {

                        int x = this.MouseX / 60;
                        int y = this.MouseY / 59;

                        if (y < 0 || y >= 11) {
                            return;
                        }

                        var mapTile = DataManager.Map.mapArray[x, y];

                        if (mapTile.Placable) {
                            tile.Color = SFML.Graphics.Color.Green;
                        } else {
                            tile.Color = SFML.Graphics.Color.Red;
                        }

                        hoverTowerSurface.Color = new Color(255, 255, 255, 200);
                        hoverTowerSurface.Scale = new Vector2f(0.5f, 0.5f);
                        hoverTowerSurface.TextureRect = new IntRect(256, 0, 128, 167);
                        hoverTowerSurface.Position = new Vector2f(this.MouseX - 32, this.MouseY - 41);
                        tile.Position = new Vector2f(x * 60, y * 59);
                        DrawObject(tile);
                        DrawObject(hoverTowerSurface);
                    }
                }
            }
        }
    }
}
