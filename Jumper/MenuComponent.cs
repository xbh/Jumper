using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jumper {
    public class MenuComponent : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private SpriteFont regularFont, hilightFont;
        private List<string> menuItems;
        private int selectedIndex = 0;


        private Vector2 position;
        private Color regularColor = Color.Black;
        private Color hilightColor = Color.DarkCyan;


        private KeyboardState oldState;




        public MenuComponent(Game game, SpriteBatch spriteBatch,
            SpriteFont regularFont,
            SpriteFont hilightFont,
            string[] menus) : base(game) {
            this.spriteBatch = spriteBatch;
            this.regularFont = regularFont;
            this.hilightFont = hilightFont;

            menuItems = menus.ToList<string>();

            position = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);
        }

        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }

        public override void Draw(GameTime gameTime) {
            Vector2 temp = position;
            spriteBatch.Begin();

            for (int i = 0; i < menuItems.Count; i++) {
                if (selectedIndex == i) {
                    spriteBatch.DrawString(hilightFont, menuItems[i], temp, hilightColor);
                    temp.Y += hilightFont.LineSpacing;
                } else {
                    spriteBatch.DrawString(regularFont, menuItems[i], temp, regularColor);
                    temp.Y += regularFont.LineSpacing;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down)) {
                selectedIndex++;
                if (selectedIndex == menuItems.Count) {
                    selectedIndex = 0;
                }

            }
            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up)) {
                selectedIndex--;
                if (selectedIndex == -1) {
                    selectedIndex = menuItems.Count - 1;
                }

            }

            oldState = ks;

            base.Update(gameTime);
        }
    }
}
