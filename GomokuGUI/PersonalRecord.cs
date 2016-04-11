using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GomokuGUI
{
    public partial class PersonalRecord : Form
    {

        string[] posArray;
        private bool selected;

        public PersonalRecord(string player)
        {
            InitializeComponent();

            selected = false;

            Text = player;
            try
            {
                string result = GlobleVar.sh.sendMessageR("getRecordPersonal",player);
                if (!result.Equals("null"))
                {
                    List<string[]> personal = (List<string[]>)JsonConvert.DeserializeObject(result, typeof(List<string[]>));

                    posArray = new string[personal.Count];

                    for (int i = 0; i < personal.Count; i++)
                    {
                        string[] tempPersonal = personal[i];

                        ListViewItem tempItem = new ListViewItem(tempPersonal[0]);
                        tempItem.SubItems.Add(tempPersonal[1]);
                        tempItem.SubItems.Add(tempPersonal[4]);
                        tempItem.SubItems.Add(tempPersonal[2]);
                        tempItem.SubItems.Add(tempPersonal[3]);
                        tempItem.SubItems.Add(tempPersonal[5]);
                        tempItem.SubItems.Add(tempPersonal[6]);
                        listView1.Items.Add(tempItem);
                        posArray[i] = tempPersonal[7];
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems[0].SubItems[1].Text.Equals("pvp"))
                GlobleVar.rv = new Review(listView1.SelectedItems[0].SubItems[3].Text, listView1.SelectedItems[0].SubItems[4].Text, posArray[listView1.SelectedItems[0].Index], "PvP");
            else
                GlobleVar.rv = new Review(listView1.SelectedItems[0].SubItems[3].Text, listView1.SelectedItems[0].SubItems[4].Text, posArray[listView1.SelectedItems[0].Index], "Lv" + listView1.SelectedItems[0].SubItems[1].Text);
            Program.form1.groupBox4.Enabled = true;
            Program.form1.groupBox4.Visible = true;
            Program.form1.button5.Enabled = true;
            Program.form1.button5.Visible = true;
            Program.form1.button6.Enabled = true;
            Program.form1.button6.Visible = true;
            button2.Enabled = false;
            button2.Visible = false;

            selected = true;
            this.Close();
        }

        private void PersonalRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(selected)
                this.Owner.Close();
        }
    }
}
