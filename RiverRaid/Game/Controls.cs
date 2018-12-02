using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiverRaid.RaidGame
{
    class Controls
    {
        public Vector2 move = new Vector2(0, 0);

        public Rectangle padBackgroundPosition = new Rectangle(50, 600, 350, 350);
        public Rectangle padBackgroundTexPosition = new Rectangle(0, 0, 350, 350);

        public Rectangle padPosition = new Rectangle(50, 600, 350, 350);
        public Rectangle padTexPosition = new Rectangle(350, 0, 350, 350);

        public Rectangle shotPosition = new Rectangle(1520, 600, 350, 350);
        public Rectangle shotTexPosition = new Rectangle(700, 0, 350, 350);

        public float padOpacity = 0.7f;
        public float shotOpacity = 0.7f;

        public int touchId = -1;

        public void MovePad(int x, int y)
        {
            float temp_x = (x - 225) / 175.0f;
            float temp_y = -((y - 775) / 175.0f);
            Vector2 temp_vec = new Vector2(temp_x, temp_y);

            float scale = 1.0f;

            if (temp_vec.Length() > 1.0f)
            {
                scale = 1.0f / temp_vec.Length();

            }
            move.X = temp_vec.X * scale;
            move.Y = temp_vec.Y * scale;

            padPosition.X = 50 + (int)(move.X * 175.0f);
            padPosition.Y = 600 + (int)(move.Y * -175.0f);
            
        }

        public void Reset()
        {
            padPosition = new Rectangle(50, 600, 350, 350);
            move = new Vector2(0, 0);
            touchId = -1;
        }
        

        public Controls()
        {

        }
    }
}
