using System;
using System.Collections.Generic;
using System.Text;

namespace RiverRaid.RaidGame
{
    class TileMap
    {

        // 16 x 11, ale mapa jest w stanie pomiescic 16x9 tile
        // jest 11 a nie 9, bo troche musi wychodzic poza mape, zeby gracz sie nie skapnal
        public int[,] TileMapArray = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        // przesuniecie tilow w dol (wartosci od 0 do 200)
        public float tileMapShift = 0;

        private MapLoader mapLoader = new MapLoader();

        private SingleMap currentMap;
        private int tilePassedCounter = 8;

        public void LoadMaps()
        {
            mapLoader.LoadMaps();
        }

        // TYLKO od 0 do 9 !!!
        private int CharToInt(char c)
        {
            return int.Parse("" + c);
        }

        public void LoadFirstMap()
        {
            currentMap = mapLoader.Maps[0];
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    TileMapArray[row, col] = CharToInt(mapLoader.Maps[0].lines[row][col]);
                }
            }
        }

        public void UpdateTilePosition(int deltaTime, float playerXVelocity)
        {
            float tempShift = tileMapShift + deltaTime * playerXVelocity;
            if (tempShift > 400.0f)
            {
                // 2 tile w dol podczas 1 update? 
            }
            else if (tempShift > 120.0f)
            {
                // przesun wszystko o 1 w dol
                MoveTilesDown();
                tileMapShift = tempShift - 120.0f;
            }
            else
                tileMapShift = tempShift;

        }

        private void MoveTilesDown()
        {
            for(int row = 10; row > 0; row--)
            {
                for(int col = 0; col < 16; col++)
                {
                    TileMapArray[row, col] = TileMapArray[row - 1, col];
                }
            }

            tilePassedCounter++;

            // find other matching map
            if (tilePassedCounter > 8)
            {
                List<SingleMap> matchingMaps = new List<SingleMap>();
                foreach(SingleMap map in mapLoader.Maps)
                {
                    if (currentMap.lines[0] == map.lines[8])
                        matchingMaps.Add(map);
                }

                Random rand = new Random();
                int r = rand.Next(0, matchingMaps.Count);
                
                Console.WriteLine("LOSOWANKO: " + r);
                currentMap = matchingMaps[r];

                tilePassedCounter = 0;
            }
            for (int col = 0; col < 16; col++)
            {
                TileMapArray[0, col] = CharToInt(currentMap.lines[8 - tilePassedCounter][col]);
            }

        }
    }
}
