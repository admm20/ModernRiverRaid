using Microsoft.Xna.Framework.Content;
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

        public void LoadMaps(ContentManager content)
        {
            mapLoader.LoadMaps(content);
        }

        // TYLKO od 0 do 9 !!!
        private int CharToInt(char c)
        {
            return int.Parse("" + c);
        }

        public void LoadFirstMap()
        {
            tileMapShift = 0;
            currentMap = mapLoader.Maps[0];
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    TileMapArray[row, col] = CharToInt(mapLoader.Maps[0].lines[row][col]);
                }
            }
        }

        public void UpdateTilePosition(int deltaTime, float playerXVelocity, GameMode gameMode)
        {
            float tempShift = tileMapShift + deltaTime * playerXVelocity;
            if (tempShift > 400.0f)
            {
                // 2 tile w dol podczas 1 update? 
            }
            else if (tempShift > 120.0f)
            {
                // przesun wszystko o 1 w dol
                MoveTilesDown(gameMode);
                tileMapShift = tempShift - 120.0f;
            }
            else
                tileMapShift = tempShift;

        }

        private void MoveTilesDown(GameMode gameMode)
        {
            for(int row = 10; row > 0; row--)
            {
                for(int col = 0; col < 16; col++)
                {
                    TileMapArray[row, col] = TileMapArray[row - 1, col];
                }
            }

            tilePassedCounter++;

            Random rand = new Random();

            // find other matching map
            if (tilePassedCounter > 8)
            {
                List<SingleMap> matchingMaps = new List<SingleMap>();
                foreach(SingleMap map in mapLoader.Maps)
                {
                    if (currentMap.lines[0] == map.lines[8])
                        matchingMaps.Add(map);
                }
                
                int r = rand.Next(0, matchingMaps.Count);
                while(currentMap == matchingMaps[r])
                    r = rand.Next(0, matchingMaps.Count);
                currentMap = matchingMaps[r];

                tilePassedCounter = 0;
            }


            for (int col = 0; col < 16; col++)
            {
                TileMapArray[0, col] = CharToInt(currentMap.lines[8 - tilePassedCounter][col]);

                int r = rand.Next(0, 100);
                if (r == 2 && TileMapArray[0, col] == 0)
                {
                    // generuj paliwo
                    GameObject fuel = new GameObject(GameObjectType.FUEL, col * 120 + 15, -120, 65, 120);
                    gameMode.listOfFuels.Add(fuel);
                }
                else if(r == 3 && TileMapArray[0, col] == 0)
                {
                    GameObject ship = new GameObject(GameObjectType.SHIP, col * 120 + 15, -120, 124, 120);
                    gameMode.listOfEnemies.Add(ship);
                }
                else if (r == 4 && TileMapArray[0, col] == 0)
                {
                    GameObject heli = new GameObject(GameObjectType.HELICOPTER, col * 120 + 15, -120, 90, 120);
                    gameMode.listOfEnemies.Add(heli);
                }

            }

        }
    }
}
