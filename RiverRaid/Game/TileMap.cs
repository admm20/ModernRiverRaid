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
            for(int row = 1; row < 10; row++)
            {
                for(int col = 0; col < 16; col++)
                {
                    TileMapArray[row, col] = CharToInt(mapLoader.Maps[0].lines[row - 1][col]);
                }
            }
        }

        public void UpdateTilePosition(int deltaTime)
        {
            float tempShift = tileMapShift + deltaTime * 0.7f;
            if(tempShift > 200.0f)
            {
                // przesun wszystko o 1 w dol
                MoveTilesDown();
                tileMapShift = tempShift - 200.0f;
            }
            if(tempShift > 400.0f)
            {
                // 2 tile w dol podczas 1 update? 
            }
        }

        private void MoveTilesDown()
        {

        }
    }
}
