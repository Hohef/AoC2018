using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

delegate int Function();
delegate string FunctionS();

namespace AoC2018
{
    struct sProblemSet
    {
        public bool b1I, b1S, b2I, b2S;
        public Function Problem1I, Problem2I;
        public FunctionS Problem1S, Problem2S;

        public sProblemSet(Function func1, FunctionS func1s, Function func2, FunctionS func2s)
        {
            Problem1I = func1;
            Problem1S = func1s;
            Problem2I = func2;
            Problem2S = func2s;
            b1I = (func1 != null);
            b1S = (func1s != null);
            b2I = (func2 != null);
            b2S = (func2s != null);
        }
    }


    public partial class Form1 : Form
    {
        List<sProblemSet> problemset = new List<sProblemSet>(25);

        public Form1()
        {
            InitializeComponent();

            InitializeProblemSet();
        }

        private void InitializeProblemSet()
        {
            problemset.Add(new sProblemSet(AoC1.Function1, null, null, null));
            problemset.Add(new sProblemSet(AoC2.Function1, null, null, AoC2.Function2));

            for (int i = 0; i < problemset.Count; i++)
                lb_Runs.Items.Add(string.Format("Day {0}",i+1));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lb_Runs.SelectedIndex >= 0)
            {
                if (problemset[lb_Runs.SelectedIndex].b1I) lbl_result.Text = "Result: " + problemset[lb_Runs.SelectedIndex].Problem1I.Invoke();
                else if (problemset[lb_Runs.SelectedIndex].b1S) lbl_result.Text = "Result: " + problemset[lb_Runs.SelectedIndex].Problem1S.Invoke();
            }
        }

        private void Run2_Click(object sender, EventArgs e)
        {
            if (lb_Runs.SelectedIndex >= 0)
            {
                if (problemset[lb_Runs.SelectedIndex].b2I) lbl_result.Text = "Result: " + problemset[lb_Runs.SelectedIndex].Problem2I.Invoke();
                else if (problemset[lb_Runs.SelectedIndex].b2S) lbl_result2.Text = "Result2: " + problemset[lb_Runs.SelectedIndex].Problem2S.Invoke();
            }
        }

        private string FunctionSNull()
        {
            return "";
        }
        private int FunctionINull()
        {
            return -1;
        }
    }
}
