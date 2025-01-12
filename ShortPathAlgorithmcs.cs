using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankWar
{
    public class Tag
    {
        public Point place {get; set;}
        public double distance { get; set; }
        public bool wasChecked { get; set; }

        public Tag(Point place, double distance)
        {
            this.place = place;
            this.distance = distance;
            this.wasChecked = false;
        }
    }

    static class  ShortPathAlgorithmcs
    {
        private static Tag[,] cityTagMap;
        private static List<Tag> ring1 = new List<Tag>();
        private static List<Tag> ring2 = new List<Tag>();
        private static Point[] DirectionsArr = new Point[8];
        private static Tag from, to;


        public static List<Point> GetPath()
        {
            cityTagMap = SetTagCityMap();
            create_DirArr();
            return find_path();
        }

        private static Tag[,] SetTagCityMap()
        {
            Tag[,] cityTagMap = new Tag[S.cityWidth, S.cityHeight];

            for (int i = 0; i < S.cityWidth; i++)
            {
                for (int j = 0; j < S.cityHeight; j++)
                {
                    cityTagMap[i, j] = new Tag(new Point(i, j), double.MaxValue);
                }
            }

            return cityTagMap;

        }

        private static void create_DirArr()
        {
            int j = 0;
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x != 0 || y != 0)
                    {
                        DirectionsArr[j++] = new Point(x, y);
                    }
                }
            }
        }

        private static List<Point> find_path()
        {
            // Check if there the 2 tanks are on the same plate
            for (int i = 0; i < S.cityWidth; i++)
            {

                for (int j = 0; j < S.cityHeight; j++)
                {

                    if (S.CityMap[i, j] == S.CitySpot.BOTH)
                    {
                        return null;
                    }
                }
            }

            List<Point> PathToGo = new List<Point>();
            Create_rings_of_distance();

            // free each tag
            for (int i = 0; i < S.cityWidth; i++)
            {
                for (int j = 0; j < S.cityHeight; j++)
                {
                    cityTagMap[i, j].wasChecked = false;
                }
            }

            Tag CurrentTag = to;
        

            // Search for the short Path and save it in PathToGo list
            PathToGo.Add(CurrentTag.place);
            while (S.CityMap[CurrentTag.place.X, CurrentTag.place.Y] != S.CitySpot.COMPUTER_TANK)
            {

                Tag nextTag = null;
                foreach (Point k in DirectionsArr)
                {
                    bool in_bounds = is_in_bounds(CurrentTag.place.X + k.X, CurrentTag.place.Y + k.Y, cityTagMap);
                    if (in_bounds)
                    {
                        bool next_null = (nextTag == null);
                        bool closer = false;

                        if (!next_null)
                        {
                            Tag nT = nextTag;

                            closer = cityTagMap[CurrentTag.place.X + k.X, CurrentTag.place.Y + k.Y].distance
                                                  < nT.distance;
                        }

                        bool is_better = next_null || closer;
                        is_better &= is_not_through_cross(k.X, k.Y, CurrentTag.place.X, CurrentTag.place.Y);

                        if (is_better)
                        {
                            bool wasnt_checked =
                                !cityTagMap[CurrentTag.place.X + k.X, CurrentTag.place.Y + k.Y].wasChecked;
                            if (wasnt_checked)
                            {
                                nextTag = cityTagMap[CurrentTag.place.X + k.X, CurrentTag.place.Y + k.Y];
                                nextTag.wasChecked = true;
                            }
                        }

                    }
                }
                PathToGo.Add(nextTag.place);
                CurrentTag = nextTag;

            }

            PathToGo.Reverse();
            return PathToGo;
        }

        private static void Create_rings_of_distance()
        {

            
            ring1.Clear();
            ring2.Clear();

           // prepare each tag for joining a ring

            for (int i = 0; i < S.cityWidth; i++)
            {

                for (int j = 0; j < S.cityHeight; j++)
                {


                    Tag tag = cityTagMap[i, j];
                    tag.wasChecked = false;

                    if (S.CityMap[i, j] == S.CitySpot.EMPTY)
                    {
                        tag.distance = double.MaxValue;
                    }
                    if (S.CityMap[i, j] == S.CitySpot.MY_TANK)
                    {
                        to = cityTagMap[i, j];
                    }
                    if (S.CityMap[i, j] == S.CitySpot.COMPUTER_TANK)
                    {
                        from = cityTagMap[i, j];
                        tag.distance = 0;
                    }
                }
            }
     
          // Add computer tank location to the first ring
            ring1.Add(from);
            bool targetFound = false;
            
            // Loop to create ring 2
            while (!targetFound)
            {
                foreach (Tag t in ring1)
                {
                    foreach (Point kvn in DirectionsArr)
                    {
                        #region Advance in direction and create ring member.
                        #region Init new X and Y
                        int nX = t.place.X + kvn.X;
                        int nY = t.place.Y + kvn.Y;
                        #endregion

                        if (is_in_bounds(nX, nY, cityTagMap))
                        {
                            if (S.CityMap[nX, nY] != S.CitySpot.BUILDING &&
                                is_not_through_cross(
                                         kvn.X, kvn.Y, t.place.X, t.place.Y))
                            {
                                #region If meet target then stop creating rings but finish this one.
                                if (S.CityMap[nX, nY] == S.CitySpot.MY_TANK)
                                    targetFound = true;
                                #endregion

                                double distance = t.distance +
                                                    get_distance(nX, nY, t.place.X, t.place.Y);
                                /**/
                                Tag tag = cityTagMap[nX, nY];
                                if (distance < tag.distance)
                                {
                                    #region Update minimum distance.
                                    tag.distance = distance;
                                    bool already_in_ring = tag.wasChecked;
                                    if (!already_in_ring)
                                    {
                                        ring2.Add(tag);
                                        tag.wasChecked = true;
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }
                }
                #region set ring 2 in ring 1 and clear ring 2 for next loop
                ring1.Clear();
                ring1.AddRange(ring2);
                ring2.Clear();
                #endregion
            }

        }

        private static bool is_not_through_cross(int kvnX, int kvnY, int tX, int tY)
        {
            // Checks if the tank can move through cross (if there are no buldings on the sides)

            int nX = tX + kvnX;
            int nY = tY + kvnY;

            bool alashson = kvnX != 0 || kvnY != 0;
            bool may_move = true;
            bool may_move_alashson = true;
            if (is_in_bounds(nX, tY, cityTagMap) &&
                is_in_bounds(tX, nY, cityTagMap))
                may_move_alashson = !(S.CityMap[nX, tY] == S.CitySpot.BUILDING || S.CityMap[tX, nY] == S.CitySpot.BUILDING);

            if (alashson)
                may_move = may_move_alashson;

            return may_move;
        }

        private static double get_distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }    
        
        private static bool is_in_bounds<T>(int x, int y, T[,] arr)
        {
            return x >= 0 && x < arr.GetLength(0) && y >= 0 && y < arr.GetLength(1);
        }
    }
}
