using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuGUI
{
    class Review
    {
        private int[] pos;

        private int temp;

        public string first;

        public string second;

        public Review(string first,string second,string posStr,string level)
        {
            this.first = first;
            this.second = second;
            string [] posStrArray = posStr.Split(' ');
            pos = new int[posStrArray.Length-1];
            for (int i=0; i < pos.Length; i++)
            {
                pos[i] = Convert.ToInt32(posStrArray[i]);
            }

            temp = -1;

            Program.form1.Clear_chessboard();
            Program.form1.label8.Text = first;
            Program.form1.label9.Text = second;
            Program.form1.label10.Text = first;
            Program.form1.label11.Text = "";
            Program.form1.label12.Text = "0";
            Program.form1.first = first;
            Program.form1.second = second;
            Program.form1.label4.Text = level;
        }

        public int next()
        {
            if (temp == pos.Length-1)
                return -1;

            temp++;
            return pos[temp];
        }


    }
}
