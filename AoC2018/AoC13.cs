using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC13
    {
        struct sPoint
        {
            public int x, y;

            public sPoint(int x1, int y1) { x = x1; y = y1; }
        }

        //Cart Movement left, straight, right repeat
        enum eTurn { LEFT, STRAIGHT, RIGHT, END};
        enum eDirection { LEFT, RIGHT, UP, DOWN};

        static readonly eDirection[,] TURN = { { eDirection.DOWN, eDirection.LEFT, eDirection.UP },
                                       { eDirection.UP, eDirection.RIGHT, eDirection.DOWN },
                                       { eDirection.LEFT, eDirection.UP, eDirection.RIGHT },
                                       { eDirection.RIGHT, eDirection.DOWN, eDirection.LEFT } };

        class sCar
        {
            public int x, y;
            public eTurn turn;
            public eDirection direction;
            public bool wrecked;

            public sCar (int x1, int y1, eDirection direction1, eTurn turn1 = eTurn.LEFT)
            {
                x = x1; y = y1; turn = turn1; direction = direction1;
                wrecked = false;
            }

            public bool IsSooner(sCar otherCart)
            {
                if (y < otherCart.y) return true;
                if (otherCart.y == y && x < otherCart.x) return true;
                return false;
            }

            public void Move()
            {
                switch (direction)
                {
                    case eDirection.RIGHT:
                        x++;
                        if (track[x, y] == '\\') direction = eDirection.DOWN;
                        else if (track[x, y] == '/') direction = eDirection.UP;
                        break;
                    case eDirection.LEFT:
                        x--;
                        if (track[x, y] == '\\') direction = eDirection.UP;
                        else if (track[x, y] == '/') direction = eDirection.DOWN;
                        break;
                    case eDirection.UP:
                        y--;
                        if (track[x, y] == '\\') direction = eDirection.LEFT;
                        else if (track[x, y] == '/') direction = eDirection.RIGHT;
                        break;
                    case eDirection.DOWN:
                        y++;
                        if (track[x, y] == '\\') direction = eDirection.RIGHT;
                        else if (track[x, y] == '/') direction = eDirection.LEFT;
                        break;
                }
                
                //Handle an intersection
                if(track[x,y] == '+')
                {
                    direction = TURN[(int)direction, (int)turn];
                    //Go to next turn type
                    turn = (eTurn)(((int)turn + 1) % (int)eTurn.END);
                }
            }
        }

        static char[,] track;
        static List<sCar> carts = new List<sCar>();
        static int TrackW, TrackH;
        static int TrackWM1,TrackHM1;

        private static void ReadFile()
        {
            carts.Clear();
            List<string> sTrack = new List<string>(100);
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input13.txt");
            do
            {
                sTrack.Add(reader.ReadLine());

            } while (!reader.EndOfStream);

            track = new char[sTrack[0].Length, sTrack.Count];
            TrackW = sTrack[0].Length;
            TrackH = sTrack.Count;
            TrackWM1 = TrackH - 1;
            TrackHM1 = TrackH - 1;

            int tIdx = 0;
            foreach (string railline in sTrack)
            {
                int index = 0;
                foreach (char c in railline)
                {
                    //Assuming carts start on open road
                    if (c == '>')
                    {
                        carts.Add(new sCar(index, tIdx, eDirection.RIGHT));
                        track[index++, tIdx] = '-';
                    }
                    else if (c == '<')
                    {
                        carts.Add(new sCar(index, tIdx, eDirection.LEFT));
                        track[index++, tIdx] = '-';
                    }
                    else if (c == '^')
                    {
                        carts.Add(new sCar(index, tIdx, eDirection.UP));
                        track[index++, tIdx] = '|';
                    }
                    else if (c == 'v')
                    {
                        carts.Add(new sCar(index, tIdx, eDirection.DOWN));
                        track[index++, tIdx] = '|';
                    }
                    else
                        track[index++, tIdx] = c;
                }

                tIdx++;
            }

            //Note: Since we went top down and left right carts start in sorted order
        }

        public static string Function1()
        {
            int curCart = 0; int collisionIdx;
            bool bWreck = false;
            ReadFile();

            //Lets simulate a wreck
            while (!bWreck)
            {
                curCart = 0;
                for (curCart = 0; curCart < carts.Count; curCart++)
                {
                    //Move the cart
                    carts[curCart].Move();                    
                    if (bWreck = WasCollision(curCart, out collisionIdx)) break;
                }

                if (bWreck) break;

                //Resort (poor man sort)
                for (int i = 1; i < carts.Count; i++)
                {
                    for (int inner = 0; inner < carts.Count; inner++)
                    {
                        //loop to itself
                        if (i == inner) break;
                        //Find first cart that is higher and insert
                        if (carts[i].IsSooner(carts[inner]))
                        {
                            sCar Temp = carts[i];
                            carts[i] = carts[inner];
                            carts[inner] = Temp;
                        }
                    }
                }
            }

            return string.Format("{0},{1}",carts[curCart].x, carts[curCart].y);
        }

        private static bool WasCollision(int cartIdx, out int colIdx)
        {
            colIdx = 0;
            for (int i = 0; i < carts.Count; i++)
            {
                if (cartIdx == i || carts[i].wrecked) continue;

                if (carts[cartIdx].x == carts[i].x &&
                    carts[cartIdx].y == carts[i].y)
                {
                    colIdx = i;
                    return true;
                }
            }
            return false;
        }

        public static string Function2()
        {
            int curCart = 0; int collisionIdx;
            bool bWreck = false;
            ReadFile();

            //Lets simulate a wreck
            while (carts.Count > 1)
            {
                curCart = 0;
                bWreck = false;
                for (curCart = 0; curCart < carts.Count; curCart++)
                {
                    if (carts[curCart].wrecked) continue;
                    //Move the cart
                    carts[curCart].Move();
                    if (WasCollision(curCart, out collisionIdx))
                    {
                        carts[curCart].wrecked = true;
                        carts[collisionIdx].wrecked = true;
                        bWreck = true;
                    }
                }

                if (bWreck)
                {
                    //Remove Wrecked carts
                    foreach (sCar cart in carts.ToArray())
                        if (cart.wrecked)
                            carts.Remove(cart);
                }

                //Resort (poor man sort)
                for (int i = 1; i < carts.Count; i++)
                {
                    for (int inner = 0; inner < carts.Count; inner++)
                    {
                        //loop to itself
                        if (i == inner) break;
                        //Find first cart that is higher and insert
                        if (carts[i].IsSooner(carts[inner]))
                        {
                            sCar Temp = carts[i];
                            carts[i] = carts[inner];
                            carts[inner] = Temp;
                        }
                    }
                }
            }

            return string.Format("{0},{1}", carts[0].x, carts[0].y);
        }
    }
}
