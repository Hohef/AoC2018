using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC5
    {
        private static Stack<char> polymer = new Stack<char>();

        private static bool IsAlchemicalMatch(char a, char b)
        {
            //Is upper/lower match (ie. a/A, but not A/A)
            return (Math.Abs(a - b) == 32);
        }

        private static void PolymerRead()
        {
            char unit;
            //Read in shift awake times
            System.IO.StreamReader input = new StreamReader(@"..\..\Inputs\Input5.txt");
            do
            {
                unit = (char)input.Read();

                if (polymer.Count == 0)
                    polymer.Push(unit);
                else
                {
                    char prev = polymer.Peek();
                    //Remove upper/lower match (ie. a/A, but not A/A)
                    if (!IsAlchemicalMatch(prev, unit))
                        polymer.Push(unit);
                    else
                        polymer.Pop();
                }
            }
            while (!input.EndOfStream);
        }

        private static int TryPolymer(string molecule, char lcremove)
        {
            Stack<char> polymer1 = new Stack<char>();
            foreach(char unit in molecule)
            {
                if (char.ToLower(unit) == lcremove) continue;

                if (polymer1.Count == 0)
                    polymer1.Push(unit);
                else
                {
                    char prev = polymer1.Peek();
                    if (!IsAlchemicalMatch(prev, unit))
                        polymer1.Push(unit);
                    else
                        polymer1.Pop();
                }
            }

            return polymer1.Count;
        }


        public static int Function1()
        {
            PolymerRead();
            return polymer.Count;
        }

        public static int Function2()
        {
            string line;
            System.IO.StreamReader input = new StreamReader(@"..\..\Inputs\Input5.txt");
            line = input.ReadToEnd();

            int smallest = -1;
            //Try Polymer's a-Z
            for (char c = 'a'; c <= 'z'; c++)
            {
                int count = TryPolymer(line,c);
                if (smallest == -1 || count < smallest)
                    smallest = count;
            }

            return smallest;
        }
    }
}
