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

        private Controls controls = new Controls();


        private SpriteFont font;

        Boolean fail = false;

        public List<GameObject> listOfShots = new List<GameObject>();
        public List<GameObject> listOfFuels = new List<GameObject>();
        public List<GameObject> listOfEnemies = new List<GameObject>();
        private bool helicopterAnimation = true;

        private bool moveWorld = false;


        private void GameEvent(GameEventEnum ev)
        {
            switch (ev)
            {
                case GameEventEnum.HIT_ENEMY:
                    fail = true;
                    hit.Play();
                    GameEvent(GameEventEnum.LIFE_LOST);
                    break;
                case GameEventEnum.HIT_WALL:
                    fail = true;
                    hit.Play();
                    GameEvent(GameEventEnum.LIFE_LOST);
                    break;
                case GameEventEnum.TAKE_FUEL:
                    //increase fuel
                    take_fuel.Play();
                    break;
                case GameEventEnum.LIFE_LOST:
                    player.lifes -= 1;
                    player.X = -3000;
                    moveWorld = false;
                    if (player.lifes < 1)
                    {
                        GameEvent(GameEventEnum.GAME_OVER);
                    }
                    else
                    {
                        Timer loseTimer = new Timer(() => Revive(), 1500);
                    }
                    break;
                case GameEventEnum.GAME_OVER:
                    Timer goToMenuDelay = new Timer(() => game.GoToMenu(), 1500);
                    break;
                case GameEventEnum.SHOOT_ENEMY:
                    //get points
                    player.score += 10;
                    destroy.Play();
                    break;
                case GameEventEnum.SHOOT_BRIDGE:
                    player.score += 10;
                    destroy.Play();
                    break;
            }
        }

        private void Revive()
        {

            tileMap.LoadFirstMap();
            player.X = 1920 / 2;
            player.playerYVelocity = 0.5f;
            player.fuelLeft = 100;
            moveWorld = true; 
            listOfEnemies.Clear();
            listOfFuels.Clear();
            listOfShots.Clear();
        }

        public override void BlackToNormalTransitionFinished()
        {
        }
        
        public override void CursorClick(int x, int y, int id)
        {
#if ANDROID
            if(id == controls.touchId)
            {
                controls.touchId = -1;
                controls.padPosition = new Rectangle(50, 600, 350, 350);
                controls.move = new Vector2(0, 0);
            }

            if(controls.shotPosition.Contains(x, y))
            {
                // pew pew
                GameObject bullet = new GameObject(GameObjectType.SHOT, player.Position.Center.X - 13, player.Y - 15, 15, 50);
                listOfShots.Add(bullet);
                MediaPlayer.Play(shot);
            }
            game.GamePaused = false;
            controls.shotOpacity = 0.7f;
#endif
        }

        public override void CursorHolding(int x, int y, int id)
        {
#if ANDROID
            if(controls.padPosition.Contains(x, y))
            {
                controls.touchId = id;
            }

            if(controls.touchId == id)
            {
                controls.MovePad(x, y);
            }

            if(controls.shotPosition.Contains(x, y))
            {
                controls.shotOpacity = 1.0f;
            }
#endif
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            // in game fuels
            foreach (GameObject o in listOfFuels)
            {
                spriteBatch.Draw(road_bridge_fuel, o.hitbox, new Rectangle(390, 0, o.hitbox.Width, o.hitbox.Height), Color.White);
            }

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


            // bullets
            foreach(GameObject o in listOfShots)
            {
                spriteBatch.Draw(player_bullet, o.hitbox, new Rectangle(320, 36, o.hitbox.Width, o.hitbox.Height), Color.White);
            }

            foreach(GameObject o in listOfEnemies)
            {
                if(o.type == GameObjectType.HELICOPTER)
                {
                    if (helicopterAnimation)
                    {
                        spriteBatch.Draw(enemies, o.hitbox, new Rectangle(0, 0, o.hitbox.Width, o.hitbox.Height), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(enemies, o.hitbox, new Rectangle(91, 0, o.hitbox.Width, o.hitbox.Height), Color.White);
                    }
                }
                else if(o.type == GameObjectType.SHIP)
                {
                    spriteBatch.Draw(enemies, o.hitbox, new Rectangle(276, 0, o.hitbox.Width, o.hitbox.Height), Color.White);

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
            spriteBatch.Draw(fuel_rate, new Rectangle(RiverRaidGame.GAME_WIDTH / 2 + 148 - (int)((100-player.fuelLeft)*2.78 ), RiverRaidGame.GAME_HEIGHT - 100,
                25, fuel_rate.Height), new Rectangle(fuel_rate.Width - 25, 0, 25, fuel_rate.Height), Color.White);

#if ANDROID
            //spriteBatch.Draw(controllers, new Rectangle(0, 0, RiverRaidGame.GAME_WIDTH, RiverRaidGame.GAME_HEIGHT), Color.White);
            spriteBatch.Draw(controllers, controls.padBackgroundPosition, controls.padBackgroundTexPosition, Color.White * 0.7f);
            spriteBatch.Draw(controllers, controls.padPosition, controls.padTexPosition, Color.White * 0.7f);
            spriteBatch.Draw(controllers, controls.shotPosition, controls.shotTexPosition, Color.White * controls.shotOpacity);
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
            if (player.lifes == 3) spriteBatch.Draw(numbers, rectangle_y, new Rectangle(2 * numWidth, 0, numWidth, numbers.Height), Color.White);
            else if (player.lifes == 2) spriteBatch.Draw(numbers, rectangle_y, new Rectangle(1 * numWidth, 0, numWidth, numbers.Height), Color.White);
            else if (player.lifes == 1) spriteBatch.Draw(numbers, rectangle_y, new Rectangle(0 * numWidth, 0, numWidth, numbers.Height), Color.White);
            else if (player.lifes == 0) spriteBatch.Draw(numbers, rectangle_y, new Rectangle(9 * numWidth, 0, numWidth, numbers.Height), Color.White);
            

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

            // naprawa buga z dzwiekiem?
            MediaPlayer.Play(shot);
            MediaPlayer.Stop();



            tileMap.LoadMaps(content);

            //font = content.Load<SpriteFont>("Shared/Fonts/scoreFont");
#if ANDROID
            controllers = content.Load<Texture2D>("Android/Textures/controllers_new");

#endif

        }

        public override void NormalToBlackTransitionFinished(bool dozmiany)
        {
            
        }

        public override void OnEnter()
        {
            controls.Reset();
            Revive();
            moveWorld = false;
            player.lifes = 3;
            player.score = 0;
            Timer t1 = new Timer(() => { moveWorld = true; }, 1500);
        }
        
        private void UpdateBulletPosition(int deltaTime)
        {
            for(int i = listOfShots.Count - 1; i >= 0; i--)
            {
                GameObject bullet = listOfShots[i];
                bullet.y -= deltaTime * 0.9f;
                if(bullet.y < -10)
                {
                    listOfShots.Remove(bullet);
                    continue;
                }

                for (int j = listOfFuels.Count - 1; j >= 0; j--)
                {
                    GameObject fuel = listOfFuels[j];
                    if (fuel.hitbox.Intersects(bullet.hitbox))
                    {
                        listOfFuels.Remove(fuel);
                        listOfShots.Remove(bullet);
                        GameEvent(GameEventEnum.SHOOT_ENEMY);
                        continue;
                    }
                }

                for (int j = listOfEnemies.Count - 1; j >= 0; j--)
                {
                    GameObject enemy = listOfEnemies[j];
                    if (enemy.hitbox.Intersects(bullet.hitbox))
                    {
                        listOfEnemies.Remove(enemy);
                        listOfShots.Remove(bullet);
                        GameEvent(GameEventEnum.SHOOT_ENEMY);
                        continue;
                    }
                }


                for(int row = 0; row < 11; row++)
                {
                    for(int col = 0; col < 16; col++)
                    {
                        if(tileMap.TileMapArray[row, col] == 8 || tileMap.TileMapArray[row, col] == 9)
                        {
                            Rectangle bridgeTile = new Rectangle(col * 120, (row-1) * 120 + (int)tileMap.tileMapShift, 120, 120);
                            if (bridgeTile.Intersects(bullet.hitbox))
                            {
                                GameEvent(GameEventEnum.SHOOT_BRIDGE);
                                listOfShots.Remove(bullet);
                                for (int x = 0; x < 16; x++)
                                {
                                    if (tileMap.TileMapArray[row, x] == 8 || tileMap.TileMapArray[row, x] == 9)
                                        tileMap.TileMapArray[row, x] = 0;
                                }
                            }
                        }
                    }
                }

            }



        }

        private int heliAnimTime = 0;
        private void UpdateEnemyAndFuelPosition(int deltaTime)
        {
            heliAnimTime += deltaTime;
            if(heliAnimTime > 200)
            {
                helicopterAnimation = !helicopterAnimation;
                heliAnimTime = 0;
            }

            for (int i = listOfFuels.Count - 1; i >= 0; i--)
            {
                GameObject fuel = listOfFuels[i];
                if (fuel.hitbox.Intersects(player.Position))
                {
                    GameEvent(GameEventEnum.TAKE_FUEL);
                    player.fuelLeft += deltaTime * 0.08f;
                    if (player.fuelLeft > 100)
                        player.fuelLeft = 100;
                }

                fuel.y += deltaTime * player.playerYVelocity;
                if (fuel.y > 1080)
                {
                    listOfFuels.Remove(fuel);
                    continue;
                }
            }

            for (int i = listOfEnemies.Count - 1; i >= 0; i--)
            {
                GameObject enemy = listOfEnemies[i];
                enemy.y += deltaTime * player.playerYVelocity;
                Rectangle hitbox = enemy.hitbox;
                hitbox.Y += 45;
                hitbox.Height -= 80;
                if (enemy.y > 1080)
                {
                    listOfEnemies.Remove(enemy);
                    continue;
                }
                if (hitbox.Intersects(player.Position))
                {
                    listOfEnemies.Remove(enemy);
                    GameEvent(GameEventEnum.HIT_ENEMY);
                    continue;
                }
            }
        }

        public void CheckCollision()
        {
            for (int row = 5; row < 11; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    int tileId = tileMap.TileMapArray[row, col];
                    if ((tileId > 0 &&  tileId < 3) || (tileId > 6 && tileId < 10))
                    {
                        Rectangle tile = new Rectangle(col * 120, (row-1) * 120 + (int)tileMap.tileMapShift, 120, 120);
                        if (tile.Intersects(player.Position))
                        {
                            GameEvent(GameEventEnum.HIT_WALL);
                        }
                    }
                }
            }
        }

        public override void Update(int deltaTime, RiverRaidGame game)
        {
            if (!moveWorld)
                return;
#if WINDOWS
            if (player.movingLeft)
            {
                player.X -= deltaTime * 0.7f;
            }
            else if (player.movingRight)
            {
                player.X += deltaTime * 0.7f;
            }
            if (game.previousKeyboardState.IsKeyUp(Keys.Left))
                player.movingLeft = false;
            if (game.previousKeyboardState.IsKeyUp(Keys.Right))
                player.movingRight = false;
#elif ANDROID
            player.playerYVelocity = (controls.move.Y + 1.2f) / 2.2f;
            player.X += deltaTime * (controls.move.X / (10.0f / 7.0f));
#endif

            tileMap.UpdateTilePosition(deltaTime, player.playerYVelocity, this);
            UpdateBulletPosition(deltaTime);
            UpdateEnemyAndFuelPosition(deltaTime);
            CheckCollision();
            
            player.playerYVelocity = 0.5f;
            player.fuelLeft -= 0.003f * deltaTime;
            if (player.fuelLeft < 0)
            {
                hit.Play();
                GameEvent(GameEventEnum.LIFE_LOST);
            }
        }

        public override void KeyboardKeyDown(Keys key)
        {
            if (moveWorld)
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
            
            if (key == Keys.Up)
            {
                player.playerYVelocity = 1.0f;
            }
            if (key == Keys.Down)
            {
                player.playerYVelocity = 0.2f;
            }

            
        }

        public override void KeyboardKeyClick(Keys key)
        {
            
            if (key == Keys.P)
            {
                game.GamePaused = !game.GamePaused;
            }

            if(key == Keys.Space)
            {
                GameObject bullet = new GameObject(GameObjectType.SHOT, player.Position.Center.X - 13, player.Y - 15, 15, 50);
                listOfShots.Add(bullet);
                MediaPlayer.Play(shot);
            }
        }

        public GameMode(RiverRaidGame game)
        {
            this.game = game;
        }
    }
}
