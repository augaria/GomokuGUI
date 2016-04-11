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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            pictureBox1.SendToBack();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                GlobleVar.sh = new SocketHandler();
                if (GlobleVar.sh.start(textBox1.Text, textBox2.Text))
                {
                    GlobleVar.user = textBox1.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    MessageBox.Show("Wrong ID or password.");
            }
            catch
            {
                MessageBox.Show("Failed to connect to the server.");
            }
        }
    }
}
