using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class AoC14
    {
        const int RECIPESTOMAKE = 440231 + 10;
        const int RECIPESSEEN = 440231;
        const int SCORESIZE = 6;
        const int SCOREREM = 100000;

        public static string Function1()
        {
            int[] recipes = new int[RECIPESTOMAKE+1];
            recipes[0] = 3;
            recipes[1] = 7;
            int numrecipes = 2;

            int elf1 = 0;
            int elf2 = 1;

            while (numrecipes < RECIPESTOMAKE)
            {
                int newrecipe = recipes[elf1] + recipes[elf2];
                if (newrecipe > 9)
                    recipes[numrecipes++] = 1;
                recipes[numrecipes++] = newrecipe % 10;

                //Select next recipes to try
                elf1 = (elf1 + 1 + recipes[elf1]) % numrecipes;
                elf2 = (elf2 + 1 + recipes[elf2]) % numrecipes;
            }

            string result = "";
            for (int i = RECIPESTOMAKE - 10; i < RECIPESTOMAKE; i++)
                result += recipes[i].ToString();

            return result;
        }

        public static int Function2()
        {
            List<int> recipes = new List<int>();
            recipes.Add(3);
            recipes.Add(7);
            int numrecipes = 2;

            int elf1 = 0;
            int elf2 = 1;

            int backm1 = 0;
            int currScore = 37;

            while (true)
            {
                int newrecipe = recipes[elf1] + recipes[elf2];
                if (newrecipe > 9)
                {
                    recipes.Add(1);
                    if (recipes.Count > SCORESIZE)
                    {
                        currScore -= (currScore / SCOREREM) * SCOREREM;
                        currScore = (currScore * 10) + 1;
                        if (currScore == RECIPESSEEN)
                            break;
                    }
                    else
                        currScore = (currScore * 10) + 1;
                }

                int recipe2 = newrecipe % 10;
                recipes.Add(recipe2);
                if (recipes.Count > SCORESIZE)
                {
                    currScore -= (currScore / SCOREREM) * SCOREREM;
                    currScore = (currScore * 10) + recipe2;
                    if (currScore == RECIPESSEEN)
                        break;
                }
                else
                    currScore = (currScore * 10) + recipe2;

                //Select next recipes to try
                elf1 = (elf1 + 1 + recipes[elf1]) % recipes.Count;
                elf2 = (elf2 + 1 + recipes[elf2]) % recipes.Count;
            }

            return recipes.Count - SCORESIZE;
        }




    }
}
