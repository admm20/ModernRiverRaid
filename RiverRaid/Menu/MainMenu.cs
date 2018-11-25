using System;
using System.Collections.Generic;
using System.Text;
#if ANDROID
using Android.OS;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RiverRaid.Menu
{
    class MainMenu : ProgramState
    {
        private RiverRaidGame game;

        private Texture2D testText;
        private Texture2D splash;
        private Texture2D menu;
        private Texture2D start_game;
        private Texture2D start_game_selected;
        private Texture2D exit;
        private Texture2D exit_selected;


        private Boolean show_menu = true;
        private int option = 0; 
        private int position_x = 0;
        private int position_y = 0;
        private Rectangle start_rectangle= new Rectangle(RiverRaidGame.GAME_WIDTH / 3 - 50, RiverRaidGame.GAME_HEIGHT / 2 - 130, 714, 130);
        private Rectangle exit_rectangle = new Rectangle(RiverRaidGame.GAME_WIDTH / 3 - 50, RiverRaidGame.GAME_HEIGHT / 2 + 130, 714, 130);

        // DO WYWALENIA
        private List<Rectangle> MultitouchTest = new List<Rectangle>();


        public override void NormalToBlackTransitionFinished(bool dozmiany)
        {
            if (!dozmiany)
            {
                game.BlackToNormalTransition(); 
                game.GoToGameMode();
            }
        }

        public override void BlackToNormalTransitionFinished()
        {
            // JESZCZE NIE ZAPROGRAMOWANE!!! ale raczej nie bedzie potrzebne
        }

        public override void CursorClick(int x, int y)
        {
            //android
            if(start_rectangle.Contains(x,y))
                option = 1; 
            else if (exit_rectangle.Contains(x,y))
                option = 2;
            else option = 0;

            switch (option)
            {
                case 0:
                    break;
                case 1:
                    //NormalToBlackTransitionFinished(false);
                    //game.BlackToNormalTransition
                    game.NormalToBlackTransition();
                    break;
                case 2:
#if WINDOWS
                    game.Exit();
                    break;
#endif
#if ANDROID
                    Process.KillProcess(Process.MyPid());
                    break;
#endif
                    
            }
        }

        public override void CursorHolding(int x, int y, int id)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //TODO dodanie splasha z timerem

            //pc
            if(show_menu)
            {
                spriteBatch.Draw(menu, new Rectangle(0, 0, RiverRaidGame.GAME_WIDTH, RiverRaidGame.GAME_HEIGHT), Color.White);
                spriteBatch.Draw(start_game, new Rectangle(RiverRaidGame.GAME_WIDTH/3 - 50, 
                    RiverRaidGame.GAME_HEIGHT/2 - 100, start_game.Width, start_game.Height), Color.White);
                spriteBatch.Draw(exit, new Rectangle(RiverRaidGame.GAME_WIDTH / 3 - 50,
                    RiverRaidGame.GAME_HEIGHT / 2 + 100, exit.Width, exit.Height), Color.White);

                if (position_x > RiverRaidGame.GAME_WIDTH / 3 - 50 && position_x < RiverRaidGame.GAME_WIDTH / 3 - 50 + 714
                    && position_y > RiverRaidGame.GAME_HEIGHT / 2 - 100 && position_y < RiverRaidGame.GAME_HEIGHT / 2 - 100 + 56)
                {
                    spriteBatch.Draw(start_game_selected, new Rectangle(RiverRaidGame.GAME_WIDTH / 3 - 50,
                    RiverRaidGame.GAME_HEIGHT / 2 - 100, start_game.Width, start_game.Height), Color.White);
                    option = 1;
                }
                else if (position_x > RiverRaidGame.GAME_WIDTH / 3 - 50 && position_x < RiverRaidGame.GAME_WIDTH / 3 - 50 + 714
                    && position_y > RiverRaidGame.GAME_HEIGHT / 2 + 100 && position_y < RiverRaidGame.GAME_HEIGHT / 2 + 100 + 56)
                {
                    spriteBatch.Draw(exit, new Rectangle(RiverRaidGame.GAME_WIDTH / 3 - 50,
                    RiverRaidGame.GAME_HEIGHT / 2 + 100, exit.Width, exit.Height), Color.White);
                    option = 2;
                }
                else option = 0;

            }
        }

        public override void LoadContent(ContentManager content)
        {
            splash = content.Load<Texture2D>("Shared/Textures/splash");
            menu = content.Load<Texture2D>("Shared/Textures/menu");
            start_game = content.Load<Texture2D>("Shared/Textures/start_game");
            start_game_selected = content.Load<Texture2D>("Shared/Textures/start_game_selected");
            exit = content.Load<Texture2D>("Shared/Textures/exit");
            exit_selected = content.Load<Texture2D>("Shared/Textures/exit_selected");
      
        }

        public override void OnEnter()
        {
            
        }

        public override void Update(int deltaTime, RiverRaidGame game)
        {
            // pc
            position_x = game.mouse_x;
            position_y = game.mouse_y;

        }

        public override void KeyboardKeyDown(Keys key)
        {

        }

        public override void KeyboardKeyClick(Keys key)
        {

        }

        public MainMenu(RiverRaidGame game)
        {
            this.game = game;
        }
    }
}
