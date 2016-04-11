using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GomokuGUI
{
    static class Program
    {

        static public Form1 form1;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new History());

            //GlobleVar.mysql = new MySql();

            form1 = new Form1();
            Application.Run(form1);

            /*
            Login login = new Login();
            login.ShowDialog();
            if (login.DialogResult == DialogResult.OK)
                Application.Run(new Form1());
            else
                return;
            */
        }
    }
}
