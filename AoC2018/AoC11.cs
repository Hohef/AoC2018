using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC11
    {
        struct sPoint {  public int x, y, xXx; }
        const int GRIDSERIALNUMBER = 1308;

        public static string Function1()
        {
            int[,] fuelcell = new int[301, 301];


            //Calculate the Power Level of each cell
            for (int row = 1; row < 301; row++)
            {
                for (int col = 1; col < 301; col++)
                {
                    int rackid = row + 10;
                    int powerlevel = rackid * col;
                    powerlevel += GRIDSERIALNUMBER;
                    powerlevel *= rackid;
                    int hundred = (powerlevel / 100) % 10;
                    fuelcell[row, col] = hundred - 5;
                }
            }

            //Determine largest 3x3 section
            int largest3x3 = 0;
            sPoint gridRef; gridRef.x = 0; gridRef.y = 0;
            for (int row = 1; row < 298; row++)
            {
                for (int col = 1; col < 298; col++)
                {
                    int start = row;
                    int stop = start + 3;
                    int colStart = col;
                    int colStop = colStart + 3;
                    int totalpower = 0;
                    for (int rowIdx = start; rowIdx < stop; rowIdx++)
                        for (int colIdx = colStart; colIdx < colStop; colIdx++)
                            totalpower += fuelcell[rowIdx, colIdx];
                    if (totalpower > largest3x3)
                    {
                        largest3x3 = totalpower;
                        gridRef.x = start;
                        gridRef.y = colStart;
                    }
                }
            }

            return string.Format("{0},{1}", gridRef.x, gridRef.y);
        }

        public static string Function2()
        {
            int[,] fuelcell = new int[301, 301];

            //Calculate the Power Level of each cell
            for (int row = 1; row < 301; row++)
            {
                for (int col = 1; col < 301; col++)
                {
                    int rackid = row + 10;
                    int powerlevel = rackid * col;
                    powerlevel += GRIDSERIALNUMBER;
                    powerlevel *= rackid;
                    int hundred = ((powerlevel / 100) % 10) - 5;
                    fuelcell[row, col] = hundred;
                }
            }

            //Determine largest XxX section
            int largestXxX1 = 0;
            sPoint gridRef; gridRef.x = 0; gridRef.y = 0; gridRef.xXx = 0;
            for (int row = 1; row < 301; row++)
            {
                for (int col = 1; col < 301; col++)
                {
                    //Power of the 1x1 grid at coordinate [row,col]
                    int totalPower = fuelcell[row, col];
                    int subRow = row;
                    int subCol = col;

                    for (int xx = 2; xx < 300; xx++)
                    {
                        if (row + xx > 300 || col + xx > 300) break;

                        subRow++;
                        subCol++;
                        for (int i = 0; i < xx; i++)
                            totalPower += fuelcell[subRow, col + i] + fuelcell[row + i, subCol];
                        //Subtract off last cell as it was counted twice
                        totalPower -= fuelcell[subRow, subCol];

                        if (totalPower > largestXxX1)
                        {
                            largestXxX1 = totalPower;
                            gridRef.x = row;
                            gridRef.y = col;
                            gridRef.xXx = xx;
                        }
                    }
                }
            }     

            return string.Format("{0},{1},{2}", gridRef.x, gridRef.y,gridRef.xXx);
        }
    }
}
