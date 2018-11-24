﻿#region Using Statements
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

        public static int GAME_WIDTH = 1920;
        public static int GAME_HEIGHT = 1080;

        private Texture2D testText;

        private Texture2D blackTile;

        private RenderTarget2D renderer;

        private float transition_opacity = 0.0f;
        private int transition_timer = 0;
        private bool transition_fade_in = false; // true - fade in // false - fade out
        private bool transition_timer_working = false;

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

        public RiverRaidGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
#if WINDOWS
            graphics.IsFullScreen = false;
#elif ANDROID
            graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#endif

            game = new GameMode(this);
            mainMenu = new MainMenu(this);

            currentState = mainMenu;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

#if WINDOWS
            Window.AllowUserResizing = true;
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
            testText = Content.Load<Texture2D>("Shared/Textures/test");
            blackTile = Content.Load<Texture2D>("Shared/Textures/black_tile");

            game.LoadContent(Content);
            mainMenu.LoadContent(Content);
        }


        protected override void Update(GameTime gameTime)
        {
#if WINDOWS
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#elif ANDROID
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Process.KillProcess(Process.MyPid());
                //Exit();

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
            // not implemented yet
            
#elif ANDROID
            TouchCollection touches = TouchPanel.GetState();

            foreach(TouchLocation touch in touches)
            {
                int touch_x = (int)(touch.Position.X * ((float)GAME_WIDTH / GraphicsDevice.PresentationParameters.BackBufferWidth));
                int touch_y = (int)(touch.Position.Y * ((float)GAME_HEIGHT / GraphicsDevice.PresentationParameters.BackBufferHeight));
                switch (touch.State)
                {
                    case TouchLocationState.Invalid:
                        break;
                    case TouchLocationState.Moved:
                        currentState.CursorHolding(touch_x, touch_y, touch.Id);
                        break;
                    case TouchLocationState.Pressed:
                        break;
                    case TouchLocationState.Released:
                        currentState.CursorClick(touch_x, touch_y);
                        break;
                }
            }
#endif


            // TODO: Add your update logic here			
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            GraphicsDevice.SetRenderTarget(renderer);
            GraphicsDevice.Clear(Color.Blue);

            spriteBatch.Begin();
            currentState.Draw(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            // Obraz będzie wyświetlany na środku ekranu i będą zachowane proporcje niezaleznie od tego,
            // jakie są wymiary okna.
            PresentationParameters windowSize = GraphicsDevice.PresentationParameters;
            Rectangle rendererPosition = new Rectangle(0, 0, windowSize.BackBufferWidth,
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


            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(renderer, rendererPosition, Color.White);
            spriteBatch.Draw(blackTile, rendererPosition, Color.White * transition_opacity);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}