using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC3
    {
        static char[] delim = { ' ', ',', ':', 'x' };

        public static int Function1()
        {
            int[,] fabric = new int[1000, 1000];
            string line;
            //Read in cut dimensions
            System.IO.StreamReader input = new StreamReader(@"..\..\Inputs\Input3.txt");            
            while ((line = input.ReadLine()) != null)
            {
                string[] data = line.Split(delim);
                int x = int.Parse(data[2]);
                int y = int.Parse(data[3]);
                int width = int.Parse(data[5]);
                int height = int.Parse(data[6]);
                for (int row = x; row < x+width; row++)
                {
                    for (int col = y; col < y+height; col++)
                    {
                        if (fabric[row, col] == 0)
                            fabric[row, col] = 1;
                        else
                            fabric[row, col] = 2;
                    }
                }
            }

            //Sum up overlap
            int overlap = 0;
            for (int row = 0; row < 1000; row++)
            {
                for (int col = 0; col < 1000; col++)
                {
                    if (fabric[row, col] == 2)
                        overlap++;
                }
            }

            return overlap;
        }

        public static int Function2()
        {
            List<bool> ids = new List<bool>(1000);
            ids.Add(true); //add id=0
            int[,] fabric = new int[1000, 1000];
            string line;
            //Read in cut dimensions
            System.IO.StreamReader input = new StreamReader(@"..\..\Inputs\Input3.txt");
            while ((line = input.ReadLine()) != null)
            {
                string[] data = line.Split(delim);
                int id = int.Parse(data[0].Substring(1));
                int x = int.Parse(data[2]);
                int y = int.Parse(data[3]);
                int width = int.Parse(data[5]);
                int height = int.Parse(data[6]);
                ids.Add(false);
                for (int row = x; row < x + width; row++)
                {
                    for (int col = y; col < y + height; col++)
                    {
                        if (fabric[row, col] == 0)
                            fabric[row, col] = id;
                        else if (fabric[row,col] != -1)
                        {
                            ids[fabric[row, col]] = true;
                            ids[id] = true;
                            fabric[row, col] = -1;                            
                        }
                        else
                            fabric[row, col] = -1;
                    }
                }                
            }

            for(int i = 0; i < ids.Count(); i++)
                if (!ids[i]) return i;

            return -1;
        }
    }
}
