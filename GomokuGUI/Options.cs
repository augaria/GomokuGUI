using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GomokuGUI
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                GlobleVar.STEPS = 3;
            else if (radioButton2.Checked)
                GlobleVar.STEPS = 4;
            else
                GlobleVar.STEPS = 5;

            GlobleVar.playerFirst = radioButton4.Checked;

            this.Close();
        }

        private void LevelOption_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (radioButton1.Checked)
                GlobleVar.STEPS = 3;
            else if (radioButton2.Checked)
                GlobleVar.STEPS = 4;
            else
                GlobleVar.STEPS = 5;

            GlobleVar.playerFirst = radioButton4.Checked;
        }
    }
}
