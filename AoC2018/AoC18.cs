using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC18
    {
        const int AREADIM = 50;
        const long TIMER = 10;
        const long TIMER2 = 1000000000;
        //const long TIMER2 = 10000;
        static char[,] foresta = new char[AREADIM+2, AREADIM+2];
        static char[,] forestb = new char[AREADIM+2, AREADIM+2];
        public static void ReadFile()
        {
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input18.txt");
            int row = 1;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                int i = 1;
                foreach(char c in line)
                {
                    foresta[row, i] = c;
                    forestb[row, i] = c;
                    i++;
                }
                row++;
            }
        }

        private static void Count(ref int wood, ref int open, ref int lumber,char acre)
        {
            if (acre == '.') open++;
            else if (acre == '|') wood++;
            else if (acre == '#') lumber++;
        }

        private static char ApplyRules(ref char[,] forest, int row, int col)
        {
            int sumwood = 0;
            int sumopen = 0;
            int sumlumber = 0;

            Count(ref sumwood, ref sumopen, ref sumlumber, forest[row - 1, col - 1]);
            Count(ref sumwood, ref sumopen, ref sumlumber, forest[row - 1, col]);
            Count(ref sumwood, ref sumopen, ref sumlumber, forest[row - 1, col + 1]);
            Count(ref sumwood, ref sumopen, ref sumlumber, forest[row, col - 1]);
            Count(ref sumwood, ref sumopen, ref sumlumber, forest[row, col + 1]);
            Count(ref sumwood, ref sumopen, ref sumlumber, forest[row + 1, col - 1]);
            Count(ref sumwood, ref sumopen, ref sumlumber, forest[row + 1, col]);
            Count(ref sumwood, ref sumopen, ref sumlumber, forest[row + 1, col + 1]);

            char acre = forest[row, col];
            if (acre == '.')
            {
                if (sumwood >= 3) return '|';
                return '.';
            }
            else if (acre == '|')
            {
                if (sumlumber >= 3) return '#';
                return '|';
            }
            else// if (acre == '#')
            {
                if (sumlumber >= 1 && sumwood >= 1) return '#';
                return '.';
            }
        }

        public static void Print(ref char[,] forest)
        {
            
            for (int row = 1; row <= AREADIM; row++)
            {
                string line = "";
                for (int col = 1; col <= AREADIM; col++)
                    line += forest[row, col];
                System.Diagnostics.Debug.Print(line);
            }
        }


        public static int Function1()
        {
            ReadFile();
            bool UseForestA = true;
            for (int timer = 0; timer < TIMER; timer++)
            {
                if (UseForestA)
                {
                    for (int row = 1; row <= AREADIM; row++)
                    {
                        for (int col = 1; col <= AREADIM; col++)
                            forestb[row, col] = ApplyRules(ref foresta, row, col);
                    }
                    System.Diagnostics.Debug.Print("----Forest B----");
                    Print(ref forestb);
                }
                else
                {
                    for (int row = 1; row <= AREADIM; row++)
                    {
                        for (int col = 1; col <= AREADIM; col++)
                            foresta[row, col] = ApplyRules(ref forestb, row, col);
                    }
                    System.Diagnostics.Debug.Print("----Forest A----");
                    Print(ref foresta);
                }

                UseForestA = !UseForestA;
            }

            //Count the forest
            if (UseForestA)
                return SumResource(ref foresta);
            else
                return SumResource(ref forestb);
        }

        public static int Function2()
        {
            ReadFile();
            bool UseForestA = true;
            int prevResCnt = 0;
            int curResCnt = 0;
            LinkedList<int> values = new LinkedList<int>();
            for (long timer = 0; timer < TIMER2; timer++)
            {
                prevResCnt = curResCnt;
                if (UseForestA)
                {
                    for (int row = 1; row <= AREADIM; row++)
                    {
                        for (int col = 1; col <= AREADIM; col++)
                            forestb[row, col] = ApplyRules(ref foresta, row, col);
                    }
                    curResCnt = SumResource(ref forestb);
                }
                else
                {
                    for (int row = 1; row <= AREADIM; row++)
                    {
                        for (int col = 1; col <= AREADIM; col++)
                            foresta[row, col] = ApplyRules(ref forestb, row, col);
                    }
                    curResCnt = SumResource(ref foresta);
                }

                UseForestA = !UseForestA;
                if (values.Count > 200) values.Clear();
                if (values.Count > 2)
                {
                    //Looking for pattern to repeat itself
                    if (values.First.Value == prevResCnt)

                        if (values.First.Next.Value == curResCnt)
                        {
                            long index = (TIMER2 - timer) % (long)(values.Count - 1);
                            LinkedListNode<int> node = values.First;
                            for (int i = 0; i < index; i++)
                                node = node.Next;
                            return node.Value;
                        }
                }
                values.AddLast(curResCnt);
            }

            //If all else fails the brute force will solve the problem
            //Count the forest
            if (UseForestA)
                return SumResource(ref foresta);
            else
                return SumResource(ref forestb);
        }

        static int SumResource(ref char[,] forest)
        {
            int sumwood = 0;
            int sumopen = 0;
            int sumlumber = 0;

            for (int row = 1; row <= AREADIM; row++)
            {
                for (int col = 1; col <= AREADIM; col++)
                    Count(ref sumwood, ref sumopen, ref sumlumber, forest[row, col]);
            }

            return sumwood * sumlumber;
        }
    }
}
