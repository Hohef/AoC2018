using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC12
    {
        const int PLANTCYCLES = 20;
        const int ADDITIONALPOTS = (PLANTCYCLES + 1000) * 2;
        const int POTSTARTIDX = ADDITIONALPOTS / 2;
        const int CYCLESTARTIDX = POTSTARTIDX - 4;

        struct sGreenThumb
        {
            public int state;
            public bool bContainsPlant;
            public bool bPending;
            private bool bPendingSet;
            public bool ContainsPlant() { return (state & 0b00100) > 0; }

            public bool RuleApplies(int rule)
            {
                return (state == rule);
            }

            public bool ApplyRule(bool set)
            {
                int pendingState;
                bPendingSet = set;

                if (set)
                    pendingState = state | 0b00100;
                else
                    pendingState = state & 0b11011;

                bPending = (pendingState != state);
                return set;
            }

            /// <summary>
            /// Applies Rule and returns if plant state changed
            /// </summary>
            /// <param name="rule"></param>
            /// <returns></returns>
            public bool ApplyPendingRule()
            {
                bPending = false;

                if (bPendingSet)
                    state |= 0b00100;
                else
                    state &= 0b11011;
                
                bContainsPlant = (state & 0b00100) > 0;
                return bContainsPlant;
            }

            public void UpdateSlot(int slot, bool set)
            {
                if (set)
                {
                    state |= 0x1 << slot;
                    bContainsPlant = true;
                }
                else
                {
                    state &= ~(0x1 << slot);
                    bContainsPlant = false;
                }
            }
        }

        //static List<sGreenThumb> plants;
        static sGreenThumb[] pots;
        static List<int> potRules = new List<int>();
        static long iInitialPots;


        private static void ReadTree()
        {
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input12b.txt");
            int rule = 0;
            do
            {
                string line = reader.ReadLine().Substring(3);
                foreach (char c in line)
                {
                    //rule = rule << 1;
                    if (c == '#')
                        rule++;
                }
            } while (!reader.EndOfStream);
        }


        private static void ReadFile()
        {
            ReadTree();
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input12.txt");
            //Read initial state
            string initline = reader.ReadLine().Remove(0,15);
            //Empty Line
            reader.ReadLine();

            //Initialize the plants
            iInitialPots = 0;
            pots = new sGreenThumb[initline.Length + ADDITIONALPOTS];
            int potIdx = POTSTARTIDX;
            foreach (char plantstate in initline)
            {
                if (plantstate == '#')
                {                    
                    pots[potIdx-2].UpdateSlot(0, true);
                    pots[potIdx-1].UpdateSlot(1, true);
                    pots[potIdx].UpdateSlot(2,true); //current slot
                    pots[potIdx+1].UpdateSlot(3, true);
                    pots[potIdx+2].UpdateSlot(4, true);
                    iInitialPots += potIdx;
                }
                potIdx++;
            }

            do
            {
                string line = reader.ReadLine();
                char plant = line[line.Length - 1];
                if (plant == '#')
                {
                    line = line.Substring(0, 5);
                    int rule = 0;
                    foreach (char c in line)
                    {
                        rule = rule << 1;
                        if (c == '#')
                            rule++;
                    }

                    potRules.Add(rule);
                }
            } while (!reader.EndOfStream);
        }


        private static bool RuleFound(int index)
        {
            foreach (int rule in potRules)
                if (pots[index].RuleApplies(rule))
                    return true;

            return false;
        }



        public static int Function1()
        {
            bool hasplant;
            ReadFile();
            //This will be infinite if '.....' makes a plant so ruling that case out.
            //starting at reasonable start of 4 back to cover the '....#' condition
            int startpotIdx = CYCLESTARTIDX;
            int stoppotIdx = pots.Length - POTSTARTIDX + 4;
            for (int i = 0; i < PLANTCYCLES; i++)
            {
                for (int potIdx = startpotIdx; potIdx < stoppotIdx; potIdx++)
                    pots[potIdx].ApplyRule(RuleFound(potIdx));

                for (int potIdx = startpotIdx; potIdx < stoppotIdx; potIdx++)
                {  
                    if (pots[potIdx].bPending)
                    {
                        hasplant = pots[potIdx].ApplyPendingRule();
                        pots[potIdx - 2].UpdateSlot(0, hasplant);
                        pots[potIdx - 1].UpdateSlot(1, hasplant);
                        pots[potIdx + 1].UpdateSlot(3, hasplant);
                        pots[potIdx + 2].UpdateSlot(4, hasplant);
                    }
                }

                startpotIdx--;
                stoppotIdx++;
            }

            //Sum up the Pot Numbers
            int plantsgrown = 0;
            for (int i = 0; i < pots.Length;i++)
            {
                if (pots[i].ContainsPlant())
                {
                    plantsgrown += (i - POTSTARTIDX);
                }
            }

            return plantsgrown;
        }

        private static void Print(int startpotIdx, int stoppotIdx)
        {
            return;
            string s = "";
            for (int potIdx = startpotIdx; potIdx < stoppotIdx; potIdx++)
            {
                s += pots[potIdx].state + ",";
            }
            System.Diagnostics.Debug.Print(s);
        }

        /// <summary>
        /// Initial idea was to find where a pattern repeats, but didn't think one
        /// would exist.  This solution is based upon other people's ideas.
        /// </summary>
        /// <returns></returns>
        public static long Function2()
        {
            bool hasplant;
            ReadFile();
            //This will be infinite if '.....' makes a plant so ruling that case out.
            //starting at reasonable start of 4 back to cover the '....#' condition
            int startpotIdx = CYCLESTARTIDX;
            int stoppotIdx = pots.Length - POTSTARTIDX + 4;

            long scoregen1 = iInitialPots;
            long scoregen2 = 0;
            long scoregen3 = 0;
            long plantsgrown = 0;

            for (int i = 0; i < 1000; i++)
            {
                for (int potIdx = startpotIdx; potIdx < stoppotIdx; potIdx++)
                    pots[potIdx].ApplyRule(RuleFound(potIdx));

                for (int potIdx = startpotIdx; potIdx < stoppotIdx; potIdx++)
                {
                    if (pots[potIdx].bPending)
                    {
                        hasplant = pots[potIdx].ApplyPendingRule();
                        pots[potIdx - 2].UpdateSlot(0, hasplant);
                        pots[potIdx - 1].UpdateSlot(1, hasplant);
                        pots[potIdx + 1].UpdateSlot(3, hasplant);
                        pots[potIdx + 2].UpdateSlot(4, hasplant);
                    }
                }

                startpotIdx--;
                stoppotIdx++;

                scoregen3 = scoregen2;
                scoregen2 = scoregen1;
                scoregen1 = plantsgrown;

                //Sum up the Pot Numbers
                plantsgrown = 0;
                for (int idx = 0; idx < pots.Length; idx++)
                {
                    if (pots[idx].ContainsPlant())
                    {
                        plantsgrown += (idx - POTSTARTIDX);
                    }
                }

                if (i > 10)
                {
                    //Looking for constant increase rate
                    if (((plantsgrown - scoregen1) == (scoregen1 - scoregen2)) &&
                        ((plantsgrown - scoregen1) == (scoregen2 - scoregen3)))
                    {
                        
                        plantsgrown = plantsgrown + (plantsgrown - scoregen1)*(50000000000 - (i+1));
                        break;
                    }
                }               
            }

            return plantsgrown;
        }
    }
}
