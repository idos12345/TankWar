using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



namespace TankWar
{
    class Map
    {
        private S.CitySpot[,] CityMat;
        private Random rnd;
        public Map()
        {
            CityMat = new S.CitySpot[S.cityWidth, S.cityHeight];
            rnd = new Random();
        }

        // Properties
        public S.CitySpot this[int x, int y]
        {
            get
            {
                return CityMat[x, y];
            }
            set
            {
                CityMat[x, y] = value;
            }
        }

        public void BuildCity()
        {
            Drawable tmp;
            List<Point> PlatesList = new List<Point>();

            // Put Floor Plates On the Area
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    tmp = new Drawable("floor", true, false, new Vector3(i * 800 + 400, 0, j * 800 + 400), 400);
                    PlatesList.Add(new Point(i, j));
                    CityMat[i, j] = S.CitySpot.EMPTY;
                }
           }

            // Dont Randomize the first or last positions since there are tanks there
            PlatesList.RemoveAt(0);
            PlatesList.RemoveAt(PlatesList.Count - 1);

            // Randomize Buldings Positions
            for (int i = 0; i < S.NumOfBuildings; i++)
            {
                int plateInd = rnd.Next(PlatesList.Count);
                CityMat[PlatesList[plateInd].X, PlatesList[plateInd].Y] = S.CitySpot.BUILDING;
                S.Objects.Add(new Drawable("Building2", true, false, new Vector3(PlatesList[plateInd].X * 800 + 400, 0, PlatesList[plateInd].Y * 800 + 400), 4));
            }

            S.CityMap[0, 0] = S.CitySpot.MY_TANK;
            S.CityMap[14, 14] = S.CitySpot.COMPUTER_TANK;
        }


    }
}
