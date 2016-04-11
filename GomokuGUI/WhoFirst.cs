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
    public partial class WhoFirst : Form
    {
        public WhoFirst()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            int randomNum;
            if (radioButton1.Checked)
                randomNum=rd.Next(101, 200);
            else if (radioButton2.Checked)
                randomNum = 0;
            else
                randomNum = rd.Next(1, 100);


            GlobleVar.sh.sendMessage("points", randomNum.ToString());
            this.Close();
        }

    }
}
