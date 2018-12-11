using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC10
    {
        class CPoint
        {
            public int x;
            public int y;
            public int vX;
            public int vY;

            public void Move()
            {
                x += vX;
                y += vY;
            }

            public bool NextTo(CPoint right)
            {
                return (Math.Abs(x - right.x) < 2 && Math.Abs(y - right.y) < 2);
            }
        }

        static List<CPoint> points;

        static int maxX = 0;
        static int minX = 100000;
        static int maxY = 0;
        static int minY = 1000000;

        private static void ReadFile()
        {
            char[] delim = {'<',',','>'};
            points = new List<CPoint>();
            maxX = 0; minX = 100000; maxY = 0; minY = 1000000;

            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input10.txt");
            do
            {
                string[] line = reader.ReadLine().Split(delim);
                CPoint point = new CPoint();
                point.x = int.Parse(line[1].Trim());
                point.y = int.Parse(line[2].Trim());
                point.vX = int.Parse(line[4].Trim());
                point.vY = int.Parse(line[5].Trim());
                points.Add(point);
                if (maxX < point.x) maxX = point.x;
                if (minX > point.x) maxX = point.x;
                if (maxY < point.y) maxY = point.y;
                if (minY > point.y) maxY = point.y;
            } while (!reader.EndOfStream);
        }

        class PointPrint
        {
            CPoint point;
            int index;
        }
        private static void Print()
        {
            //Poor Man print
            bool[] bPrinted = new bool[points.Count];
            int printed = 0;
            int toppoint = int.MaxValue;
            List<SortedList<int,int>> printer = new List<SortedList<int,int>>(points.Count);

            while (printed < points.Count)
            {
                toppoint = int.MaxValue;
                SortedList<int,int> printerRow = new SortedList<int,int>();
                for (int i = 0; i < points.Count; i++)
                {
                    if (bPrinted[i]) continue;

                    if (points[i].y < toppoint)
                    {
                        toppoint = points[i].y;
                        printerRow = new SortedList<int, int>();
                        printerRow.Add(points[i].x, i);
                    }
                    else if (points[i].y == toppoint)
                    {
                        if (printerRow.ContainsKey(points[i].x))
                        {
                            bPrinted[i] = true;
                            printed++;
                        }
                        else                        
                            printerRow.Add(points[i].x, i);
                    }
                }

                printer.Add(printerRow);

                foreach (KeyValuePair<int,int> pair in printerRow)
                {
                    bPrinted[pair.Value] = true;
                    printed++;
                }
            }

            //Length of message
            int topleft = points[printer[0].Values[0]].x;
            int bottomleft = points[printer[printer.Count-1].Values[0]].x;
            int topright = points[printer[0].Values[printer[0].Count-1]].x;
            int bottomright = points[printer[printer.Count - 1].Values[printer[printer.Count-1].Count-1]].x;

            int ll = Math.Min(topleft, bottomleft);
            int rr = Math.Min(topright, bottomright);
            int distance = Math.Abs(Math.Abs(ll) - Math.Abs(rr)) + 1;



            string s = " ";
            string srow = s.PadLeft(distance, ' ');
            char[] crow = new char[distance];
            int index;
            int internalPos;
            foreach (SortedList<int,int> row in printer)
            {
                int loc = 0;
                index = 0;
                internalPos = ll;
                for (int i = 0;i < distance;i++)
                {
                    if (index < row.Count && internalPos == points[row.Values[index]].x)
                    {
                        index++;
                        crow[loc] = '#';
                    }
                    else
                        crow[loc] = ' ';

                    loc++;
                    internalPos++;
                }

                string outp = new string(crow);
                System.Diagnostics.Debug.Print(outp);
            }
        }


        public static int Function1()
        {
            int seconds = 0;
            ReadFile();
            while (true)
            {
                seconds++;
                foreach (CPoint point in points)
                    point.Move();

                bool bmatched = false;
                for (int i = 0; i < points.Count; i++)
                {
                    bmatched = false;
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (i == j) continue;
                        //Determine if next to at least one other point
                        if (points[i].NextTo(points[j]))
                        {
                            bmatched = true;
                            break;
                        }
                    }

                    //Not next to another point, need to move points
                    if (!bmatched) break;
                }

                if (bmatched)
                {
                    Print();
                    return seconds;
                }
            }


            return -1;
        }

        public static int Function2()
        {
            return -1;
        }
    }
}
