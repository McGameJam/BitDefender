using System.Collections.Generic;
using System.IO;
using TowerDefense.Graphics.Sfml.Scenes.Objects;
using TowerDefense.Data.Models.Towers.Models;

namespace TowerDefense.Graphics.Sfml.Scenes
{
    public class SceneSystem : IScenes
    {
        // The collection of all the scene related surfaces, and objects.
        private List<GraphicalSurface> _surfaces;
        private List<SceneObject>[] _UIObject;

        // The scene object that has the current focus.
        private SceneObject _curFocus;

        // General bool variable stating whether or not the mouse buttons are down.
        public bool MouseLeftDown { private set; get; }
        public bool MouseMiddleDown { private set; get; }
        public bool MouseRightDown { private set; get; }

        #region Core Scene System Logic
        // All these methods pertain to event handling.
        // You shouldn't have to change this.

        public SceneSystem() {
            // Create an array of collections containing scene objects for 
            // every game state.
            this._UIObject = new List<SceneObject>[(int)GameState.Length];
            for (int i = 0; i < this._UIObject.Length; i++) {
                this._UIObject[i] = new List<SceneObject>();
            }

            // Load all the graphical surfaces.
            this.LoadSurfaces();

            // Load all the hard-coded scene objects.
            this.LoadSceneObjects();
        }

        public void Reload() {

        }

        public void Destroy() {

        }

        private void LoadSurfaces() {
            // Initialize the collection.
            this._surfaces = new List<GraphicalSurface>();

            // Load every png file we find in the directory specified.
            foreach (string file in Directory.GetFiles(GraphicsManager.GuiPath, "*.png")) {
                this._surfaces.Add(new GraphicalSurface(file));
            }
        }

        public void MouseMove(int x, int y) {
            // If our left mouse button is down, we can apply dragging
            // processing on our focused scene object.
            if (this.MouseLeftDown && this._curFocus != null) {
                this._curFocus.Drag(x, y);
            }

            // Make sure that we actually initialized the scene system.
            if (this._UIObject != null) {
                // Make sure that we actually have scene objects in our current state.
                if (this._UIObject[(int)Game.State] != null) {
                    // Loop through all possible values for the ZOrder.
                    for (int z = ZOrder.GetHighZ(); z >= 0; z--) {
                        // Loop through every scene object we have in our current state.
                        foreach (var obj in this._UIObject[(int)Game.State]) {

                            obj.HasMouse = false;

                            // Does the object's ZIndex match the ZOrder?
                            if (obj.Z == z) {
                                // Is the object visible?
                                if (obj.Visible) {
                                    // Did we move our mouse within the area of the scene object?
                                    if (x >= obj.Left && x <= obj.Left + obj.Width) {
                                        if (y >= obj.Top && y <= obj.Top + obj.Height) {

                                            // We did. Invoke appropriate event-handling methods.
                                            obj.MouseMove(x - obj.Left, y - obj.Top);

                                            // We assume we have the object we moused over.
                                            // Return so that we don't apply similar logic on scene objects that 
                                            // should not receive this processing.
                                            return;
                                        }
                                    }
                                }
                                // This break will break out of the current loop through all the scene objects for a respective Z value.
                                // It ensures we don't waste time looking for another object that can't exist.
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void MouseUp(string button, int x, int y) {
            // Set the states of the appropriate mouse button to not pressed.
            switch (button) {
                case "left":
                    this.MouseLeftDown = false;
                    break;
                case "middle":
                    this.MouseMiddleDown = false;
                    break;
                case "right":
                    this.MouseRightDown = false;
                    break;
            }

            // Since we're lifting a mouse button, do we have a 
            // currently focused scene object?
            if (this._curFocus != null) {
                // Invoke the EndDrag method for that object.
                this._curFocus.EndDrag();
            }
        }

        public void MouseDown(string button, int x, int y) {
            // Set the states of the appropriate mouse button to pressed.
            switch (button) {
                case "left":
                    this.MouseLeftDown = true;
                    break;
                case "middle":
                    this.MouseMiddleDown = true;
                    break;
                case "right":
                    this.MouseRightDown = true;
                    break;
            }

            ((Sfml)GraphicsManager.Graphics).HoverSurfaceName = null;

            // Make sure that the scene system has actually been initialized.
            if (this._UIObject != null) {
                // Make sure that we actually have scene objects in our current state.
                if (this._UIObject[(int)Game.State] != null) {
                    // Loop through every possible ZOrder value.
                    for (int z = ZOrder.GetHighZ(); z >= 0; z--) {
                        // Loop through all the scene objects in our current state.
                        foreach (var obj in this._UIObject[(int)Game.State]) {
                            // Does the ZIndex match the ZOrder?
                            if (obj.Z == z) {
                                // Make sure that the object is visible.
                                if (obj.Visible) {
                                    // Did we click within the area of the scene object?
                                    if (x >= obj.Left && x <= obj.Left + obj.Width) {
                                        if (y >= obj.Top && y <= obj.Top + obj.Height) {

                                            //If we had a previous scene object, let that object
                                            // know that it no longer has the focus.
                                            if (this._curFocus != null) {
                                                this._curFocus.HasFocus = false;
                                            }

                                            // Assign this scene object as our currently focused scene object and
                                            // let it know that it has our focus.
                                            this._curFocus = obj;
                                            this._curFocus.HasFocus = true;

                                            // Invoke the appropriate event handling methods.
                                            this._curFocus.MouseDown(x - obj.Left, y - obj.Top);
                                            this._curFocus.BeginDrag(x - obj.Left, y - obj.Top);

                                            // We assume that we have the object we clicked on.
                                            // Return so that we don't apply similar logic on scene objects that
                                            // should not receive this processing.
                                            return;
                                        }
                                    }
                                }
                                // This break will break out of the current loop through all the scene objects for a respective Z value.
                                // It ensures we don't waste time looking for another object that can't exist.
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void KeyDown(string key) {
            // Keypress event handling regarding the scene system requires an
            // object being focused.
            if (this._curFocus != null) {
                this._curFocus.KeyDown(key);
            }
        }

        public void KeyUp(string key) {
            // Keyup event handling regarding the scene system requires an
            // object being focused.
            if (this._curFocus != null) {
                this._curFocus.KeyUp(key);
            }
        }

        public void Draw() {
            // Make sure that we've actually loaded the scene system.
            if (this._UIObject != null) {
                // Make sure we actually have scene objects in our current state.
                if (this._UIObject[(int)Game.State] != null) {
                    // Draw every object in this scene if it's visible.
                    foreach (var obj in this._UIObject[(int)Game.State]) {
                        if (obj.Visible) {
                            obj.Draw();
                        }
                    }
                }
            }

            // Updating logic.
            UpdateLogic();
        }

        private GraphicalSurface GetSurface(string tagName) {
            // Loop through our collection of graphical surfaces.
            foreach (var surface in this._surfaces) {
                // If the surface's tag matches our specific tag, return the surface.
                if (surface.Tag == tagName.ToLower()) {
                    return surface;
                }
            }
            // If the surface does not exist, return null.
            return null;
        }

        public SceneObject GetUIObject(string name) {
            // Make sure that we actually initialized the scene system.
            if (this._UIObject != null) {
                // Make sure we actually have scene objects in our current state.
                if (this._UIObject[(int)Game.State] != null) {
                    // Loop through all the scene objects in our current state.
                    foreach (var obj in this._UIObject[(int)Game.State]) {
                        // If the object has the same name as the one specified, return it.
                        if (obj.Name.ToLower() == name.ToLower()) {
                            return obj;
                        }
                    }
                }
            }
            // If the scene object could not be found, return null.
            return null;
        }

        #endregion

        private void LoadSceneObjects() {
            LoadMainMenuUI();
            LoadStageSelectUI();
            LoadGameUI();
            LoadGameOverUI();
        }

        private void LoadMainMenuUI() {
            _UIObject[(int)GameState.MainMenu] = new List<SceneObject>();
            var scene = _UIObject[(int)GameState.MainMenu];


            // The main menu background.
            var background = new Image() {
                Name = "imgBackground",
                Width = 1260,
                Height = 649,
                Surface = GetSurface("background")
            };
            scene.Add(background);

            // The button for picking a stage.
            var stageSelect = new Button() {
                Name = "cmdStageSelect",
                Left = 425,
                Top = 315,
                Width = 240,
                Height = 100,
                Surface = GetSurface("buttonStart")
            };
            stageSelect.OnMouseDown += stageSelect.cmdStageSelect_MouseDown;
            scene.Add(stageSelect);
        }

        private void LoadStageSelectUI() {
            _UIObject[(int)GameState.StageSelect] = new List<SceneObject>();
            var scene = _UIObject[(int)GameState.StageSelect];

            // The main menu background.
            var background = new Image() {
                Name = "imgBackground",
                Width = 1260,
                Height = 649,
                Surface = GetSurface("stageselect")
            };
            scene.Add(background);

            // The button for going back to the main menu.
            var backButton = new Button() {
                Name = "cmdBackButton",
                Caption = "Return to Main Menu",
                Left = (1260 / 2) - 100,
                Top = 500,
                Width = 200,
                Height = 50,
                Surface = GetSurface("button"),
                TextColor = SFML.Graphics.Color.White
            };
            backButton.OnMouseDown += backButton.cmdBackButton_MouseDown;
            scene.Add(backButton);

            // Stage one
            var stage1 = new Image() {
                Name = "stg1",
                Width = 200,
                Height = 200,
                Left = 150,
                Top = 200
            };
            stage1.OnMouseDown += stage1.cmdStage1_MouseDown;
            scene.Add(stage1);
        }

        private void LoadGameUI() {
            _UIObject[(int)GameState.Game] = new List<SceneObject>();
            var scene = _UIObject[(int)GameState.Game];

            // The Store
            var imgStoreBackground = new Image() {
                Name = "imgStoreBackground",
                Surface = GetSurface("sidemenu"),
                Left = 960,
                Height = 649,
                Width = 300
            };
            scene.Add(imgStoreBackground);

            var health = new Label() {
                Name = "lblHealth",
                FontSize = 24,
                Width = 200,
                Left = 970,
                Top = 20,
                TextColor = SFML.Graphics.Color.White
            };
            scene.Add(health);

            var score = new Label() {
                Name = "lblScore",
                FontSize = 24,
                Width = 200,
                Left = 970,
                Top = 50,
                TextColor = SFML.Graphics.Color.White
            };
            scene.Add(score);

            var money = new Label() {
                Name = "lblMoney",
                FontSize = 24,
                Width = 200,
                Left = 970,
                Top = 80,
                TextColor = SFML.Graphics.Color.White
            };
            scene.Add(money);

            // Store items
            var tower1 = new ShopItem() {
                Name = "twr1",
                ItemName = TeslaTower.TowerName,
                Description = TeslaTower.TowerDescription,
                Surface = GetSurface(TeslaTower.ShopIcon),
                HoverSurfaceName = TeslaTower.SurfaceName,
                Left = 1000,
                Top = 150,
                Width = 100,
                Height = 100,
                ItemCost = TeslaTower.TowerCost
            };
            scene.Add(tower1);

            var tower2 = new ShopItem() {
                Name = "twr2",
                ItemName = SyndraTower.TowerName,
                Description = SyndraTower.TowerDescription,
                Surface = GetSurface(SyndraTower.ShopIcon),
                HoverSurfaceName = SyndraTower.SurfaceName,
                Left = 1125,
                Top = 150,
                Width = 100,
                Height = 100,
                ItemCost = SyndraTower.TowerCost
            };
            scene.Add(tower2);
        }

        private void LoadGameOverUI() {
            _UIObject[(int)GameState.GameOver] = new List<SceneObject>();
            var scene = _UIObject[(int)GameState.GameOver];

            // The main menu background.
            var background = new Image() {
                Name = "imgBackground",
                Width = 1260,
                Height = 649,
                Surface = GetSurface("gameover")
            };
            scene.Add(background);

            var yourScore = new Label() {
                Name = "lblYourScore",
                Width = 630,
                Height = 50,
                Caption = "Your score was ",
                FontSize = 24,
                TextColor = SFML.Graphics.Color.White,
                Top = 350,
                Left = 0
            };
            scene.Add(yourScore);

            var highScore = new Label()
            {
                Name = "lblHighScore",
                Width = 630,
                Height = 50,
                Caption = "Highscore ",
                FontSize = 24,
                TextColor = SFML.Graphics.Color.White,
                Top = 400,
                Left = 0
            };
            scene.Add(highScore);

            var cmdBack = new Button() {
                Name = "cmdBack",
                Width = 200,
                Height = 50,
                Caption = "Main Menu",
                TextColor = SFML.Graphics.Color.White,
                Top = 500,
                Left = 630 / 2 - 100,
                Surface = GetSurface("button")
            };
            cmdBack.OnMouseDown += cmdBack.cmdBackButton_MouseDown;
            scene.Add(cmdBack);
        }



        private void UpdateLogic() {
            UpdateScore();
            UpdateMoney();
            UpdateHealth();
        }

        private void UpdateScore() {
            // Update the UI label if it's not null.
            var label = GetUIObject("lblScore");
            if (label != null) {
                int score = Data.DataManager.Board.Score;
                ((Label)label).Caption = "Score: " + score;
            }

            label = GetUIObject("lblYourScore");
            if (label != null) {
                int score = Data.DataManager.Board.Score;
                ((Label)label).Caption = "Your score was " + score;
            }

            label = GetUIObject("lblHighScore");
            if (label != null)
            {
                int score;
                //Read the score in the file, even if it was just created.
                using (TextReader reader = File.OpenText("highScore.txt"))
                {
                   score = int.Parse(reader.ReadLine());
                }
                ((Label)label).Caption = "Highscore " + score;
            }
        }

        private void UpdateMoney() {
            // Update the UI label if it's not null.
            var label = GetUIObject("lblMoney");
            if (label != null) {
                int money = Data.DataManager.Board.Money;
                ((Label)label).Caption = "Money: " + money;
            }
        }

        private void UpdateHealth() {
            // Update the UI lable if it's not null.
            var label = GetUIObject("lblHealth");
            if (label != null) {
                int health = Data.DataManager.Map.Home.Health;
                ((Label)label).Caption = "Health: " + health;
            }
        }
    }
}
