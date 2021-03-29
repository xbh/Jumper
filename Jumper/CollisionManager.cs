using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Jumper {
    public class CollisionManager : GameComponent {
        private JumperMan man;
        private JumperTile tile;
        private Vector2 stage;
        // todo: add sound effects here
        private SoundEffect jumpEffect;
        private Game game;
        private JumperMain main;


        public CollisionManager(Game game, JumperMain main, JumperMan man, JumperTile tile, Vector2 stage) : base(game) {
            this.man = man;
            this.tile = tile;
            this.stage = stage;
            this.game = game;
            this.main = main;
            jumpEffect = game.Content.Load<SoundEffect>("Sound/jump");

        }

        public override void Update(GameTime gameTime) {
            Rectangle manRect = man.getBoundRect();
            Rectangle tileRect = tile.getBoundRect();

            Vector2 bouncy = new Vector2(0, -11);

            if (manRect.Intersects(tileRect) && tile.Enabled) {
                 man.speed = bouncy;
                jumpEffect.Play();
                main.Explode();
                switch (tile.Type) {
                    case JumperTile.TileType.fragile:
                        main.score += 3;
                        man.speed = bouncy;
                        tile.speed = new Vector2(0, 10);
                        //tile.Visible = false;
                        //tile.Enabled = false;
                        break;
                    case JumperTile.TileType.Slide:
                        main.score += 2;
                        break;
                    case JumperTile.TileType.Static:
                        main.score++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (tile.isLast) {
                    //MessageBox.Show("Congrats");
                    main.GoToNextLevel();


                }
            }


        }

    }
}
