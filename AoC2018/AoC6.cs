using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC6
    {
        struct sCoordinate
        {
            public int x, y;
            public sCoordinate(int cX, int cY) { x = cX; y = cY; }
        }

        class sManDistance
        {
            public int cordIdx;
            public int distance;
        }

        static char[] delim = { ',', ' ' };
        static List<sCoordinate> coordinates = new List<sCoordinate>(50);
        static int maxX = 0, maxY = 0;

        private static void ReadFile()
        {
            coordinates.Clear();
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input6.txt");
            do
            {
                string [] line = reader.ReadLine().Split(delim,StringSplitOptions.RemoveEmptyEntries);
                sCoordinate coordinate = new sCoordinate(int.Parse(line[0]), int.Parse(line[1]));
                coordinates.Add(coordinate);
                if (coordinate.x > maxX) maxX = coordinate.x;
                if (coordinate.y > maxY) maxY = coordinate.y;

            } while (!reader.EndOfStream);
        }

        public static int Function1()
        {
            ReadFile();
            int squareSize = Math.Max(maxX+1, maxY+1);

            int[] cordArea = new int[coordinates.Count];
            sManDistance[,] map = new sManDistance[squareSize, squareSize];

            for (int cordIdx = 0; cordIdx < coordinates.Count; cordIdx++)
            {
                sCoordinate coordinate = coordinates[cordIdx];
                for (int idxX = 0; idxX < squareSize; idxX++)
                {
                    for (int idxY = 0; idxY < squareSize; idxY++)
                    {
                        int manhattandistance = Math.Abs(coordinate.x - idxX) + Math.Abs(coordinate.y - idxY);

                        if (map[idxX, idxY] == null)
                        {
                            map[idxX, idxY] = new sManDistance { cordIdx = cordIdx, distance = manhattandistance };
                            cordArea[cordIdx]++;
                        }
                        else if (map[idxX, idxY].distance == manhattandistance)
                        {
                            if (map[idxX, idxY].cordIdx == -1) continue;  //already accounted for
                            cordArea[map[idxX, idxY].cordIdx]--;
                            map[idxX, idxY].cordIdx = -1;
                        }
                        else if (map[idxX, idxY].distance > manhattandistance)
                        {
                            if (map[idxX, idxY].cordIdx != -1)
                                cordArea[map[idxX, idxY].cordIdx]--;
                            map[idxX, idxY].cordIdx = cordIdx;
                            map[idxX, idxY].distance = manhattandistance;
                            cordArea[cordIdx]++;
                        }
                    }
                }
            }

            //Zero out infinite space
            int outsideDim = squareSize - 1;
            for (int outside = 0; outside < squareSize; outside++)
            {
                if (map[outside, 0].cordIdx != -1)
                    cordArea[map[outside, 0].cordIdx] = -1;

                if (map[0, outside].cordIdx != -1)
                    cordArea[map[0, outside].cordIdx] = -1;

                if (map[outsideDim, outside].cordIdx != -1)
                    cordArea[map[outsideDim, outside].cordIdx] = -1;

                if (map[outside, outsideDim].cordIdx != -1)
                    cordArea[map[outside, outsideDim].cordIdx] = -1;
            }

            //Determine largets area
            int maxarea = 0;
            for (int cordIdx = 0; cordIdx < coordinates.Count; cordIdx++)
                if (cordArea[cordIdx] > maxarea)
                    maxarea = cordArea[cordIdx];


            return maxarea;
        }

        public static int Function2()
        {
            ReadFile();
            int squareSize = Math.Max(maxX + 1, maxY + 1);

            int inRegion = 0;
            int cordTotalDistance = 0;
            for (int idxX = 0; idxX < squareSize; idxX++)
            {
                for (int idxY = 0; idxY < squareSize; idxY++)
                {
                    cordTotalDistance = 0;
                    for (int cordIdx = 0; cordIdx < coordinates.Count; cordIdx++)
                    {
                        sCoordinate coordinate = coordinates[cordIdx];
                        cordTotalDistance += Math.Abs(coordinate.x - idxX) + Math.Abs(coordinate.y - idxY);                        
                    }
                    if (cordTotalDistance < 10000) inRegion++;
                }                
            }

            return inRegion;
        }
    }
}
