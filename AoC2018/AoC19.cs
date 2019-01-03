using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AoC2018
{
    class AoC19
    {
        enum eOPCODE { addr, addi, mulr, muli, banr, bani, borr, bori, setr, seti, gtir, gtri, gtrr, eqir, eqri, eqrr };
        static int[] register = new int[6];
        class CProgram
        {
            public int [] instruct = new int[4];
        }

        static List<CProgram> program = new List<CProgram>();
        static int ip_register;

        private static int CovertToFuncCode(string opcode)
        {
            switch (opcode)
            {
                case "addr": return (int)eOPCODE.addr;
                case "addi": return (int)eOPCODE.addi;
                case "mulr": return (int)eOPCODE.mulr;
                case "muli": return (int)eOPCODE.muli;
                case "banr": return (int)eOPCODE.banr;
                case "bani": return (int)eOPCODE.bani;
                case "borr": return (int)eOPCODE.borr;
                case "bori": return (int)eOPCODE.bori;
                case "setr": return (int)eOPCODE.setr;
                case "seti": return (int)eOPCODE.seti;
                case "gtir": return (int)eOPCODE.gtir;
                case "gtri": return (int)eOPCODE.gtri;
                case "gtrr": return (int)eOPCODE.gtrr;
                case "eqir": return (int)eOPCODE.eqir;
                case "eqri": return (int)eOPCODE.eqri;
                case "eqrr": return (int)eOPCODE.eqrr;
                default: return -1;
            }
        }


        public static void ReadFile()
        {
            program.Clear();
            System.IO.StreamReader reader = new StreamReader(@"..\..\Inputs\Input19.txt");
            ip_register = int.Parse(reader.ReadLine().Split(' ')[1]);

            while (!reader.EndOfStream)
            {
                string [] line = reader.ReadLine().Split(' ');
                CProgram instruction = new CProgram();
                instruction.instruct[0] = CovertToFuncCode(line[0]);
                instruction.instruct[1] = int.Parse(line[1]);
                instruction.instruct[2] = int.Parse(line[2]);
                instruction.instruct[3] = int.Parse(line[3]);
                program.Add(instruction);
            }
        }

        public static int Function1()
        {
            int ip = 0; int outreg;
            ReadFile();
            while (ip < program.Count)
            {
                //Write instruction point to register
                register[ip_register] = ip;
                //Execute the line
                outreg = program[ip].instruct[3];
                register[outreg] = OpcodeExecute(program[ip].instruct, register);
                ip = register[ip_register];
                ip++;
            }

            return register[0];
        }

        public static int Function2()
        {
            ReadFile();

            //Function1();

            int ip = 0; int outreg;
            
            register[0] = 1;
            while (ip < program.Count)
            {
                //Write instruction point to register
                register[ip_register] = ip;
                //Execute the line
                outreg = program[ip].instruct[3];
                register[outreg] = OpcodeExecute(program[ip].instruct, register);
                ip = register[ip_register];
                ip++;
            }

            return register[0];
        }

        private static int OpcodeExecute(int[] instruct, int[] regi)
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
    }
}
