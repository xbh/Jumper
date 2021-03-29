using System;
using System.Collections.Generic;
using System.Text;
using Jumper.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Jumper {
    public class GameSocket : Game {
        GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        // declare all scene references
        private JumperMenu menuScene;
        private JumperMain mainScene;
        private JumperHelp helpScene;
        private JumperAbout aboutScene;
        private Song bgm;



        public GameSocket() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 600;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            Shared.stage = new Vector2(_graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);




            base.Initialize();
        }

        //private void hideAllScenes()
        //{
        //    GameScene gs = null;
        //    foreach (GameComponent  item in this.Components)
        //    {
        //        if (item is GameScene)
        //        {
        //            gs = (GameScene)item;
        //            gs.hide();
        //        }
        //    }

        //}

        private void hideAllScenes() {
            foreach (GameScene item in Components) {
                item.hide();
            }
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            menuScene = new JumperMenu(this, spriteBatch);
            this.Components.Add(menuScene);

            //other scenes will be here

            mainScene = new JumperMain(this, spriteBatch, _graphics);
            this.Components.Add(mainScene);

            helpScene = new JumperHelp(this, spriteBatch);
            this.Components.Add(helpScene);

            aboutScene = new JumperAbout(this, spriteBatch);
            this.Components.Add(aboutScene);



            //show only startscene
            menuScene.show();

            //todo: BGM here
            bgm = Content.Load<Song>("Sound/bgm");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(bgm);



        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            /* if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                 Exit();*/

            // TODO: Add your update logic here

            int selectedIndex = 0;
            KeyboardState ks = Keyboard.GetState();

            if (menuScene.Enabled) {
                selectedIndex = menuScene.Menu.SelectedIndex;
                if (selectedIndex == 0 && ks.IsKeyDown(Keys.Enter)) {
                    hideAllScenes();
                    //startScene.hide();
                    mainScene.show();
                }

                if (selectedIndex == 1 && ks.IsKeyDown(Keys.Enter)) {
                    hideAllScenes();
                    //startScene.hide();
                    helpScene.show();
                }

                if (selectedIndex == 3 && ks.IsKeyDown(Keys.Enter)) {
                    hideAllScenes();
                    //startScene.hide();
                    aboutScene.show();
                }

                //other scenes

                if (selectedIndex == 4 && ks.IsKeyDown(Keys.Enter)) {
                    this.Exit();

                }

            }
            if (helpScene.Enabled) {
                if (ks.IsKeyDown(Keys.Escape)) {
                    helpScene.hide();
                    menuScene.show();

                }
            }

            if (aboutScene.Enabled) {
                if (ks.IsKeyDown(Keys.Escape)) {
                    aboutScene.hide();
                    menuScene.show();

                }
            }

            if (mainScene.Enabled) {
                if (ks.IsKeyDown(Keys.Escape)) {
                    BackToMenu();
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Texture2D backgroundTexture = Content.Load<Texture2D>("Image/background");
            Rectangle backgroundRectangle = new Rectangle(0, 0, 900, 1203);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, backgroundRectangle, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ReloadLevel(string levelName, int score) {
            hideAllScenes();
            mainScene.hide();
            mainScene.Dispose();
            Components.Remove(mainScene);
            mainScene = new JumperMain(this, spriteBatch, _graphics, levelName, score);
            this.Components.Add(mainScene);
            mainScene.show();
        }

        public void BackToMenu() {


            mainScene.Dispose();
            Components.Remove(mainScene);
            mainScene = new JumperMain(this, spriteBatch, _graphics);
            this.Components.Add(mainScene);
            hideAllScenes();
            menuScene.show();
        }
    }
}
