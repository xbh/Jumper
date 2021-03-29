using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jumper {
    public abstract class GameScene : DrawableGameComponent {
        private List<GameComponent> components;
        public List<GameComponent> Components { get => components; set => components = value; }

        public virtual void show() {
            this.Enabled = true;
            this.Visible = true;
        }

        public virtual void hide() {
            this.Enabled = false;
            this.Visible = false;
        }

        public GameScene(Game game) : base(game) {
            components = new List<GameComponent>();
            hide();
        }



        public override void Draw(GameTime gameTime) {
            DrawableGameComponent comp = null;
            foreach (GameComponent item in components) {
                if (item is DrawableGameComponent) {

                    comp = (DrawableGameComponent)item;
                    if (comp.Visible) {
                        comp.Draw(gameTime);
                    }

                }
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
            foreach (GameComponent item in components) {
                if (item.Enabled) {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }




    }
}
