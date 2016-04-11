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
    public partial class Invitation : Form
    {
        string friend;

        public Invitation(string friend)
        {
            InitializeComponent();

            this.friend = friend;
            label1.Text = "Waiting for the response from " + friend;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }


        public void notfound()
        {
            MessageBox.Show("Friend is " + friend + " not found.");
        }

        public void denied()
        {
            MessageBox.Show(friend+" denied your invitation.");
            this.Close(); 
        }

        public void accepted()
        {
            MessageBox.Show(friend + " accepted your invitation.");
            this.DialogResult = DialogResult.OK;
            this.Close(); 
        }

    }
}
