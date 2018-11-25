using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RiverRaid.RaidGame
{
    class MapLoader
    {
        public List<SingleMap> Maps = new List<SingleMap>();

        private bool startLoading = false;
        private SingleMap currentMap;
#if WINDOWS
        public void LoadMaps()
        {
            string[] lines = System.IO.File.ReadAllLines("Content/Maps.txt");

            foreach (string line in lines)
            {
                if (line == "[BEGIN]")
                {
                    SingleMap sm = new SingleMap();
                    Maps.Add(sm);
                    startLoading = true;
                    currentMap = sm;
                }
                else if (line == "[END]")
                {
                    startLoading = false;
                    foreach (string m in currentMap.lines)
                    {
                    }
                }
                else if (startLoading)
                {
                    currentMap.lines.Add(line);
                }
                
            }

        }

#elif ANDROID
        public void LoadMaps()
        {
            Console.WriteLine("WCZYTYWANIE XDDDDDDDDDDDDDDDDD");
            
        }

#endif
    }

    class SingleMap
    {
        public List<string> lines = new List<string>();
    }
}
