using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    struct sShift
    {
        public int id;
        public int month, day;
        public int hh, mm;
        public bool awake;
    }

    class sAWOL
    {
        public int TotalAsleep;
        public int Minute;
        public int[] Asleep = new int[60];
    }

    class AoC4
    {
        static char[] delim = {' ', '-', ':'};
        static Dictionary<int, sAWOL> elfTimes = new Dictionary<int, sAWOL>();

        private static void Punction()
        {
            string line;
            //Start date is 1518 - 12 - 25 and works backwards (problem is only few months);
            SortedList<int, sShift>[] shifts = new SortedList<int, sShift>[400];

            //Read in shift awake times
            System.IO.StreamReader input = new StreamReader(@"..\..\Inputs\Input4.txt");
            while ((line = input.ReadLine()) != null)
            {
                sShift elfShift = new sShift();

                string[] data = line.Split(']');
                string[] info = data[1].Split(' ');
                string[] date = data[0].Split(delim);
                elfShift.month = int.Parse(date[1]);
                elfShift.day = int.Parse(date[2]);
                elfShift.hh = int.Parse(date[3]);
                elfShift.mm = int.Parse(date[4]);
                if (data[1].StartsWith(" Guard"))
                    elfShift.id = int.Parse(info[2].Substring(1));
                else
                    elfShift.awake = data[1].StartsWith(" w");

                int index = (12 - elfShift.month) * 31 + 25 - elfShift.day;
                int sortIdx = elfShift.mm;
                //Account for prior to midnight night shift
                if (elfShift.hh != 0)
                {
                    index--;
                    sortIdx = (2400 - ((elfShift.hh * 100) + elfShift.mm)) * -1;
                }

                if (shifts[index] == null)
                    shifts[index] = new SortedList<int, sShift>();

                shifts[index].Add(sortIdx, elfShift);
            }

            //Combine Guard asleep time and total time
            
            sAWOL asleep;
            int startsleep = -1;
            foreach (SortedList<int, sShift> shift in shifts)
            {
                if (shift == null) continue;

                if (!elfTimes.TryGetValue(shift.Values[0].id, out asleep))
                {
                    asleep = new sAWOL();
                    elfTimes.Add(shift.Values[0].id, asleep);
                }

                //Assuming start[sleep/wake]
                for (int i = 1; i < shift.Count; i++)
                {
                    //discard non midnight (also shouldn't need check)
                    if (shift.Values[i].hh != 0) continue;

                    if (!shift.Values[i].awake) startsleep = shift.Values[i].mm;
                    else if (startsleep == -1)
                        throw new FormatException();
                    else
                    {
                        for (int minute = startsleep; minute < shift.Values[i].mm; minute++)
                        {
                            asleep.TotalAsleep++;
                            asleep.Asleep[minute]++;
                        }
                    }
                }

                //Shouldn't need, but just in case guard shift ends with him sleeping
                if (shift.Count > 1 && !shift.Values[shift.Count - 1].awake)
                {
                    for (int minute = startsleep; minute < 60; minute++)
                    {
                        asleep.TotalAsleep++;
                        asleep.Asleep[minute]++;
                    }
                }
            }
        }


        public static int Function1()
        {
            int rtnvalue = 0;
            int mostsleep = -1;

            Punction();

            //Determine which guard was alseep the longest and what minute
            //most likely asleep
            foreach (KeyValuePair<int, sAWOL> guard in elfTimes)
            {
                int mostMinute = guard.Value.Asleep[0];
                for (int minute = 1; minute < 60; minute++)
                {
                    if (guard.Value.Asleep[minute] > mostMinute)
                    {
                        guard.Value.Minute = minute;
                        mostMinute = guard.Value.Asleep[minute];
                    }
                }

                if (guard.Value.TotalAsleep > mostsleep)
                {
                    mostsleep = guard.Value.TotalAsleep;
                    rtnvalue = guard.Key * guard.Value.Minute;
                }
            }

            return rtnvalue;
        }

        public static int Function2()
        {
            int rtnvalue = 0;
            int mostMinute = -1;

            Punction();

            //Loop thru and find which guard sleep the most at any give minute
            foreach (KeyValuePair<int, sAWOL> guard in elfTimes)
            {
                for (int minute = 0; minute < 60; minute++)
                {
                    if (guard.Value.Asleep[minute] > mostMinute)
                    {
                        guard.Value.Minute = minute;
                        mostMinute = guard.Value.Asleep[minute];
                        rtnvalue = guard.Key * minute;
                    }
                }
            }

            return rtnvalue;
        }
    }
}
