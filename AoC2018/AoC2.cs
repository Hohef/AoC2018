using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC2
    {
        public static int Function1()
        {
            int two = 0, three = 0;
            string line;
            List<string> barcodes = new List<string>();
            System.IO.StreamReader input = new StreamReader(@"..\..\Inputs\Input2.txt");
            //Read in barcodes
            while ((line = input.ReadLine()) != null)
                barcodes.Add(line);

            foreach (string barcode in barcodes)
            {
                bool temp2 = false, temp3 = false;
                uint[] array = new uint[26];
                foreach (char c in barcode)
                    array[c - 'a']++;
                foreach (int i in array )
                {
                    if (i == 2) temp2 = true;
                    else if (i == 3) temp3 = true;
                }

                if (temp2) two++;
                if (temp3) three++;
            }

            return two * three;
        }

        public static string Function2()
        {
            int pos;
            string line;
            List<string> barcodes = new List<string>();
            System.IO.StreamReader input = new StreamReader(@"..\..\Inputs\Input2.txt");
            //Read in barcodes
            while ((line = input.ReadLine()) != null)
                barcodes.Add(line);

            for (int i = 0; i < barcodes.Count-1; i++)
            {
                for (int j = i + 1; j < barcodes.Count; j++)
                    if (Diff(barcodes[i], barcodes[j], out pos))
                    {
                        string rtnval = barcodes[i];
                        return rtnval.Remove(pos, 1);
                    }
            }

            return "";
        }

        private static bool Diff(string id1, string id2, out int position)
        {
            position = -1;
            if (id1.Length != id2.Length) return false;
            for (int i = 0; i < id1.Length; i++)
            {
                if (id1[i] != id2[i])
                {
                    if (position == -1)
                        position = i;
                    else
                        return false;
                }
            }
            return true;
        }
    }
}
