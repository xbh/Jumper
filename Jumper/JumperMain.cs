using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using xnaMBox = Microsoft.Xna.Framework.Input.MessageBox;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Jumper {
    public class JumperMain : GameScene {
        private GameSocket socket;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private JumperMan man;
        private JumperTile[] _tiles;
        private Vector2 stage;
        private string levelName;
        private List<string> levels;
        public int score;
        private Texture2D texExplo;
        private Explosion explosion;

        public JumperMain(GameSocket socket, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, string levelName = "level1", int score = 0) : base(socket) {
            //_graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";
            //IsMouseVisible = true;
            _spriteBatch = spriteBatch;
            this.socket = socket;
            _graphics = graphics;
            this.levelName = levelName;
            this.score = score;

            //_graphics.PreferredBackBufferHeight = 800;
            //_graphics.PreferredBackBufferWidth = 600;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            stage = new Vector2(_graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);

            levels = new List<string>() { "level1", "level2", "level3" };
            ReadLevel(levelName);

            Texture2D manTex = socket.Content.Load<Texture2D>("Image/jumper");


            Vector2 manInitPos = new Vector2(_tiles[0].Position.X, _tiles[0].Position.Y - manTex.Height);
            man = new JumperMan(socket, _spriteBatch, manTex, manInitPos, stage);
            this.Components.Add(man);



            foreach (var i in _tiles) {
                CollisionManager cm = new CollisionManager(socket, this, man, i, stage);
                this.Components.Add(cm);
            }


            texExplo = Game.Content.Load<Texture2D>("Image/explosion");
            explosion = new Explosion(socket, _spriteBatch, texExplo, man.Position, 3, man);
            this.Components.Add(explosion);

        }

        //protected override void Initialize() {
        //    // TODO: Add your initialization logic here


        //    base.Initialize();
        //}

        //protected override void LoadContent() {
        //    _spriteBatch = new SpriteBatch(GraphicsDevice);

        //    stage = new Vector2(_graphics.PreferredBackBufferWidth,
        //        _graphics.PreferredBackBufferHeight);

        //    ReadLevel(levelName);

        //    Texture2D manTex = game.Content.Load<Texture2D>("Image/jumper");


        //    Vector2 manInitPos = new Vector2(_tiles[0].Position.X + manTex.Width, _tiles[0].Position.Y - manTex.Height);
        //    man = new JumperMan(game, _spriteBatch, manTex, manInitPos, stage);
        //    this.Components.Add(man);



        //    foreach (var i in _tiles) {
        //        CollisionManager cm = new CollisionManager(game,this, man, i, stage);
        //        this.Components.Add(cm);
        //    }


        //    // TODO: use this.Content to load your game content here
        //}

        public override void Update(GameTime gameTime) {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    game.Exit();

            if (man.getBoundRect().Bottom >= stage.Y) {
                //todo: loose code
                xnaMBox.Show("Jumper", "Game over" + "\nYour score: " + score, new[] { "OK" });
                GoToMenu();
            }

            Rectangle manRectangle = man.getBoundRect();
            explosion.Position = new Vector2(manRectangle.X + manRectangle.Width / 2, manRectangle.Bottom - 10);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            Texture2D backgroundTexture = socket.Content.Load<Texture2D>("Image/background");
            Rectangle backgroundRectangle = new Rectangle(0, 0, 900, 1203);

            SpriteFont font = socket.Content.Load<SpriteFont>("Fonts/hilightfont");


            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, backgroundRectangle, Color.White);
            _spriteBatch.DrawString(font, levelName, Vector2.Zero, Color.BlueViolet);
            _spriteBatch.DrawString(font, "Score: " + score, new Vector2(stage.X - 200, 0), Color.BlueViolet);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        /// <summary>
        /// Read file and pop the map if map generated successfully
        /// </summary>
        private void ReadLevel(string levelName) {
            this.levelName = levelName;
            List<int> stringList = new List<int>();
            using (StreamReader reader = new StreamReader("Content/Levels/" + levelName + ".level")) {
                while (!reader.EndOfStream) {
                    stringList.Add(int.Parse(reader.ReadLine()));
                }
            }

            if (GenerateMapByStrings(stringList)) {
                foreach (var i in _tiles) {
                    this.Components.Add(i);
                }
            } else {
                throw new Exception("ERROR when reading level file");
            }
        }

        private bool GenerateMapByStrings(List<int> stringList) {
            try {
                Texture2D tileTex = socket.Content.Load<Texture2D>("Image/tile");
                var totalRows = stringList[0];

                int PADDING = 160 + tileTex.Height;

                const int OFFSET = 1;
                const int BLOCK_SIZE = 2;

                _tiles = new JumperTile[totalRows];
                for (int i = 0; i < totalRows; i++) {
                    int posInPercent = stringList[OFFSET + i * BLOCK_SIZE];
                    int type = stringList[OFFSET + i * BLOCK_SIZE + 1];
                    Vector2 position = new Vector2((stage.X - tileTex.Width) / 100 * posInPercent,
                        (stage.Y - tileTex.Height) - (i * PADDING) - PADDING);

                    _tiles[i] = new JumperTile(socket, _spriteBatch, tileTex, position, stage, (JumperTile.TileType)type);

                }

                _tiles.Last().isLast = true;

                return true;
            } catch (Exception e) {
                MessageBox.Show("Error occurred when generating map, file may corrupted", "Jumper", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _tiles = null;
                return false;
            }
        }

        public void GoToNextLevel() {
            int pointer = levels.FindIndex(s => s == levelName);
            if (pointer != levels.Count - 1) {
                levelName = levels[pointer + 1];
                socket.ReloadLevel(levelName, score);
            } else {
                xnaMBox.Show("Jumper", "You WIN!!" + "\nYour score: " + score, new[] { "OK" });
                socket.BackToMenu();
            }
        }

        public void GoToMenu() {
            //socket.Components.Clear();
            socket.BackToMenu();
        }

        public void Explode() {
            explosion.start();
        }


    }
}
