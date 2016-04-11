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

using System.Net;
using System.Net.Sockets;
using System.IO;


namespace GomokuGUI
{
    public partial class GeneralRecord : Form
    {

        string[][] posMatrix;

        int[] currentPage;
        bool[] hasNext;

        public GeneralRecord()
        {
            InitializeComponent();

            posMatrix = new string[5][];
            currentPage = new int[5];
            hasNext = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                currentPage[i] = 0;
                hasNext[i] = false;
            }

            try
            {

                getRecordUsr();

                getRecordPvE3();

                getRecordPvE2();

                getRecordPvE1();

                getRecordRecent();


                string result = GlobleVar.sh.sendMessageR("getUsers");
                if (!result.Equals("null"))
                {
                    List<string> names = (List<string>)JsonConvert.DeserializeObject(result, typeof(List<string>));
                    comboBox1.Items.AddRange(names.ToArray());
                    comboBox2.Items.AddRange(names.ToArray());
                }
            }
            catch
            {
                throw;
            }
        }


        private void getRecordUsr()
        {
            string result;
            result = GlobleVar.sh.sendMessageR("getRecordUsr", (currentPage[0]*20).ToString());
            if (!result.Equals("null"))
            {
                List<string[]> users = (List<string[]>)JsonConvert.DeserializeObject(result, typeof(List<string[]>));
                if (users.Count == 20)
                    hasNext[0] = true;
                else
                    hasNext[0] = false;

                listView1.Items.Clear();
                for (int i = 0; i < users.Count; i++)
                {
                    string[] tempUser = users[i];

                    ListViewItem tempItem = new ListViewItem(tempUser[0]);
                    tempItem.SubItems.Add(tempUser[1] + "/" + tempUser[2]);
                    tempItem.SubItems.Add(tempUser[3] + "/" + tempUser[4]);
                    tempItem.SubItems.Add(tempUser[5] + "/" + tempUser[6]);
                    tempItem.SubItems.Add(tempUser[7] + "/" + tempUser[8]);
                    tempItem.SubItems.Add(tempUser[9]);
                    listView1.Items.Add(tempItem);
                }
            }
        }

        private void getRecordPvE3()
        {
            string result = GlobleVar.sh.sendMessageR("getRecordPvE3", (currentPage[1] * 20).ToString());
            if (!result.Equals("null"))
            {
                List<string[]> pve3 = (List<string[]>)JsonConvert.DeserializeObject(result, typeof(List<string[]>));
                if (pve3.Count == 20)
                    hasNext[1] = true;
                else
                    hasNext[1] = false;

                listView2.Items.Clear();
                posMatrix[0] = new string[pve3.Count];
                for (int i = 0; i < pve3.Count; i++)
                {
                    string[] tempPve3 = pve3[i];

                    ListViewItem tempItem = new ListViewItem(tempPve3[0]);
                    tempItem.SubItems.Add(tempPve3[2]);
                    tempItem.SubItems.Add(tempPve3[1]);
                    tempItem.SubItems.Add(tempPve3[3]);
                    tempItem.SubItems.Add(tempPve3[4]);
                    listView2.Items.Add(tempItem);
                    posMatrix[0][i] = tempPve3[5];
                }
            }
        }

        private void getRecordPvE2()
        {
            string result = GlobleVar.sh.sendMessageR("getRecordPvE2", (currentPage[2] * 20).ToString());
            if (!result.Equals("null"))
            {
                List<string[]> pve2 = (List<string[]>)JsonConvert.DeserializeObject(result, typeof(List<string[]>));
                if (pve2.Count == 20)
                    hasNext[2] = true;
                else
                    hasNext[2] = false;

                listView3.Items.Clear();
                posMatrix[1] = new string[pve2.Count];
                for (int i = 0; i < pve2.Count; i++)
                {
                    string[] tempPve2 = pve2[i];

                    ListViewItem tempItem = new ListViewItem(tempPve2[0]);
                    tempItem.SubItems.Add(tempPve2[2]);
                    tempItem.SubItems.Add(tempPve2[1]);
                    tempItem.SubItems.Add(tempPve2[3]);
                    tempItem.SubItems.Add(tempPve2[4]);
                    listView3.Items.Add(tempItem);
                    posMatrix[1][i] = tempPve2[5];
                }
            }

        }

        private void getRecordPvE1()
        {
            string result = GlobleVar.sh.sendMessageR("getRecordPvE1", (currentPage[3] * 20).ToString());
            if (!result.Equals("null"))
            {
                List<string[]> pve1 = (List<string[]>)JsonConvert.DeserializeObject(result, typeof(List<string[]>));
                if (pve1.Count == 20)
                    hasNext[3] = true;
                else
                    hasNext[3] = false;

                listView4.Items.Clear();
                posMatrix[2] = new string[pve1.Count];
                for (int i = 0; i < pve1.Count; i++)
                {
                    string[] tempPve1 = pve1[i];

                    ListViewItem tempItem = new ListViewItem(tempPve1[0]);
                    tempItem.SubItems.Add(tempPve1[2]);
                    tempItem.SubItems.Add(tempPve1[1]);
                    tempItem.SubItems.Add(tempPve1[3]);
                    tempItem.SubItems.Add(tempPve1[4]);
                    listView4.Items.Add(tempItem);
                    posMatrix[2][i] = tempPve1[5];
                }
            }
        }


        private void getRecordRecent()
        {
            string result = GlobleVar.sh.sendMessageR("getRecordRecent", (currentPage[4] * 20).ToString());
            if (!result.Equals("null"))
            {
                List<string[]> recent = (List<string[]>)JsonConvert.DeserializeObject(result, typeof(List<string[]>));
                if (recent.Count == 20)
                    hasNext[4] = true;
                else
                    hasNext[4] = false;

                listView6.Items.Clear();
                posMatrix[4] = new string[recent.Count];
                for (int i = 0; i < recent.Count; i++)
                {
                    string[] tempRecent = recent[i];

                    ListViewItem tempItem = new ListViewItem(tempRecent[0]);
                    tempItem.SubItems.Add(tempRecent[1]);
                    tempItem.SubItems.Add(tempRecent[4]);
                    tempItem.SubItems.Add(tempRecent[2]);
                    tempItem.SubItems.Add(tempRecent[3]);
                    tempItem.SubItems.Add(tempRecent[5]);
                    tempItem.SubItems.Add(tempRecent[6]);
                    listView6.Items.Add(tempItem);
                    posMatrix[4][i] = tempRecent[7];
                }
            }
        }


        private void Rank_Load(object sender, EventArgs e)
        {

        }

        //page up
        private void button2_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (currentPage[0] == 0)
                    MessageBox.Show("No page up.");
                else
                {
                    currentPage[0]--;
                    getRecordUsr();
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                if (currentPage[1] == 0)
                    MessageBox.Show("No page up.");
                else
                {
                    currentPage[1]--;
                    getRecordPvE3();
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                if (currentPage[2] == 0)
                    MessageBox.Show("No page up.");
                else
                {
                    currentPage[2]--;
                    getRecordPvE2();
                }
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                if (currentPage[3] == 0)
                    MessageBox.Show("No page up.");
                else
                {
                    currentPage[3]--;
                    getRecordPvE1();
                }
            }
            else if (tabControl1.SelectedIndex == 5)
            {
                if (currentPage[4] == 0)
                    MessageBox.Show("No page up.");
                else
                {
                    currentPage[4]--;
                    getRecordRecent();
                }
            }

        }

        //page down
        private void button3_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (hasNext[0])
                {
                    currentPage[0]++;
                    getRecordUsr();
                }

                else
                    MessageBox.Show("No page down.");
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                if (hasNext[1])
                {
                    currentPage[1]++;
                    getRecordPvE3();
                }
                else
                    MessageBox.Show("No page down.");
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                if (hasNext[2])
                {
                    currentPage[2]++;
                    getRecordPvE2();
                }
                else
                    MessageBox.Show("No page down.");
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                if (hasNext[3])
                {
                    currentPage[3]++;
                    getRecordPvE1();
                }
                else
                    MessageBox.Show("No page down.");
            }
            else if (tabControl1.SelectedIndex == 5)
            {
                if (hasNext[4])
                {
                    currentPage[4]++;
                    getRecordRecent();
                }  
                else
                MessageBox.Show("No page down.");
            }
        }


        //view
        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                PersonalRecord pr = new PersonalRecord(listView1.SelectedItems[0].Text);
                pr.ShowDialog(this);
            }
            else if (tabControl1.SelectedIndex == 1 || tabControl1.SelectedIndex == 2 || tabControl1.SelectedIndex == 3)
            {
                ListView tempListView = (ListView)tabControl1.SelectedTab.Controls[0];
                string secondPlayer = "AI";
                if (tempListView.SelectedItems[0].SubItems[2].Text.Equals("AI"))
                    secondPlayer = tempListView.SelectedItems[0].SubItems[1].Text;
                GlobleVar.rv = new Review(tempListView.SelectedItems[0].SubItems[2].Text, secondPlayer, posMatrix[tabControl1.SelectedIndex - 1][tempListView.SelectedItems[0].Index], "Lv"+(4 - tabControl1.SelectedIndex).ToString());
                Program.form1.groupBox4.Enabled = true;
                Program.form1.groupBox4.Visible = true;
                Program.form1.button5.Enabled = true;
                Program.form1.button5.Visible = true;
                Program.form1.button6.Enabled = true;
                Program.form1.button6.Visible = true;
                button2.Enabled = false;
                button2.Visible = false;
                this.Close();
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                ListView tempListView = (ListView)tabControl1.SelectedTab.Controls[5];
                GlobleVar.rv = new Review(tempListView.SelectedItems[0].SubItems[2].Text, tempListView.SelectedItems[0].SubItems[3].Text, posMatrix[tabControl1.SelectedIndex - 1][tempListView.SelectedItems[0].Index],"PvP");
                Program.form1.groupBox4.Enabled = true;
                Program.form1.groupBox4.Visible = true;
                Program.form1.button5.Enabled = true;
                Program.form1.button5.Visible = true;
                Program.form1.button6.Enabled = true;
                Program.form1.button6.Visible = true;
                button2.Enabled = false;
                button2.Visible = false;
                this.Close();
            }
            else
            {
                ListView tempListView = (ListView)tabControl1.SelectedTab.Controls[0];
                if(tempListView.SelectedItems[0].SubItems[1].Text.Equals("pvp"))
                    GlobleVar.rv = new Review(tempListView.SelectedItems[0].SubItems[3].Text, tempListView.SelectedItems[0].SubItems[4].Text, posMatrix[tabControl1.SelectedIndex - 1][tempListView.SelectedItems[0].Index], "PvP");
                else
                    GlobleVar.rv = new Review(tempListView.SelectedItems[0].SubItems[3].Text, tempListView.SelectedItems[0].SubItems[4].Text, posMatrix[tabControl1.SelectedIndex - 1][tempListView.SelectedItems[0].Index], "Lv"+tempListView.SelectedItems[0].SubItems[1].Text);
                Program.form1.groupBox4.Enabled = true;
                Program.form1.groupBox4.Visible = true;
                Program.form1.button5.Enabled = true;
                Program.form1.button5.Visible = true;
                Program.form1.button6.Enabled = true;
                Program.form1.button6.Visible = true;
                button2.Enabled = false;
                button2.Visible = false;
                this.Close();
            }
        }

        //search pvp record
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                listView5.Items.Clear();
                string firstPlayer = comboBox1.SelectedItem.ToString();
                string secondPlayer = comboBox2.SelectedItem.ToString();

                if (firstPlayer.Equals("") || secondPlayer.Equals(""))
                    return;

                string result = GlobleVar.sh.sendMessageR("getRecordPvP", firstPlayer, secondPlayer);

                if (result.Equals("null"))
                    return;

                List<string[]> pvp = (List<string[]>)JsonConvert.DeserializeObject(result, typeof(List<string[]>));
                posMatrix[3] = new string[pvp.Count];
                for (int i = 0; i < pvp.Count; i++)
                {
                    string[] tempPvp = pvp[i];

                    ListViewItem tempItem = new ListViewItem(tempPvp[0]);
                    tempItem.SubItems.Add(tempPvp[3]);
                    tempItem.SubItems.Add(tempPvp[1]);
                    tempItem.SubItems.Add(tempPvp[2]);
                    tempItem.SubItems.Add(tempPvp[4]);
                    tempItem.SubItems.Add(tempPvp[5]);
                    listView5.Items.Add(tempItem);
                    posMatrix[3][i] = tempPvp[6];
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
