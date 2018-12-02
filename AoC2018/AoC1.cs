using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC1
    {
        public static int Function1()
        {
            object[] result = new object[2];
            string line;
            bool found = false;
            List<int> freq = new List<int>(1000);
            List<int> seen = new List<int>(1000);
            int frequency = 0;

            //Read in the frequencies
            System.IO.StreamReader input = new StreamReader(@"..\..\Inputs\Input1.txt");
            do
            {
                line = input.ReadLine();
                if (line != null)
                    freq.Add(int.Parse(line));
                frequency += freq.Last();
            } while (line != null);
            result[0] = frequency;


            //Determine duplicate
            seen.Add(0);  //Seed starting frequency
            frequency = 0; //reset counter
            while (!found)
            {
                for (int i = 0; i < freq.Count; i++)
                {
                    frequency += freq[i];
                    if (seen.Contains(frequency))
                    {
                        result[1] = frequency;
                        found = true;
                        break;
                    }
                    seen.Add(frequency);
                }
            }

            return (int)result[1];
        }
    }
}
