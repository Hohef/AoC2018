using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC16
    {
        class CProcessor
        {
            public int[] register1 = new int[4];
            public int[] instruction = new int[4];
            public int[] register2 = new int[4];
        }

        static List<CProcessor> cpu = new List<CProcessor>();
        static List<CProcessor> cpu2 = new List<CProcessor>();
        private static void ReadFile()
        {
            string line;
            for (int i = 0; i < opValF.Length; i++)
                opValF[i] = new CWhoIs();

            cpu.Clear();
            cpu2.Clear();
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input16.txt");
            //Read First Part of Input
            while ((line = reader.ReadLine()) != "")
            {
                CProcessor cpuinstruction = new CProcessor();
                string[] split = line.Substring(9).Split(new char[] { ',', ' ', ']' }, StringSplitOptions.RemoveEmptyEntries);
                cpuinstruction.register1[0] = int.Parse(split[0]);
                cpuinstruction.register1[1] = int.Parse(split[1]);
                cpuinstruction.register1[2] = int.Parse(split[2]);
                cpuinstruction.register1[3] = int.Parse(split[3]);

                split = reader.ReadLine().Split(' ');
                cpuinstruction.instruction[0] = int.Parse(split[0]);
                cpuinstruction.instruction[1] = int.Parse(split[1]);
                cpuinstruction.instruction[2] = int.Parse(split[2]);
                cpuinstruction.instruction[3] = int.Parse(split[3]);

                split = reader.ReadLine().Substring(9).Split(new char[] { ',', ' ', ']' }, StringSplitOptions.RemoveEmptyEntries);
                cpuinstruction.register2[0] = int.Parse(split[0]);
                cpuinstruction.register2[1] = int.Parse(split[1]);
                cpuinstruction.register2[2] = int.Parse(split[2]);
                cpuinstruction.register2[3] = int.Parse(split[3]);

                cpu.Add(cpuinstruction);

                //Remove blank line
                reader.ReadLine();
            }

            reader.ReadLine();

            while(!reader.EndOfStream)
            {
                CProcessor cpuinstruction = new CProcessor();
                string[] split = reader.ReadLine().Split(' ');
                cpuinstruction.instruction[0] = int.Parse(split[0]);
                cpuinstruction.instruction[1] = int.Parse(split[1]);
                cpuinstruction.instruction[2] = int.Parse(split[2]);
                cpuinstruction.instruction[3] = int.Parse(split[3]);
                cpu2.Add(cpuinstruction);
            }

            for (int i = 0; i < 16; i++)
                otherway[i] = new List<int>();
        }

        enum eOPCODE { addr,addi,mulr, muli, banr,bani, borr, bori,setr,seti,gtir,gtri,gtrr,eqir,eqri,eqrr};

        private static int OpcodeExecute(int[] instruct, int [] regi)
        {
            switch ((eOPCODE)instruct[0])
            {
                case eOPCODE.addr: return regi[instruct[1]] + regi[instruct[2]];
                case eOPCODE.addi: return regi[instruct[1]] + instruct[2];
                case eOPCODE.mulr: return regi[instruct[1]] * regi[instruct[2]];
                case eOPCODE.muli: return regi[instruct[1]] * instruct[2];
                case eOPCODE.banr: return regi[instruct[1]] & regi[instruct[2]];
                case eOPCODE.bani: return regi[instruct[1]] & instruct[2];
                case eOPCODE.borr: return regi[instruct[1]] | regi[instruct[2]];
                case eOPCODE.bori: return regi[instruct[1]] | instruct[2];
                case eOPCODE.setr: return regi[instruct[1]];
                case eOPCODE.seti: return instruct[1];
                case eOPCODE.gtir: return (instruct[1] > regi[instruct[2]]) ? 1 : 0;
                case eOPCODE.gtri: return (regi[instruct[1]] > instruct[2]) ? 1 : 0;
                case eOPCODE.gtrr: return (regi[instruct[1]] > regi[instruct[2]]) ? 1 : 0;
                case eOPCODE.eqir: return (instruct[1] == regi[instruct[2]]) ? 1 : 0;
                case eOPCODE.eqri: return (regi[instruct[1]] == instruct[2]) ? 1 : 0;
                case eOPCODE.eqrr: return (regi[instruct[1]] == regi[instruct[2]]) ? 1 : 0;
                default: return -1;
            }
        }

        class CWhoIs
        {
            bool found = false;
            List<int> codes = new List<int>();

            public void Add(int [] opcodes)
            {
                if (found) return;
                if (opcodes.Length == 1)
                {
                    codes.Clear();
                    codes.Add(opcodes[0]);
                }
                if (codes.Count == 0)
                    codes.AddRange(opcodes.ToArray());
                else
                {
                    //Remove all in list not in second
                    foreach (int code in codes.ToArray())
                        if (!opcodes.Contains(code))
                            codes.Remove(code);
                }
            }

            public bool Remove(int opcode)
            {
                if (found) return false;
                codes.Remove(opcode);
                if (codes.Count == 1)
                {
                    //found = true;
                    //return true;
                }
                return false;
            }

            public bool Check()
            {
                if (!found)
                    if (codes.Count == 1)
                    {
                        found = true;
                        return true;
                    }

                return false;
            }

            public void Matched(int opcode)
            {
                codes.Clear();
                codes.Add(opcode);
            }

            public bool Found() { return found; }
        }

        static CWhoIs[] opValF = new CWhoIs[16];

        static eOPCODE[] opValue = new eOPCODE[16];
        static bool[] bopValue = new bool[16];
        static List<int>[] otherway = new List<int>[16];

        private static int MatchedOpCode(int [] regi,int[] instruct, int [] rego)
        {
            List<int> matchopcodes = new List<int>();
            int opcode = instruct[0];

            int matched = 0;
            //addr RegisterA + RegisterB
            if ((regi[instruct[1]] + regi[instruct[2]]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(0);
                if (!otherway[0].Contains(opcode))
                    otherway[0].Add(opcode);
            }
            //addi RegisterA + valueB
            if ((regi[instruct[1]] + instruct[2]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(1);
                if (!otherway[1].Contains(opcode))
                    otherway[1].Add(opcode);
            }
            //mulr RegisterA * RegisterB
            if ((regi[instruct[1]] * regi[instruct[2]]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(2);
                if (!otherway[2].Contains(opcode))
                    otherway[2].Add(opcode);
            }
            //muli RegisterA * valueB
            if ((regi[instruct[1]] * instruct[2]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(3);
                if (!otherway[3].Contains(opcode))
                    otherway[3].Add(opcode);
            }
            //banr RegisterA | RegisterB
            if ((regi[instruct[1]] & regi[instruct[2]]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(4);
                if (!otherway[4].Contains(opcode))
                    otherway[4].Add(opcode);
            }
            //bani RegisterA | valueB
            if ((regi[instruct[1]] & instruct[2]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(5);
                if (!otherway[5].Contains(opcode))
                    otherway[5].Add(opcode);
            }
            //borr RegisterA | RegisterB
            if ((regi[instruct[1]] | regi[instruct[2]]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(6);
                if (!otherway[6].Contains(opcode))
                    otherway[6].Add(opcode);
            }
            //bori RegisterA | valueB
            if ((regi[instruct[1]] | instruct[2]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(7);
                if (!otherway[7].Contains(opcode))
                    otherway[7].Add(opcode);
            }
            //setr RegisterB = RegisterC
            if ((regi[instruct[1]]) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(8);
                if (!otherway[8].Contains(opcode))
                    otherway[8].Add(opcode);
            }
            //seti RegisterB = valueC
            if (instruct[1] == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(9);
                if (!otherway[9].Contains(opcode))
                    otherway[9].Add(opcode);
            }
            //gtir valueA > RegisterB
            if (((instruct[1] > regi[instruct[2]]) ? 1 : 0) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(10);
                if (!otherway[10].Contains(opcode))
                    otherway[10].Add(opcode);
            }
            //gtri RegisterA > ValueB
            if (((regi[instruct[1]] > instruct[2]) ? 1 : 0) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(11);
                if (!otherway[11].Contains(opcode))
                    otherway[11].Add(opcode);
            }
            //gtrr RegisterA > RegisterB
            if (((regi[instruct[1]] > regi[instruct[2]]) ? 1 : 0) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(12);
                if (!otherway[12].Contains(opcode))
                    otherway[12].Add(opcode);
            }
            //eqir valueA == RegisterB
            if (((instruct[1] == regi[instruct[2]]) ? 1 : 0) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(13);
                if (!otherway[13].Contains(opcode))
                    otherway[13].Add(opcode);
            }
            //eqri RegisterA == valueB
            if (((regi[instruct[1]] == instruct[2]) ? 1 : 0) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(14);
                if (!otherway[14].Contains(opcode))
                    otherway[14].Add(opcode);
            }
            //eqrr RegisterA == RegisterB
            if (((regi[instruct[1]] == regi[instruct[2]]) ? 1 : 0) == rego[instruct[3]])
            {
                matched++;
                matchopcodes.Add(15);
                if (!otherway[15].Contains(opcode))
                    otherway[15].Add(opcode);
            }


           
            if (!bopValue[opcode])
            {
                opValF[opcode].Add(matchopcodes.ToArray());

                bool run = true;
                while (run)
                {
                    run = false;
                    for (int i = 0; i < opValF.Length; i++)
                    {
                        if (opValF[i].Check())
                        {
                            bopValue[i] = true;
                            for (int j = 0; j < opValF.Length; j++)
                                if (i != j)
                                    opValF[j].Remove(i);
                            run = true;
                        }
                    }
                }
            }

            return matched;
        }


        public static int Function1()
        {
            ReadFile();
            int match3 = 0;

            foreach (CProcessor instruction in cpu)
                if (MatchedOpCode(instruction.register1, instruction.instruction, instruction.register2) >= 3) match3++;

            return match3;
        }

        static bool[] bob = new bool[16];
        public static int Function2()
        {
            ReadFile();
            int match3 = 0;

            foreach (CProcessor instruction in cpu)
                if (MatchedOpCode(instruction.register1, instruction.instruction, instruction.register2) >= 3) match3++;

            for (int i = 0; i < 16; i++)
                bob[i] = false;
            //Remove codes 
            bool restart = false;
            do
            {
                restart = false;
                for (int i = 0; i < 16; i++)
                {
                    if (otherway[i].Count == 1 && !bob[i])
                    {
                        op2func[otherway[i][0]] = i;
                        restart = true;
                        bob[i] = true;
                        for (int j = 0; j < 16; j++)
                        {
                            if (i != j)
                                otherway[j].Remove(otherway[i][0]);
                        }
                    }
                }

            } while (restart);


            //Convert Function to OpCode


            int[] registerA = new int[4];
            //registerA[0] = cpu.Last().register2[0];
            //registerA[1] = cpu.Last().register2[1];
            //registerA[2] = cpu.Last().register2[2];
            //registerA[3] = cpu.Last().register2[3];


            foreach (CProcessor instruction in cpu2)
            {
                instruction.instruction[0] = op2func[instruction.instruction[0]];
                registerA[instruction.instruction[3]] = OpcodeExecute(instruction.instruction, registerA);
            }



            return registerA[0];
        }

        static int[] op2func = new int[16];
    }
}
