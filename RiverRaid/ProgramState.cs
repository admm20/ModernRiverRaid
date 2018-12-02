using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiverRaid
{
    public abstract class ProgramState
    {
        public abstract void LoadContent(ContentManager content);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(int deltaTime, RiverRaidGame game);

        // wejście do np. menu, gry
        public abstract void OnEnter();

        // funkcja wywolywana po wcisnieciu i puszczeniu przycisku myszy (win) albo tapnięciu w ekran (android)
        public abstract void CursorClick(int x, int y, int id);

        // wywoływana, gdy LPM jest trzymany. Dla ekranow z multitouchem podawany tez jest id
        public abstract void CursorHolding(int x, int y, int id);

        // wywoływana gdy z jasnego ekranu zrobił się w 100% ciemny
        public abstract void NormalToBlackTransitionFinished(bool dozmiany);

        // wywoływana gdy z ciemnego ekranu zrobił się w 100% jasny
        public abstract void BlackToNormalTransitionFinished();

        // TYLKO DLA WINDOWSA:

        // przycisk key jest trzymany
        public abstract void KeyboardKeyDown(Keys key);

        // przycisk zostal klikniety (wcisniety i puszczony) (tylko P i spacja)
        public abstract void KeyboardKeyClick(Keys key);
    }
}
