using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRaid.RaidGame
{
    class GameMode : ProgramState
    {
        private RiverRaidGame game;

        private Texture2D enemies;
        private Texture2D explosions;
        private Texture2D fuel_rate;
        private Texture2D numbers;
        private Texture2D player_bullet;
        private Texture2D road_bridge_fuel;
        private Texture2D tiles;
        private Texture2D controllers;
        private Texture2D bottom;

        private SpriteFont font;

        private int score = 15309;

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
            spriteBatch.Draw(bottom, new Rectangle(0, RiverRaidGame.GAME_HEIGHT - 180, 
                RiverRaidGame.GAME_WIDTH, RiverRaidGame.GAME_HEIGHT), Color.White);
            spriteBatch.Draw(fuel_rate, new Rectangle(RiverRaidGame.GAME_WIDTH / 2 - 160, RiverRaidGame.GAME_HEIGHT - 100,
                fuel_rate.Width, fuel_rate.Height), new Rectangle(0,0,fuel_rate.Width - 25, fuel_rate.Height), Color.White);
            spriteBatch.Draw(controllers, new Rectangle(0, 0, RiverRaidGame.GAME_WIDTH, RiverRaidGame.GAME_HEIGHT), Color.White);
            string scoreString = score.ToString();

            for (int i = 0; i < scoreString.Length; i++)
            {
                switch(scoreString[i])
                {
                    case '0':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width/10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width/10, numbers.Height),
                            new Rectangle(9 * numbers.Width/10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '1':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width / 10, numbers.Height),
                            new Rectangle(0, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '2':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width/10, numbers.Height),
                            new Rectangle(1 * numbers.Width / 10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '3':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width / 10, numbers.Height),
                            new Rectangle(2 * numbers.Width / 10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '4':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width / 10, numbers.Height),
                            new Rectangle(3 * numbers.Width / 10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '5':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width / 10, numbers.Height),
                            new Rectangle(4 * numbers.Width / 10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '6':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width / 10, numbers.Height),
                            new Rectangle(5 * numbers.Width / 10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '7':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width / 10, numbers.Height),
                            new Rectangle(6 * numbers.Width / 10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '8':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width / 10, numbers.Height),
                            new Rectangle(7 * numbers.Width / 10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                    case '9':
                        spriteBatch.Draw(numbers, new Rectangle(i * numbers.Width / 10 + 1150 - scoreString.Length * numbers.Width / 10, RiverRaidGame.GAME_HEIGHT - 160, numbers.Width / 10, numbers.Height),
                            new Rectangle(8 * numbers.Width / 10, 0, numbers.Width / 10, numbers.Height), Color.White);
                        break;
                }
            }
            
        }

        public override void LoadContent(ContentManager content)
        {
            enemies = content.Load<Texture2D>("Shared/Textures/enemies");
            explosions = content.Load<Texture2D>("Shared/Textures/explosions");
            fuel_rate = content.Load<Texture2D>("Shared/Textures/fuel_rate");
            numbers = content.Load<Texture2D>("Shared/Textures/numbers");
            player_bullet = content.Load<Texture2D>("Shared/Textures/player_bullet");
            road_bridge_fuel = content.Load<Texture2D>("Shared/Textures/road_bridge_fuel");
            tiles = content.Load<Texture2D>("Shared/Textures/tiles");
            bottom = content.Load<Texture2D>("Shared/Textures/bottom");

            controllers = content.Load<Texture2D>("Android/Textures/controllers");

            //font = content.Load<SpriteFont>("Shared/Fonts/scoreFont");


        }

        public override void NormalToBlackTransitionFinished(bool dozmiany)
        {
            
        }

        public override void OnEnter()
        {
            
        }

        public override void Update(int deltaTime, RiverRaidGame game)
        {
            
        }

        public GameMode(RiverRaidGame game)
        {
            this.game = game;
        }
    }
}
