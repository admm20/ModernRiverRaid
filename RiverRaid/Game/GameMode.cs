using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RiverRaid.RaidGame
{

    public enum GameEvent
    {
        
    }

    class GameMode : ProgramState
    {
        private RiverRaidGame game;

        private TileMap tileMap = new TileMap();

        private Player player = new Player();

        public override void BlackToNormalTransitionFinished()
        {
        }

        public override void CursorClick(int x, int y)
        {
            
        }

        public override void CursorHolding(int x, int y, int id)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle playerPos = new Rectangle((int)player.X, (int)player.Y, 120, 120);
            if (player.movingLeft)
                spriteBatch.Draw(player.texture, playerPos, new Rectangle(115, 0, 90, 120), Color.White);
            if (player.movingRight)
                spriteBatch.Draw(player.texture, playerPos, new Rectangle(200, 0, 90, 120), Color.White);
            else
                spriteBatch.Draw(player.texture, playerPos, new Rectangle(0, 0, 120, 120), Color.White);

            for(int row = 0; row < 11; row++)
            {
                for(int col = 0; col < 16; col++)
                {
                    
                }
            }
        }

        public override void LoadContent(ContentManager content)
        {
            player.texture = content.Load<Texture2D>("Shared/Textures/player_bullet");
        }

        public override void NormalToBlackTransitionFinished(bool dozmiany)
        {
            
        }

        public override void OnEnter()
        {
            // Reset all player's values
            // todo 

            tileMap.LoadMaps();
            tileMap.LoadFirstMap();
            Timer t1 = new Timer(() => { Console.WriteLine("xd"); }, 1000);
            Timer t2 = new Timer(() => { Console.WriteLine("xD"); }, 2000);
            Timer t3 = new Timer(() => { Console.WriteLine("XDDD"); }, 3000);
        }

        public override void Update(int deltaTime, RiverRaidGame game)
        {
            if (player.movingLeft)
            {
                player.X -= deltaTime * 0.7f;
            }
            else if (player.movingRight)
            {
                player.X += deltaTime * 0.7f;
            }

            if(game.previousKeyboardState.IsKeyUp(Keys.Left))
                player.movingLeft = false;
            if (game.previousKeyboardState.IsKeyUp(Keys.Right))
                player.movingRight = false;

            
        }


        public override void KeyboardKeyDown(Keys key)
        {
            if (key == Keys.Left)
            {
                player.movingLeft = true;
            }

            if (key == Keys.Right)
            {
                player.movingRight = true;
            }
        }

        public override void KeyboardKeyClick(Keys key)
        {
            if (key == Keys.P)
            {
                if (game.GamePaused)
                    game.ResumeGame();
                else
                    game.PauseGame();
            }
        }

        public GameMode(RiverRaidGame game)
        {
            this.game = game;
        }
    }
}
