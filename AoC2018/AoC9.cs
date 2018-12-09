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


        static private void RevStack(ref Stack<int> stack1, ref Stack<int> stack2)
        {
            ref Stack<int> temp = ref stack2;
            ref Stack<int> temp2 = ref stack1;


            stack1 = temp;
            stack2 = temp2;
        }

        class CStack
        {
            public Stack<int>[] stacks = new Stack<int>[2];
            private int leftIdx = 0;
            private int rightIdx = 1;

            public CStack()
            {
                stacks[0] = new Stack<int>();
                stacks[1] = new Stack<int>();
            }

            public int position = 0;
            
            public void ReverseIfNullRight()
            {
                if (stacks[rightIdx].Count == 0)
                    Reverse();
            }

            public void ReverseIfNullLeft()
            {
                if (stacks[leftIdx].Count == 0)
                    Reverse();
            }

            public void Reverse()
            {
                int tempIdx = leftIdx;
                leftIdx = rightIdx;
                rightIdx = tempIdx;

                if (stacks[0].Count != 0)
                    stacks[0].Reverse();
                else
                    stacks[1].Reverse();
            }

            public bool CheckNullRight()
            {
                return stacks[rightIdx].Count == 0;
            }

            public void MoveLeft()
            {
                stacks[leftIdx].Push(stacks[rightIdx].Pop());
            }

            public void MoveRight()
            {
                stacks[rightIdx].Push(stacks[leftIdx].Pop());
            }

            public void MovePositionLeft()
            {
                stacks[leftIdx].Push(position);
            }

            public void MovePositionRight()
            {
                stacks[rightIdx].Push(position);
            }

            public void SetPositionLeft()
            {
                position = stacks[leftIdx].Pop();
            }

            public void SetPositionRight()
            {
                position = stacks[rightIdx].Pop();
            }
        }


        static public long Function3L()
        {
            CStack myStack = new CStack();
            ReadFile();
            //NUMMARBLES *= 100;
            int elfIdx = -1;
            long[] elfScore = new long[NUMELVES];

            for (int marble = 1; marble < NUMMARBLES; marble++)
            {
                elfIdx = (elfIdx + 1) % NUMELVES;

                if (marble %23 != 0)
                {
                    myStack.MovePositionLeft();
                    myStack.ReverseIfNullRight();
                    myStack.MoveLeft();
                    myStack.position = marble;
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        myStack.ReverseIfNullLeft();
                        myStack.MovePositionRight();
                        myStack.SetPositionLeft();
                    }

                    elfScore[elfIdx] += marble;
                    elfScore[elfIdx] += myStack.position;
                    myStack.ReverseIfNullRight();
                    myStack.SetPositionRight();
                }
            }

            long highscore = 0;
            for (int i = 0; i < NUMELVES; i++)
                if (elfScore[i] > highscore) highscore = elfScore[i];

            return highscore;
        }
    }
}
