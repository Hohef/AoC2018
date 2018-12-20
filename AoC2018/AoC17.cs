using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC17
    {
        struct sScanData
        {
            public int x, y, size;
            public bool bIsVertical;
        }

        static char[,] ground;
        static int largeX, largeY;

        static int lowX = int.MaxValue;
        static int minY = int.MaxValue;

        static void ReadFile()
        {
            largeX = 0; largeY = 0;
            List<sScanData> scannedData = new List<sScanData>();

            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input17.txt");
            while(!reader.EndOfStream)
            {
                sScanData scanData = new sScanData();
                string[] line = reader.ReadLine().Split(',');
                string[] subline = line[0].Split('=');
                string[] subline2 = line[1].Substring(3).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (subline[0] == "x")
                {
                    scanData.x = int.Parse(subline[1]);                    
                    scanData.y = int.Parse(subline2[0]);
                    scanData.size = (int.Parse(subline2[1]) - scanData.y) + 1;
                    scanData.bIsVertical = true;
                }
                else
                {
                    scanData.y = int.Parse(subline[1]);
                    scanData.x = int.Parse(subline2[0]);
                    scanData.size = (int.Parse(subline2[1]) - scanData.x) + 1;
                    scanData.bIsVertical = false;
                }

                scannedData.Add(scanData);
                if (scanData.x > largeX) largeX = scanData.x;
                if (scanData.y > largeY) largeY = scanData.y;
                if (scanData.x < lowX) lowX = scanData.x;
                if (scanData.y < minY) minY = scanData.y;
            }

            largeX += 2;
            ground = new char[largeX,largeY+1];

            foreach(sScanData scanData in scannedData)
            {
                if (scanData.bIsVertical)
                    for (int y = 0; y < scanData.size; y++)
                        ground[scanData.x, scanData.y + y] = '#';
                else
                    for (int x = 0; x < scanData.size; x++)
                        ground[scanData.x+x, scanData.y] = '#';
            }
        }

        static private bool FindBottom(ref int x, ref int y)
        {
            bool found = false;
            do
            {
                if (ground[x, y + 1] == '|') break;
                //Check down
                if (ground[x, y + 1] == '#' || ground[x, y + 1] == '~')
                {
                    found = true;
                    break;
                }                
                ground[x, y] = '|';
                y++;
            } while (!found && y < largeY);

            if (!found)
                ground[x, y] = '|';

            return found;
        }

        static private bool CanFill(int x, int y)
        {
            //Check left
            bool foundleft = false;
            for (int row = x; row > 0; row--)
            {
                if (ground[row, y + 1] == '\0') return false;
                if (ground[row,y] == '#')
                {
                    foundleft = true;
                    break;
                }                
            }
            bool foundright = false;
            for (int row = x; row < largeX;row++)
            {
                if (ground[row, y + 1] == '\0') return false;
                if (ground[row, y] == '#' )
                {
                    foundright = true;
                    break;
                }                
            }

            return foundright & foundleft;
        }

        static void FillRow(int x, int y)
        {
            for (int row = x; row > 0; row--)
            {
                if (ground[row, y] == '#')
                    break;
                ground[row, y] = '~';
            }
            for (int row = x; row < largeX; row++)
            {
                if (ground[row, y] == '#')
                    break;
                ground[row, y] = '~';
            }
        }

        static private void FillHole(int x, int y)
        {
            if (FindBottom(ref x, ref y))
            {
                if (CanFill(x, y))
                {
                    FillRow(x, y);
                    FillHole(x, y - 1);
                }
                else
                {
                    //Fill Left
                    for (int row = x; row > 0; row--)
                    {
                        if (ground[row, y] == '#' || ground[row,y] == '~')
                            break;
                        ground[row, y] = '|';
                        if (ground[row, y + 1] == '\0')
                        {
                            FillHole(row, y);
                            break;
                        }
                    }
                    //Fill Right
                    for (int row = x; row < largeX; row++)
                    {
                        if (ground[row, y] == '#' || ground[row, y] == '~')
                            break;
                        ground[row, y] = '|';
                        if (ground[row, y + 1] == '\0')
                        {
                            FillHole(row, y);
                            break;
                        }
                    }
                }
            }
        }

        static public int Function1()
        {
            ReadFile();

            //Fill with water.  Water starts at (500,0)
            //Find first clay whole
            int x = 500; int y = 1;
            //This is starting point=
            FillHole(x, y);

            //Count Water
            int waterCnt = 0;
            for (int i = 0; i < largeX; i++)
                for (int j = minY; j < largeY+1; j++)
                    if (ground[i, j] == '~' || ground[i, j] == '|') waterCnt++;

            for (int j = 0; j < largeY; j++)
            {
                string s = "";
                for (int i = lowX; i < largeX; i++)


                    if (ground[i, j] == '\0')
                        s += '.';
                    else
                        s += ground[i, j];
                System.Diagnostics.Debug.Print(s);
            }

            return waterCnt;
        }

        static public int Function2()
        {
            ReadFile();

            //Fill with water.  Water starts at (500,0)
            //Find first clay whole
            int x = 500; int y = 1;
            //This is starting point=
            FillHole(x, y);

            //Count Water
            int waterCnt = 0;
            for (int i = 0; i < largeX; i++)
                for (int j = minY; j < largeY + 1; j++)
                    if (ground[i, j] == '~') waterCnt++;

            for (int j = 0; j < largeY; j++)
            {
                string s = "";
                for (int i = lowX; i < largeX; i++)


                    if (ground[i, j] == '\0')
                        s += '.';
                    else
                        s += ground[i, j];
                System.Diagnostics.Debug.Print(s);
            }

            return waterCnt;
        }
    }
}
