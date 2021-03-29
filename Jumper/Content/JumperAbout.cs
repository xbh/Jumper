using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jumper.Content {
    class JumperAbout : GameScene {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        public JumperAbout(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            tex = game.Content.Load<Texture2D>("Image/About");
        }


        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            //version 2
            spriteBatch.Draw(tex, Vector2.Zero, Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
