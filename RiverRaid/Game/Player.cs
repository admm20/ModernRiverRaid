using System;
using System.Collections.Generic;
using System.Text;

namespace RiverRaid.RaidGame
{
    class Player
    {

        public float X = 1920.0f / 2.0f, Y = 790.0f;

        //public float accelerationX = 0.0f;
        //public float accelerationY = 0.0f;

        public float playerXVelocity = 0.35f;

        public bool movingLeft = false;
        public bool movingRight = false;

        public bool movingFaster = false;
        public bool movingSlower = false;

        public bool canShoot = true;

        public int lifes = 3;
        public int score = 0;

    }
}
