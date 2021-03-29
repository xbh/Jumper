using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jumper {
    public class JumperMan : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 stage;
        public Vector2 speed = Vector2.Zero;

        private Vector2 gravity = new Vector2(0, 0.5f);

        public Vector2 Position {
            get;
            set;
        }


        public JumperMan(
            Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position,
            Vector2 stage) : base(game) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.Position = position;
            this.stage = stage;
            //this.hitSound = hitSound;
        }

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, Position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
          
            if (Position.Y + tex.Height <= stage.Y) {
                speed += gravity;
                Position += speed;
            }

            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left) && Position.X > 0) {
                Position += new Vector2(-5, 0);
                tex = Game.Content.Load<Texture2D>("Image/jumperRe");
            }
            if (ks.IsKeyDown(Keys.Right) && Position.X + tex.Width < stage.X) {
                tex = Game.Content.Load<Texture2D>("Image/jumper");
                Position += new Vector2(5, 0);
            }
        }
        public Rectangle getBoundRect() {
            return new Rectangle((int)Position.X, (int)Position.Y, tex.Width - 20, tex.Height);
        }
    }
}
