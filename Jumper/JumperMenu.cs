using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jumper {
    class JumperMenu : GameScene {
        private MenuComponent menu;
        private SpriteBatch spriteBatch;
        private string[] menuItems = { "Start game", "Help", "High Score", "About", "Quit" };

        private Texture2D texMan;

        public MenuComponent Menu { get => menu; set => menu = value; }

        public JumperMenu(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            texMan = game.Content.Load<Texture2D>("Image/jumper");


            menu = new MenuComponent(game, spriteBatch,
                game.Content.Load<SpriteFont>("Fonts/RegularFont"),
                game.Content.Load<SpriteFont>("Fonts/HilightFont"),
                menuItems);

            this.Components.Add(menu);


        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle headerRectangle = new Rectangle((int)(Shared.stage.X-200) / 2, (int)Shared.stage.Y / 10, 204, 256);
            spriteBatch.Begin();
            spriteBatch.Draw(texMan, headerRectangle, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
