using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RiverRaid.RaidGame
{
    class GameMode : ProgramState
    {

        public enum GameEventEnum
        {
            HIT_ENEMY,
            HIT_WALL,
            TAKE_FUEL,
            LIFE_LOST,
            GAME_OVER,
            SHOOT_ENEMY,
            SHOOT_BRIDGE
        }

        private RiverRaidGame game;

        private TileMap tileMap = new TileMap();

        private Player player = new Player();

        private Texture2D enemies;
        private Texture2D explosions;
        private Texture2D fuel_rate;
        private Texture2D numbers;
        private Texture2D player_bullet;
        private Texture2D road_bridge_fuel;
        private Texture2D tiles;
        private Texture2D controllers;
        private Texture2D bottom;

        private SoundEffect destroy;
        private SoundEffect hit;
        private SoundEffect take_fuel;
        private Song shot;

        private SpriteFont font;

        Boolean fail = false;


        private void GameEvent(GameEventEnum ev)
        {
            switch (ev)
            {
                case GameEventEnum.HIT_ENEMY:
                    fail = true;
                    break;
                case GameEventEnum.HIT_WALL:
                    fail = true;
                    break;
                case GameEventEnum.TAKE_FUEL:
                    //increase fuel
                    break;
                case GameEventEnum.LIFE_LOST:
                    player.lifes -= 1;
                    break;
                case GameEventEnum.GAME_OVER:
                    break;
                case GameEventEnum.SHOOT_ENEMY:
                    //get points
                    break;
                case GameEventEnum.SHOOT_BRIDGE:
                    break;
            }
        }

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
            Rectangle playerPos = new Rectangle((int)player.X, (int)player.Y, 90, 120);
            if (player.movingLeft)
                spriteBatch.Draw(player_bullet, playerPos, new Rectangle(96, 0, 95, 120), Color.White);
            else if (player.movingRight)
                spriteBatch.Draw(player_bullet, playerPos, new Rectangle(96+96, 0, 95, 120), Color.White);
            else
                spriteBatch.Draw(player_bullet, playerPos, new Rectangle(0, 0, 95, 120), Color.White);

            for (int row = 0; row < 11; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    int currentTile = tileMap.TileMapArray[row, col];
                    if(currentTile > 0)
                    {
                        Rectangle location = new Rectangle(col * 120, (row-1) * 120 + (int)tileMap.tileMapShift, 120, 120);
                        if (currentTile < 7)
                        {
                            spriteBatch.Draw(tiles, location, new Rectangle((currentTile - 1) * 120, 0, 120, 120), Color.White);
                        }
                        else if(currentTile <= 9)
                        {
                            spriteBatch.Draw(road_bridge_fuel, location, new Rectangle((currentTile - 7) * 120, 0, 120, 120), Color.White);
                        }

                    }
                }
            }


            //*****************************
            //  INTERFACE AND CONTROLS
            //*****************************
            spriteBatch.Draw(bottom, new Rectangle(0, RiverRaidGame.GAME_HEIGHT - 180, 
                RiverRaidGame.GAME_WIDTH, RiverRaidGame.GAME_HEIGHT), Color.White);
            //fuel
            spriteBatch.Draw(fuel_rate, new Rectangle(RiverRaidGame.GAME_WIDTH / 2 - 160, RiverRaidGame.GAME_HEIGHT - 100,
                fuel_rate.Width, fuel_rate.Height), new Rectangle(0,0,fuel_rate.Width - 25, fuel_rate.Height), Color.White);
            //fuel_rate
            spriteBatch.Draw(fuel_rate, new Rectangle(RiverRaidGame.GAME_WIDTH / 2 + 148, RiverRaidGame.GAME_HEIGHT - 100,
                25, fuel_rate.Height), new Rectangle(fuel_rate.Width - 25, 0, 25, fuel_rate.Height), Color.White);

            #if ANDROID
            spriteBatch.Draw(controllers, new Rectangle(0, 0, RiverRaidGame.GAME_WIDTH, RiverRaidGame.GAME_HEIGHT), Color.White);
            #endif

            string scoreString = player.score.ToString();

            int numWidth = numbers.Width / 10;
            for (int i = 0; i < scoreString.Length; i++)
            {
                
                Rectangle rect_y = new Rectangle(i * numWidth + 1150 - scoreString.Length * numWidth, RiverRaidGame.GAME_HEIGHT - 160, numWidth, numbers.Height);
                
                switch(scoreString[i])
                {
                    case '0':spriteBatch.Draw(numbers, rect_y, new Rectangle(9 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '1':spriteBatch.Draw(numbers, rect_y, new Rectangle(0 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '2':spriteBatch.Draw(numbers, rect_y, new Rectangle(1 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '3':spriteBatch.Draw(numbers, rect_y, new Rectangle(2 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '4':spriteBatch.Draw(numbers, rect_y, new Rectangle(3 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '5':spriteBatch.Draw(numbers, rect_y, new Rectangle(4 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '6':spriteBatch.Draw(numbers, rect_y, new Rectangle(5 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '7':spriteBatch.Draw(numbers, rect_y, new Rectangle(6 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '8':spriteBatch.Draw(numbers, rect_y, new Rectangle(7 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                    case '9':spriteBatch.Draw(numbers, rect_y, new Rectangle(8 * numWidth, 0, numWidth, numbers.Height), Color.White);break;
                }
            }

            

            Rectangle rectangle_y = new Rectangle(numWidth + 660, RiverRaidGame.GAME_HEIGHT - 60, numbers.Width / 10, numbers.Height);
            spriteBatch.Draw(numbers, rectangle_y, new Rectangle(2 * numWidth, 0, numWidth, numbers.Height), Color.White);

           if (fail)
            {
                fail = false;
                if (player.lifes == 3) spriteBatch.Draw(numbers, rectangle_y, new Rectangle(1 * numWidth, 0, numWidth, numbers.Height), Color.White);
                else if (player.lifes == 2) spriteBatch.Draw(numbers, rectangle_y, new Rectangle(0 * numWidth, 0, numWidth, numbers.Height), Color.White);
                else if (player.lifes == 1) spriteBatch.Draw(numbers, rectangle_y, new Rectangle(9 * numWidth, 0, numWidth, numbers.Height), Color.White);
            }

           //TODO decrease and increase fuel

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


            destroy = content.Load<SoundEffect>("Shared/Audio/destroy");
            hit = content.Load<SoundEffect>("Shared/Audio/hit");
            take_fuel = content.Load<SoundEffect>("Shared/Audio/take_fuel");
            shot = content.Load<Song>("Shared/Audio/shot");
            


            

            //font = content.Load<SpriteFont>("Shared/Fonts/scoreFont");
#if ANDROID
            controllers = content.Load<Texture2D>("Android/Textures/controllers");
            var filePath = Path.Combine(content.RootDirectory, "Android/Maps.txt");

            using (var levelfile = TitleContainer.OpenStream(filePath))
            {
                using (var sr = new StreamReader(levelfile))
                {
                    var line = sr.ReadLine();
                    while (line != null)
                    {
                        Console.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
            }
#endif

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
#if WINDOWS
            if (game.previousKeyboardState.IsKeyUp(Keys.Left))
                player.movingLeft = false;
            if (game.previousKeyboardState.IsKeyUp(Keys.Right))
                player.movingRight = false;
#endif

            tileMap.UpdateTilePosition(deltaTime, player.playerXVelocity);
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
            /*
            if (key == Keys.P)
            {
                if (game.GamePaused)
                    game.ResumeGame();
                else
                    game.PauseGame();
            }*/
        }

        public GameMode(RiverRaidGame game)
        {
            this.game = game;
        }
    }
}
