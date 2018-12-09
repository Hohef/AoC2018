using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC9
    {
        static int NUMELVES;
        static int NUMMARBLES;

        private static void ReadFile()
        {
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input9.txt");
            string [] line = reader.ReadLine().Split(' ');
            NUMELVES = int.Parse(line[0]);
            NUMMARBLES = int.Parse(line[6]);
        }

        public static int Function1()
        {
            ReadFile();

            int[] elfScore = new int[NUMELVES];
            int elfIdx = -1;
            List<int> board = new List<int>();
            board.Add(0);
            int boardPosition = 0;
            for (int marble = 1; marble < NUMMARBLES; marble++)
            {
                elfIdx = (elfIdx + 1) % NUMELVES;

                //Remove marbles divisable by 23 and 7 back
                if (marble % 23 == 0)
                {
                    boardPosition -= 7;
                    if (boardPosition < 0) boardPosition += board.Count;
                    elfScore[elfIdx] += marble;
                    elfScore[elfIdx] += board[boardPosition];
                    board.RemoveAt(boardPosition);
                    continue;
                }

                boardPosition = (boardPosition + 2);
                if (boardPosition > board.Count)
                    boardPosition -= board.Count;
                
                if (boardPosition < board.Count)
                    board.Insert(boardPosition, marble);
                else
                    board.Add(marble);                
            }

            int highscore = 0;
            for (int i = 0; i < NUMELVES; i++)
                if (elfScore[i] > highscore) highscore = elfScore[i];

            return highscore;
        }


        public static long Function2()
        {
            ReadFile();
            NUMMARBLES *= 100;
            LinkedListNode<int>[] nodes = new LinkedListNode<int>[NUMMARBLES];

            long[] elfScore = new long[NUMELVES];
            int elfIdx = -1;
            LinkedList<int> board = new LinkedList<int>();
            nodes[0] = board.AddLast(0);

            LinkedListNode<int> currentNode = nodes[0];
            for (int marble = 1; marble < NUMMARBLES; marble++)
            {
                elfIdx = (elfIdx + 1) % NUMELVES;

                //Remove marbles divisable by 23 and 7 back
                if (marble % 23 == 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (currentNode == board.First)
                            currentNode = board.Last;
                        else
                            currentNode = currentNode.Previous;
                    }

                    elfScore[elfIdx] += marble;
                    elfScore[elfIdx] += currentNode.Value;
                    LinkedListNode<int> removeNode = currentNode;
                    currentNode = currentNode.Next;
                    board.Remove(removeNode);
                    continue;
                }

                //Move Clockwise by 1 node
                if (currentNode == board.Last)
                    currentNode = board.First;
                else
                    currentNode = currentNode.Next;

                currentNode = board.AddAfter(currentNode,marble);
            }

            long highscore = 0;
            for (int i = 0; i < NUMELVES; i++)
                if (elfScore[i] > highscore) highscore = elfScore[i];

            return highscore;
        }
    }
}
