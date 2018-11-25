using System;
using System.Collections.Generic;
using System.Text;

namespace RiverRaid
{
    class Timer
    {
        // lista wszystkich stworzonych timerow
        public static List<Timer> CreatedTimers = new List<Timer>();
        private static List<Timer> TimersToBeDeleted = new List<Timer>();


        public delegate void Action();
        private Action action;

        public float actualTime = 0.0f;
        public float period = 0.0f;

        // akcja (najlepiej w lambdzie); czas po jakim sie ona odpali
        public Timer(Action action, float period)
        {
            this.period = period;
            this.action = (Action)action;
            CreatedTimers.Add(this);
        }

        private void Update(int deltaTime)
        {
            actualTime += deltaTime;
            if(actualTime > period)
            {
                action();

                TimersToBeDeleted.Add(this);
            }
        }

        public static void UpdateAllTimers(int deltaTime)
        {
            foreach(Timer t in TimersToBeDeleted)
            {
                CreatedTimers.Remove(t);
            }
            TimersToBeDeleted.Clear();

            foreach(Timer t in CreatedTimers)
            {
                t.Update(deltaTime);
            }
        }

    }
}
