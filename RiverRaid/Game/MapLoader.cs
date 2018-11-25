using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public void LoadMaps(ContentManager content)
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
        public void LoadMaps(ContentManager content)
        {
            Console.WriteLine(content.RootDirectory);
            var filePath = Path.Combine(content.RootDirectory, "Android/Maps.txt");

            using (var levelfile = TitleContainer.OpenStream(filePath))
            {
                using (var sr = new StreamReader(levelfile))
                {
                    var line = sr.ReadLine();
                    while (line != null)
                    {
                        //Console.WriteLine(line);

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



                        line = sr.ReadLine();
                    }
                }
            }
            Console.WriteLine(Maps.Count);
            Console.WriteLine(Maps.Count);
            Console.WriteLine(Maps.Count);
            Console.WriteLine(Maps.Count);
            Console.WriteLine(Maps.Count);
        }

#endif
    }

    class SingleMap
    {
        public List<string> lines = new List<string>();
    }
}
