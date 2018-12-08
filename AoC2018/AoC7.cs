using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC7
    {
        const int NUMWORKERS = 5;
        const int SECPERUNIT = 61;

        struct sInstruction
        {
            public string step;
            public string stepPrior;

            public sInstruction(string cX, string cY) { step = cX; stepPrior = cY; }
        }

        static char[] delim = { ',', ' ' };
        static List<sInstruction> instructions = new List<sInstruction>(50);

        static int[] numPrereq = new int[26];
        static bool[] bStepSeen = new bool[26];
        static List<int>[] prereqFor = new List<int>[26];

        private static void ReadFile()
        {
            numPrereq.Initialize();
            for (int i = 0; i < 26; i++)
                prereqFor[i] = new List<int>();

            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input7.txt");
            do
            {
                string[] line = reader.ReadLine().Split(' ');
                int stepA = char.Parse(line[1]) - 'A';
                int stepB = char.Parse(line[7]) - 'A';
                bStepSeen[stepA] = true;
                bStepSeen[stepB] = true;

                numPrereq[stepB]++;
                prereqFor[stepA].Add(stepB);
            } while (!reader.EndOfStream);
        }

        public static string Function1()
        {
            ReadFile();
            string order = "";
            bool proccess = false;
            do
            {
                proccess = false;
                for (int i = 0; i < 26; i++)
                {
                    if (numPrereq[i] == 0)
                    {
                        order += char.ToString((char)(i + 'A'));
                        foreach (int step in prereqFor[i])
                        {
                            numPrereq[step]--;
                        }
                        numPrereq[i] = -1;
                        proccess = true;
                        break;
                    }                    
                }

            } while (order.Length < 26 && proccess);

            return order;
        }

        struct sWorker
        {
            private int _step;
            public int step
            {
                get { return _step; }
                set { _step = value; workLeft = SECPERUNIT + value; }
            }
            public int workLeft;

            public int PerformWork() { if (workLeft == 0) return -1; return --workLeft; }
        }

        public static int Function2()
        {
            ReadFile();
            int totalSeconds = -1;
            bool process;
            sWorker[] workers = new sWorker[NUMWORKERS];
            int availworker = 0;
            do
            {
                process = false;
                for (int i = 0; i < 26; i++)
                {
                    //Only process if available workder
                    if ((availworker = NextAvailWorker(workers)) == -1) break;

                    if (numPrereq[i] == 0 && bStepSeen[i])
                    {
                        workers[availworker].step = i;
                        numPrereq[i] = -1;
                    }
                }

                //Perform any work on Sleigh
                for (int i = 0; i < workers.Length; i++)
                {
                    int sec = workers[i].PerformWork();
                    if (sec == 0)
                    {
                        //order += char.ToString((char)(i + 'A'));
                        foreach (int step in prereqFor[workers[i].step])
                            numPrereq[step]--;
                        process = true;
                    }
                    else if (sec > 0) process = true;  //Elf's working
                }

                totalSeconds++;
            } while (process);



            return totalSeconds;
        }

        private static int NextAvailWorker(sWorker[] workers)
        {
            for (int i = 0; i < workers.Length;i++)
            {
                if (workers[i].workLeft == 0) return i;
            }

            return -1;
        }
    }
}
