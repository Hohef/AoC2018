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
delegate long FunctionL();

namespace AoC2018
{
    struct sProblemSet
    {
        public bool b1I, b1S, b2I, b2S, b1L, b2L;
        public Function Problem1I, Problem2I;
        public FunctionS Problem1S, Problem2S;
        public FunctionL Problem1L, Problem2L;

        public sProblemSet(Function func1, FunctionS func1s, Function func2, FunctionS func2s)
        {
            Problem1I = func1;
            Problem1S = func1s;
            Problem2I = func2;
            Problem2S = func2s;
            Problem1L = null;
            Problem2L = null;
            b1I = (func1 != null);
            b1S = (func1s != null);
            b2I = (func2 != null);
            b2S = (func2s != null);
            b1L = false;
            b2L = false;
        }

        public sProblemSet(Function func1, FunctionS func1s, FunctionL func1l, Function func2, FunctionS func2s, FunctionL func2l)
        {
            Problem1I = func1;
            Problem1S = func1s;
            Problem2I = func2;
            Problem2S = func2s;
            Problem1L = func1l;
            Problem2L = func2l;
            b1I = (func1 != null);
            b1S = (func1s != null);
            b2I = (func2 != null);
            b2S = (func2s != null);
            b1L = (func1l != null);
            b2L = (func2l != null);
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
            problemset.Add(new sProblemSet(AoC3.Function1, null, AoC3.Function2, null));
            problemset.Add(new sProblemSet(AoC4.Function1, null, AoC4.Function2, null));
            problemset.Add(new sProblemSet(AoC5.Function1, null, AoC5.Function2, null));
            problemset.Add(new sProblemSet(AoC6.Function1, null, AoC6.Function2, null));
            problemset.Add(new sProblemSet(null, AoC7.Function1, AoC7.Function2, null));
            problemset.Add(new sProblemSet(AoC8.Function1, null, AoC8.Function2, null));
            problemset.Add(new sProblemSet(AoC9.Function1, null, null, null, null, AoC9.Function2));
            problemset.Add(new sProblemSet(AoC10.Function1, null, null, null));
            problemset.Add(new sProblemSet(null, AoC11.Function1, null, AoC11.Function2));
            problemset.Add(new sProblemSet(AoC12.Function1, null, AoC12.Function2, null));
            problemset.Add(new sProblemSet(null, AoC13.Function1, null, AoC13.Function2));


            for (int i = 0; i < problemset.Count; i++)
                lb_Runs.Items.Add(string.Format("Day {0}",i+1));

            lb_Runs.SelectedIndex = lb_Runs.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lb_Runs.SelectedIndex >= 0)
            {
                if (problemset[lb_Runs.SelectedIndex].b1I) lbl_result.Text = "Result: " + problemset[lb_Runs.SelectedIndex].Problem1I.Invoke();
                else if (problemset[lb_Runs.SelectedIndex].b1S) lbl_result.Text = "Result: " + problemset[lb_Runs.SelectedIndex].Problem1S.Invoke();
                else if (problemset[lb_Runs.SelectedIndex].b1L) lbl_result2.Text = "Result2: " + problemset[lb_Runs.SelectedIndex].Problem1L.Invoke();
            }
        }

        private void Run2_Click(object sender, EventArgs e)
        {
            if (lb_Runs.SelectedIndex >= 0)
            {
                if (problemset[lb_Runs.SelectedIndex].b2I) lbl_result2.Text = "Result: " + problemset[lb_Runs.SelectedIndex].Problem2I.Invoke();
                else if (problemset[lb_Runs.SelectedIndex].b2S) lbl_result2.Text = "Result2: " + problemset[lb_Runs.SelectedIndex].Problem2S.Invoke();
                else if (problemset[lb_Runs.SelectedIndex].b2L) lbl_result2.Text = "Result2: " + problemset[lb_Runs.SelectedIndex].Problem2L.Invoke();
            }
        }
    }
}
