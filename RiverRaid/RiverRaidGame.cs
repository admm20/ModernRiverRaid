#region Using Statements
using System;
#if ANDROID
using Android.OS;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using RiverRaid.Menu;
using RiverRaid.RaidGame;

#endregion

namespace RiverRaid
{
    public class RiverRaidGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ProgramState currentState;

        GameMode game;
        MainMenu mainMenu;

        public bool GamePaused = false;

        public static int GAME_WIDTH = 1920;
        public static int GAME_HEIGHT = 1080;

        private Texture2D blackTile;
        private Texture2D pauseTexture;

        private RenderTarget2D renderer;
        private Rectangle rendererPosition = new Rectangle(0, 0, 1, 1);

        private float transition_opacity = 0.0f;
        private int transition_timer = 0;
        private bool transition_fade_in = false; // true - fade in // false - fade out
        private bool transition_timer_working = false;
        
        public int mouse_x = 0;
        public int mouse_y = 0;

#if WINDOWS
        private MouseState previousMouseState;
        public KeyboardState previousKeyboardState;

#endif

        // przejscie z ciemnego ekranu w jasny
        public void BlackToNormalTransition()
        {
            transition_opacity = 1.0f;
            transition_timer = 0;
            transition_timer_working = true;
            transition_fade_in = true;
        }

        // przejscie z jasnego ekranu w ciemny
        public void NormalToBlackTransition()
        {
            transition_opacity = 0.0f;
            transition_timer = 0;
            transition_timer_working = true;
            transition_fade_in = false;
        }

        public void GoToMenu()
        {
            currentState = mainMenu;
            currentState.OnEnter();
        }

        public void GoToGameMode()
        {

            currentState = game;
            currentState.OnEnter();
        }



        public RiverRaidGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
#if WINDOWS
            graphics.IsFullScreen = false;
            this.IsMouseVisible = true;
#elif ANDROID
            graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#endif
            mainMenu = new MainMenu(this);
            game = new GameMode(this);
        }

        protected override void Initialize()
        {
            
            // TODO: Add your initialization logic here

#if WINDOWS
            Window.AllowUserResizing = true;
            previousMouseState = Mouse.GetState();
#endif

            renderer = new RenderTarget2D(GraphicsDevice, GAME_WIDTH, GAME_HEIGHT);

            // change update frequency
            base.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: use this.Content to load your game content here 
            blackTile = Content.Load<Texture2D>("Shared/Textures/black_tile");
            pauseTexture = Content.Load<Texture2D>("Shared/Textures/paused_game");

            game.LoadContent(Content);
            mainMenu.LoadContent(Content);

            GoToMenu();
            //GoToGameMode();
        }

        private bool backPressed = false;
        protected override void Update(GameTime gameTime)
        {
#if WINDOWS
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#elif ANDROID
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                backPressed = true;
                //Exit();

            }

            if(backPressed && GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released)
            {
                backPressed = false;
                if (GamePaused)
                {
                    GoToMenu();
                    GamePaused = false;
                }
                else if(currentState == mainMenu)
                {
                    Process.KillProcess(Process.MyPid());
                }
                else
                    GamePaused = true;
            }
#endif
            // EFEKTY PRZEJSCIA
            if (transition_timer_working)
            {
                transition_timer += gameTime.ElapsedGameTime.Milliseconds;

                if (transition_fade_in)
                    transition_opacity = 1 - (transition_timer / 1000.0f);
                else
                    transition_opacity = (transition_timer / 1000.0f);

                if (transition_opacity > 1.0f)
                    transition_opacity = 1.0f;
                if (transition_opacity < 0.0f)
                    transition_opacity = 0.0f;

                if (transition_timer > 1000)
                {
                    transition_timer_working = false;
                    currentState.NormalToBlackTransitionFinished(transition_fade_in);
                    //OnFadeFinished(new FadeEffectEventArgs(transition_fade_in));
                }
            }

            // STEROWANIE:
#if WINDOWS
            MouseState mouseStateNow = Mouse.GetState();

            mouse_x = (int)(mouseStateNow.Position.X * ((float)GAME_WIDTH / GraphicsDevice.PresentationParameters.BackBufferWidth));
            mouse_y = (int)(mouseStateNow.Position.Y * ((float)GAME_HEIGHT / GraphicsDevice.PresentationParameters.BackBufferHeight));
            if (previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine(mouse_x);
                currentState.CursorHolding(mouse_x, mouse_y, 0);
                if(mouseStateNow.LeftButton == ButtonState.Released)
                {
                    currentState.CursorClick(mouse_x, mouse_y);
                }
            }
            previousMouseState = mouseStateNow;

            KeyboardState keyboardStateNow = Keyboard.GetState();
            if (keyboardStateNow.GetPressedKeys().Length > 0)
            {
                foreach (Keys k in keyboardStateNow.GetPressedKeys())
                {
                    currentState.KeyboardKeyDown(k);

                }

            }
            if (keyboardStateNow.IsKeyUp(Keys.P) && previousKeyboardState.IsKeyDown(Keys.P))
            {
                currentState.KeyboardKeyClick(Keys.P);
            }

            if (keyboardStateNow.IsKeyUp(Keys.Space) && previousKeyboardState.IsKeyDown(Keys.Space))
            {
                currentState.KeyboardKeyClick(Keys.Space);
            }
            previousKeyboardState = keyboardStateNow;

#elif ANDROID
            TouchCollection touches = TouchPanel.GetState();

            foreach(TouchLocation touch in touches)
            {
                //int touch_x = (int)(touch.Position.X * ((float)GAME_WIDTH / GraphicsDevice.PresentationParameters.BackBufferWidth));
                //int touch_y = (int)(touch.Position.Y * ((float)GAME_HEIGHT / GraphicsDevice.PresentationParameters.BackBufferHeight));
                // lepszy sposob:
                float touch_x = (float)((touch.Position.X - rendererPosition.X) / rendererPosition.Width);
                float touch_y = (float)((touch.Position.Y - rendererPosition.Y) / rendererPosition.Height);
                touch_x *= GAME_WIDTH;
                touch_y *= GAME_HEIGHT;

                switch (touch.State)
                {
                    case TouchLocationState.Invalid:
                        break;
                    case TouchLocationState.Moved:
                        currentState.CursorHolding((int)touch_x, (int)touch_y, touch.Id);
                        break;
                    case TouchLocationState.Pressed:
                        break;
                    case TouchLocationState.Released:
                        currentState.CursorClick((int)touch_x, (int)touch_y, touch.Id);
                        break;
                }
            }
#endif

            if (GamePaused)
            {
                // do nothing
            }
            else
            {
                currentState.Update(gameTime.ElapsedGameTime.Milliseconds, this);
                Timer.UpdateAllTimers(gameTime.ElapsedGameTime.Milliseconds);
            }

            // TODO: Add your update logic here			
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.SetRenderTarget(renderer);

            spriteBatch.Begin();
            if (GamePaused)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Draw(pauseTexture, new Rectangle(GAME_WIDTH / 2, GAME_HEIGHT / 2, pauseTexture.Width, pauseTexture.Height), Color.White);
            }
            else
            {
                GraphicsDevice.Clear(Color.Blue);
                currentState.Draw(spriteBatch);
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            // Obraz będzie wyświetlany na środku ekranu i będą zachowane proporcje niezaleznie od tego,
            // jakie są wymiary okna.
            PresentationParameters windowSize = GraphicsDevice.PresentationParameters;
            rendererPosition = new Rectangle(0, 0, windowSize.BackBufferWidth,
                windowSize.BackBufferHeight);


            if (rendererPosition.Width > rendererPosition.Height * 16.0 / 9.0) // za szerokie
            {
                rendererPosition.Width = (int)(rendererPosition.Height * (16.0 / 9.0));
                rendererPosition.X = (int)((windowSize.BackBufferWidth - rendererPosition.Width) / 2.0);
            }
            else
            {
                rendererPosition.Height = (int)(rendererPosition.Width * (9.0 / 16.0));
                rendererPosition.Y = (int)((windowSize.BackBufferHeight - rendererPosition.Height) / 2.0);
            }


            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(renderer, rendererPosition, Color.White);
            spriteBatch.Draw(blackTile, rendererPosition, Color.White * transition_opacity);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
