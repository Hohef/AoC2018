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


        unsafe public static void ReadFile()
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

        unsafe public static int Function1()
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

        static int[] register2 = new int[6];
        unsafe class CDirectInstruction
        {
            public eOPCODE opcode;
            public int* A;
            public int* B;
            public int* C;
            public int Aa;
            public int Ba;
        }        

        unsafe public static int Function2()
        {
            int ip = 0;
            ReadFile();
            List<CDirectInstruction> dirprogram = new List<CDirectInstruction>();

            fixed (int* pRegister = register2, pIp = &register2[ip_register])
            {
                foreach (CProgram instruction in program)
                {
                    //Convert to pointer based program
                    CDirectInstruction newinstruct = new CDirectInstruction();
                    newinstruct.opcode = (eOPCODE)instruction.instruct[0];
                    switch (newinstruct.opcode)
                    {
                        case eOPCODE.addr:
                            newinstruct.A = pRegister + instruction.instruct[1];
                            newinstruct.B = pRegister + instruction.instruct[2];
                            break;
                        case eOPCODE.addi:
                            newinstruct.A = pRegister + instruction.instruct[1];
                            newinstruct.Ba = instruction.instruct[2];
                            break;
                        case eOPCODE.mulr: goto case eOPCODE.addr;
                        case eOPCODE.muli: goto case eOPCODE.addi;
                        case eOPCODE.banr: goto case eOPCODE.addr;
                        case eOPCODE.bani: goto case eOPCODE.addi;
                        case eOPCODE.borr: goto case eOPCODE.addr;
                        case eOPCODE.bori: goto case eOPCODE.addi;
                        case eOPCODE.setr:
                            newinstruct.A = pRegister + instruction.instruct[1];
                            break;
                        case eOPCODE.seti:
                            newinstruct.Aa = instruction.instruct[1];
                            break;
                        case eOPCODE.gtir:
                            newinstruct.Aa = instruction.instruct[1];
                            newinstruct.B = pRegister + instruction.instruct[2];
                            break;
                        case eOPCODE.gtri: goto case eOPCODE.addi;
                        case eOPCODE.gtrr: goto case eOPCODE.addr;
                        case eOPCODE.eqir: goto case eOPCODE.gtri;
                        case eOPCODE.eqri: goto case eOPCODE.addi;
                        case eOPCODE.eqrr: goto case eOPCODE.addr;
                    }
                    //Output Register is always third parameter
                    newinstruct.C = pRegister + instruction.instruct[3];
                    dirprogram.Add(newinstruct);
                }

                register2[0] = 1;
                while (ip < program.Count)
                {
                    //Write instruction point to register
                    *pIp = ip;
                    //Execute the line
                    CDirectInstruction statement = dirprogram[ip];

                    switch (statement.opcode)
                    {
                        case eOPCODE.addr: *statement.C = *statement.A + *statement.B; break;
                        case eOPCODE.addi: *statement.C = *statement.A + statement.Ba; break;
                        case eOPCODE.mulr: *statement.C = *statement.A * *statement.B; break;
                        case eOPCODE.muli: *statement.C = *statement.A * statement.Ba; break;
                        case eOPCODE.banr: *statement.C = *statement.A & *statement.B; break;
                        case eOPCODE.bani: *statement.C = *statement.A & statement.Ba; break;
                        case eOPCODE.borr: *statement.C = *statement.A | *statement.B; break;
                        case eOPCODE.bori: *statement.C = *statement.A | statement.Ba; break;
                        case eOPCODE.setr: *statement.C = *statement.A; break;
                        case eOPCODE.seti: *statement.C = statement.Aa; break;
                        case eOPCODE.gtir: *statement.C = (statement.Aa > *statement.B) ? 1 : 0; break;
                        case eOPCODE.gtri: *statement.C = (*statement.A > statement.Ba) ? 1 : 0; break;
                        case eOPCODE.gtrr: *statement.C = (*statement.A > *statement.B) ? 1 : 0; break;
                        case eOPCODE.eqir: *statement.C = (statement.Aa == *statement.B) ? 1 : 0; break;
                        case eOPCODE.eqri: *statement.C = (*statement.A == statement.Ba) ? 1 : 0; break;
                        case eOPCODE.eqrr: *statement.C = (*statement.A == *statement.B) ? 1 : 0; break;

                        default: return -1;
                    }

                    //Read next instruction pointer and increment by one
                    ip = *pIp;
                    ip++;
                }
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

        /// <summary>
        /// Function2() will solve the problem, but in a long time.  Went with option
        /// of looking at instructions and tailoring code to match instructions.  The
        /// input I was given would require 10551364 * (101551364 * 9) iterations.
        /// </summary>
        /// <returns></returns>
        public static long Function2a()
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

                //Instruction 3 begins main loop
                if (ip == 3)
                {
                    while (register[3] <= 10551364)
                    {
                        //Register0 only increments when instruction 4 has
                        //Register2 == Register5.  Register2 at this point
                        //is Register1*Register3 hence can only equal Register5
                        //when register 3 is a multiple of register5.
                        if (10551364 % register[3] == 0)
                            register[0] += register[3];
                        register[3]++;
                    }

                    break;
                }
            }

            return register[0];
        }
    }
}
