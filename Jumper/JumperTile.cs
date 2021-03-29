using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jumper {
    public class JumperTile : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        public Vector2 Position { set; get; }
        public Vector2 speed = Vector2.Zero;
        private Vector2 stage;
        public TileType Type;
        public bool isLast;

        public enum TileType {
            Static,
            Slide,
            fragile
        }

        public JumperTile(
            Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position,
            Vector2 stage,
            TileType type,
            bool isLast = false

        ) : base(game) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.Position = position;
            this.stage = stage;
            this.Type = type;
            this.isLast = isLast;
            if (type == TileType.Slide) {
                speed = new Vector2(3, 0);
            }
        }



        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            switch (Type) {
                case TileType.Static:
                    spriteBatch.Draw(tex, Position, Color.White);
                    break;
                case TileType.Slide:
                    spriteBatch.Draw(tex, Position, Color.Magenta);
                    break;
                case TileType.fragile:
                    spriteBatch.Draw(tex, Position, Color.Aqua);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (isLast) {
                spriteBatch.Draw(tex, Position, Color.Green);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
            switch (Type) {
                case TileType.Static:
                    speed = Vector2.Zero;
                    break;
                case TileType.Slide:
                    //speed = new Vector2(3, 0);
                    Position += speed;
                    if (Position.X < 0) {
                        speed.X = -speed.X;
                    }
                    if (Position.X + tex.Width > stage.X) {
                        speed.X = -speed.X;

                    }

                    break;
                case TileType.fragile:
                    Position += speed;
                    //speed = Vector2.Zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Vector2 rollSpeed = new Vector2(0, 1.5f);
            Position += rollSpeed;
            base.Update(gameTime);
        }

        public Rectangle getBoundRect() {
            return new Rectangle((int)Position.X, (int)Position.Y, tex.Width, tex.Height);
        }
    }
}
