using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRaid.Menu
{
    class MainMenu : ProgramState
    {
        private RiverRaidGame game;

        private Texture2D testText;

        // DO WYWALENIA
        private List<Rectangle> MultitouchTest = new List<Rectangle>();


        public override void NormalToBlackTransitionFinished(bool dozmiany)
        {
            if (!dozmiany)
            {
                Console.WriteLine("halo policja");
                game.BlackToNormalTransition();

            }
        }

        public override void BlackToNormalTransitionFinished()
        {
            // JESZCZE NIE ZAPROGRAMOWANE!!! ale raczej nie bedzie potrzebne
        }

        public override void CursorClick(int x, int y)
        {
            game.NormalToBlackTransition();
        }

        public override void CursorHolding(int x, int y, int id)
        {
            MultitouchTest.Add(new Rectangle(x, y, testText.Width, testText.Height));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(testText, new Rectangle(20, 50, testText.Width, testText.Height), Color.White);

            foreach(Rectangle r in MultitouchTest)
            {
                spriteBatch.Draw(testText, r, Color.White);
            }
            MultitouchTest.Clear();
        }

        public override void LoadContent(ContentManager content)
        {
            testText = content.Load<Texture2D>("Shared/Textures/test");
        }

        public override void OnEnter()
        {
        }

        public override void Update(int deltaTime, RiverRaidGame game)
        {
        }

        public MainMenu(RiverRaidGame game)
        {
            this.game = game;
        }
    }
}
