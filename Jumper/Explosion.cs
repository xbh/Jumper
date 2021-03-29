using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jumper {
    class Explosion : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        public Vector2 Position { set; get; }
        private int dimention;
        private List<Rectangle> frames;
        private int frameIndex;
        private int delay;
        private int delayCounter;
        private JumperMan man;

        private const int COL = 5;
        private const int ROW = 5;


        public Explosion(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 position, int delay, JumperMan man) : base(game) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.Position = position;
            this.delay = delay;
            this.man = man;
            dimention = tex.Width / COL;
            hide();
            createFrames();
        }

        private void createFrames() {
            frames = new List<Rectangle>();
            for (int i = 0; i < COL; i++) {
                for (int j = 0; j < ROW; j++) {
                    Rectangle rect = new Rectangle(i * dimention, j * dimention, dimention, dimention);
                    frames.Add(rect);
                }
            }
        }

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            if (frameIndex >= 0) {
                spriteBatch.Draw(tex, Position, frames[frameIndex], Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
            //Position = man.Position;
            delayCounter++;
            if (delayCounter > delay) {
                frameIndex++;
                if (frameIndex > COL - 1) {
                    frameIndex = -1;
                    hide();
                }

                delayCounter = 0;
            }
            base.Update(gameTime);
        }

        public void hide() {
            Enabled = false;
            Visible = false;
        }

        public void start() {
            Enabled = true;
            Visible = true;
            frameIndex = -1;
        }
    }
}
