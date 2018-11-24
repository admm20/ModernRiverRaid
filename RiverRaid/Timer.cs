using System;
using System.Collections.Generic;
using System.Text;

namespace RiverRaid
{
    class Timer
    {
        // lista wszystkich stworzonych timerow
        public static List<Timer> CreatedTimers = new List<Timer>();

        public delegate void Action();

        public float actualTime = 0.0f;
        public float dueTime = 0.0f;
        public float period = 0.0f;

        public Timer(Object action, float dueTime, float period)
        {

        }

        private void Update(int deltaTime)
        {
            if(actualTime > period)
            {

            }
        }

        public void UpdateAllTimers(int deltaTime)
        {
            foreach(Timer t in CreatedTimers)
            {
                t.Update(deltaTime);
            }
        }

    }
}
