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

namespace GomokuGUI
{
    public partial class Form1 : Form
    {

        private bool pvp;
        public delegate void OneIntInvoke(int a);
        public delegate void OneBoolInvoke(bool a);
        public delegate void OneStringInvoke(string a);
        public delegate void TwoStringInvoke(string a, string b);
        private bool isWhite;
        private int moveCount;

        public string first;
        public string second;
        

        public Form1()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
            richTextBox1.TextChanged += richTextBoxTextChanged;
            tabPage1.Enter += tabFocus;
            Clear_chessboard();
            pvp = false;
            GlobleVar.user = "Guest";
            GlobleVar.friend = "";
        }

        //new
        private void button1_Click(object sender, EventArgs e)
        {
            Clear_chessboard();

            Options lo = new Options();
            lo.ShowDialog();

            label4.Text = "Lv" + (GlobleVar.STEPS-2);
            
            richTextBox1.Text = "Player vs AI, new game starts!!!\r\n";

            if (GlobleVar.playerFirst)
            {
                pictureBox1.MouseClick += pictureBox1_MouseClick;
                first = GlobleVar.user;
                second = "AI";
                label8.Text = first;
                label9.Text = second;
                label10.Text = first;
                label11.Text = "";
                label12.Text = "0";
            }
            else
            {
                first = "AI";
                second = GlobleVar.user;
                label8.Text = first;
                label9.Text = second;
                label10.Text = first;
                label11.Text = "";
                label12.Text = "0";
                AIMove();
            }
            
        }


        public void friendMove(int position)
        {
            pictureBox1.MouseClick += pictureBox1_MouseClick;

            GlobleVar.AI = 3 - GlobleVar.AI;
            GlobleVar.PLAYER = 3 - GlobleVar.PLAYER;
            GlobleVar.preValue = 0 - GlobleVar.preValue;
            GlobleVar.nodeCount = 0;
            //GlobleVar.STEPS = 3;
            //GlobleVar.EMPTY_3 = param;

            
            GlobleVar.choice = position;
            GlobleVar.chessboard[position / 15, position % 15] = GlobleVar.AI;

            Display_GUI(GlobleVar.choice);

            TreeNode root = new TreeNode(GlobleVar.choice, null);
            //root.turn = 3 - root.turn;
            root.eval(false);
            GlobleVar.findChoices(GlobleVar.choice, GlobleVar.chessboard, GlobleVar.choices);
            GlobleVar.preValue = root.tempValue;

            GlobleVar.posHistory += GlobleVar.choice.ToString() + " ";
            if (GlobleVar.MostSeq[GlobleVar.AI] >= 5)
            {
                pictureBox1.MouseClick -= pictureBox1_MouseClick;
                
                GlobleVar.sh.sendMessage("position", "-1");
                MessageBox.Show("Your friend wins!");
               
                pvp = false;
                return;
            }

            GlobleVar.round++;
            
        }

        //put a piece
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            //int param = 3600;
            GlobleVar.AI = 3 - GlobleVar.AI;
            GlobleVar.PLAYER = 3 - GlobleVar.PLAYER;
            GlobleVar.preValue = 0 - GlobleVar.preValue;
            GlobleVar.nodeCount = 0;
            //GlobleVar.STEPS = 3;
            //GlobleVar.EMPTY_3 = param;

            int x = (e.X - 5) / 35;
            int y = (e.Y - 5) / 35;

            if (GlobleVar.chessboard[x, y] == GlobleVar.PLAYER || GlobleVar.chessboard[x, y] == GlobleVar.AI)
            {
                //Console.WriteLine("Already has a piece, change a place.");
                //Console.WriteLine();
                pictureBox1.MouseClick += pictureBox1_MouseClick;
                return;
            }
            GlobleVar.choice = x * 15 + y;
            GlobleVar.chessboard[x, y] = GlobleVar.AI;

            Display_GUI(GlobleVar.choice);
            pictureBox1.Update();
            pictureBox1.MouseClick -= pictureBox1_MouseClick;
            GlobleVar.posHistory += GlobleVar.choice.ToString() + " ";

            TreeNode root = new TreeNode(GlobleVar.choice, null);
            //root.turn = 3 - root.turn;
            root.eval(false);
            GlobleVar.findChoices(GlobleVar.choice, GlobleVar.chessboard, GlobleVar.choices);
            GlobleVar.preValue = root.tempValue;
            
            if (pvp)
                GlobleVar.sh.sendMessage("position", GlobleVar.choice.ToString());

            if (GlobleVar.MostSeq[GlobleVar.AI] >= 5)
            {
                pictureBox1.MouseClick -= pictureBox1_MouseClick;
                
                MessageBox.Show("You win!");
                //Console.WriteLine("Congratulations! You Win!\n\n");
                TimeSpan ts = DateTime.Now - GlobleVar.startTime;

                int level = GlobleVar.STEPS - 2;
                if (pvp)
                {
                    level = 0;
                }
                if (GlobleVar.sh != null && radioButton1.Checked)
                    GlobleVar.sh.record(level, first, second, GlobleVar.user, Convert.ToInt32(ts.TotalSeconds), (moveCount + 1) / 2, GlobleVar.posHistory);
                //GlobleVar.mysql.insert(0, GlobleVar.user, "AI", GlobleVar.user, Convert.ToInt32(ts.TotalSeconds), GlobleVar.round, GlobleVar.posHistory);
                //Console.ReadLine();

                pvp = false;
                return;
            }



            if (!pvp)
                AIMove();
        }

        private void AIMove()
        {
            GlobleVar.AI = 3 - GlobleVar.AI;
            GlobleVar.PLAYER = 3 - GlobleVar.PLAYER;
            GlobleVar.preValue = 0 - GlobleVar.preValue;
            GlobleVar.nodeCount = 0;
            //GlobleVar.STEPS = 3;
            //GlobleVar.EMPTY_3 = param;

            //GlobleVar.sw.Start();
            GlobleVar.choice = Agent.agent(GlobleVar.choice);
            //GlobleVar.sw.Stop();

            Display_GUI(GlobleVar.choice);

            GlobleVar.posHistory += GlobleVar.choice.ToString() + " ";
            if (GlobleVar.MostSeq[GlobleVar.AI] >= 5)
            {
                pictureBox1.MouseClick -= pictureBox1_MouseClick;
                MessageBox.Show("AI wins!");
                //Console.WriteLine("Unfortunately, AI Wins\n");
                TimeSpan ts = DateTime.Now - GlobleVar.startTime;

                if (GlobleVar.sh != null && radioButton1.Checked)
                    GlobleVar.sh.record(GlobleVar.STEPS - 2, first, second, "AI", Convert.ToInt32(ts.TotalSeconds), (moveCount + 1) / 2, GlobleVar.posHistory);
                //GlobleVar.mysql.insert(0, GlobleVar.user, "AI", "AI", Convert.ToInt32(ts.TotalSeconds), GlobleVar.round, GlobleVar.posHistory);
                return;
            }

            GlobleVar.round++;

            pictureBox1.MouseClick += pictureBox1_MouseClick;
        }

        private void Display_GUI(int choice)
        {
            if (pictureBox1.InvokeRequired)
            {
                OneIntInvoke _displayInvoke = new OneIntInvoke(Display_GUI);
                this.Invoke(_displayInvoke, new object[] { choice });
            }
            else
            {
                int px = choice / 15 * 35 + 10;
                int py = (choice % 15) * 35 + 10;
                System.Windows.Forms.PictureBox pictureBox2;
                pictureBox2 = new System.Windows.Forms.PictureBox();
                pictureBox1.Controls.Add(pictureBox2);
                pictureBox2.BackColor = System.Drawing.Color.Transparent;
                if (isWhite)
                    pictureBox2.Image = global::GomokuGUI.Properties.Resources.white;
                else
                    pictureBox2.Image = global::GomokuGUI.Properties.Resources.black;
                pictureBox2.Location = new System.Drawing.Point(px, py);
                pictureBox2.Name = "pictureBox2";
                pictureBox2.Size = new System.Drawing.Size(24, 24);
                pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                pictureBox2.TabIndex = 2;
                pictureBox2.TabStop = false;
                isWhite = !isWhite;
                moveCount++;
                if (moveCount % 2 == 0)
                    label10.Text = first;
                else
                    label10.Text = second;
                label11.Text=(char)('A'+choice%15)+(choice/15+1).ToString();
                label12.Text = ((moveCount+1) / 2).ToString();
            }
        }


        public void Clear_chessboard()
        {
            for (int i = 0; i < GlobleVar.boardSize; i++)
                for (int j = 0; j < GlobleVar.boardSize; j++)
                    GlobleVar.chessboard[i, j] = 0;

            GlobleVar.TEST = false;

            GlobleVar.nodeCount = 0;
            GlobleVar.preValue = 0;

            GlobleVar.round = 0;

            GlobleVar.handledNode = 0;
            GlobleVar.possibleNode = 0;

            GlobleVar.MostSeq = new int[3] { 0, 0, 0 };
            GlobleVar.winChoice = new int[3] { -1, -1, -1 };

            GlobleVar.AI = 2;
            GlobleVar.PLAYER = 1;
            GlobleVar.STEPS = 5;
            GlobleVar.choices = new ArrayList();

            GlobleVar.posHistory = "";
            GlobleVar.choice = -1;

            GlobleVar.startTime = DateTime.Now;

            moveCount = 0;

            isWhite = true;

            pictureBox1.Controls.Clear();

            for (int i = 0; i < 15; i++)
            {
                System.Windows.Forms.Label labelY = new System.Windows.Forms.Label();
                labelY.AutoSize = true;
                labelY.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                labelY.ForeColor = System.Drawing.Color.Purple;
                labelY.BackColor = System.Drawing.Color.Transparent;
                labelY.Location = new System.Drawing.Point(7, 16 + i * 35);
                labelY.Name = "labelY";
                labelY.Size = new System.Drawing.Size(14, 13);
                labelY.Text = "" + (char)('A' + i);
                this.pictureBox1.Controls.Add(labelY);
            }

            for (int i = 0; i < 15; i++)
            {
                System.Windows.Forms.Label labelX = new System.Windows.Forms.Label();
                labelX.AutoSize = true;
                labelX.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                labelX.ForeColor = System.Drawing.Color.Purple;
                labelX.BackColor = System.Drawing.Color.Transparent;
                labelX.Location = new System.Drawing.Point(16+i*35, 7);
                labelX.Name = "labelX";
                labelX.Size = new System.Drawing.Size(14, 13);
                labelX.Text = (i+1).ToString();
                this.pictureBox1.Controls.Add(labelX);
            }

            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            label12.Text = "0";
            groupBox4.Enabled = false;
            groupBox4.Visible = false;
            button5.Enabled = false;
            button5.Visible = false;
            button6.Enabled = false;
            button6.Visible = false;
            button2.Enabled = true;
            button2.Visible = true;

            pictureBox1.MouseClick -= pictureBox1_MouseClick;

        }


        //review
        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                MessageBox.Show("Please get online first.");
                return;
            }
            try
            {
                GeneralRecord rank = new GeneralRecord();
                rank.ShowDialog();
            }
            catch (Exception error)
            {
                richTextBox1.Text += error.ToString();
            }

        }

        //online
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton1.Checked)
                return;


            Login login = new Login();
            login.ShowDialog();
            if (login.DialogResult == DialogResult.OK)
            {
                button3.Enabled = true;
                button4.Enabled = true;
                button7.Enabled = true;
                
            }

            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
        }

        //offline
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!radioButton2.Checked)
                    return;

                //ends the connection
                if (GlobleVar.sh != null)
                    GlobleVar.sh.stop();
                GlobleVar.sh = null;
                listBox1.Items.Clear();
                Clear_chessboard();
                GlobleVar.friend = "";
                label4.Text = "";
                pvp = false;

                for (int i = 1; i < tabControl1.TabPages.Count; i++)
                    tabControl1.TabPages.RemoveAt(i);
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (GlobleVar.sh != null)
                    GlobleVar.sh.stop();
            }
            catch { }
        }


        private void challengeFriend(object sender, EventArgs e)
        {
            GlobleVar.sh.sendMessage("request", listBox1.SelectedItem.ToString());
            GlobleVar.it = new Invitation(listBox1.SelectedItem.ToString());
            GlobleVar.it.ShowDialog();
            if (GlobleVar.it.DialogResult == DialogResult.Abort)
                GlobleVar.sh.sendMessage("cancel");
            else if (GlobleVar.it.DialogResult == DialogResult.OK)
            {
                GlobleVar.friend = listBox1.SelectedItem.ToString();
                label4.Text = "PvP";
                GlobleVar.sh.sendMessage("confirmA");
            }
        }


        public void newPvP(bool isFirst)
        {
            if (pictureBox1.InvokeRequired)
            {
                OneBoolInvoke _newPvPInvoke = new OneBoolInvoke(newPvP);
                this.Invoke(_newPvPInvoke, new object[] { isFirst });
            }
            else
            {
                Clear_chessboard();
                if (isFirst)
                {
                    pictureBox1.MouseClick += pictureBox1_MouseClick;
                    first = GlobleVar.user;
                    second = GlobleVar.friend;
                }
                else
                {
                    first = GlobleVar.friend;
                    second = GlobleVar.user;
                }
                label8.Text = first;
                label9.Text = second;
                label10.Text = first;
                label11.Text = "";
                label12.Text = "0";
                pvp = true;
                richTextBox1.Text = GlobleVar.user + " vs " + GlobleVar.friend + ", new game starts!!!\r\n";
            }
        }

        //next
        private void button5_Click(object sender, EventArgs e)
        {
            int temp = GlobleVar.rv.next();
            if ( temp == -1)
                MessageBox.Show("Review Finished.");
            else
                Display_GUI(temp);
        }

        //chat
        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString().Equals(GlobleVar.user))
                return;
            string result = GlobleVar.sh.sendMessageR("setupChat", listBox1.SelectedItem.ToString());
            if (result.Equals("succeed"))
                addChat(listBox1.SelectedItem.ToString());
            else
                MessageBox.Show("Not Online.");
        }

        public void addChat(string name)
        {
            if (tabControl1.InvokeRequired)
            {
                OneStringInvoke _addChatInvoke = new OneStringInvoke(addChat);
                this.Invoke(_addChatInvoke, new object[] { name });
            }
            else
            {
                TabPage tp = new TabPage();
                tp.Name = name;
                tp.Text = name;
                RichTextBox rtb = new RichTextBox();
                rtb.Size = new System.Drawing.Size(244, 179);
                rtb.Location = new System.Drawing.Point(0, 0);
                rtb.TextChanged += richTextBoxTextChanged;
                tp.Controls.Add(rtb);
                tp.Enter+=tabFocus;
                tabControl1.TabPages.Add(tp);
                System.Media.SystemSounds.Beep.Play();
            }
        }

        public void addMessage(string name, string message)
        {

            if (tabControl1.InvokeRequired)
            {
                TwoStringInvoke _addMessageInvoke = new TwoStringInvoke(addMessage);
                this.Invoke(_addMessageInvoke, new object[] { name, message });
            }
            else
            {
                for (int i = 1; i < tabControl1.TabPages.Count; i++)
                {
                    if (tabControl1.TabPages[i].Name.Equals(name))
                    {
                        ((RichTextBox)(tabControl1.TabPages[i].Controls[0])).SelectionColor = Color.Green;
                        ((RichTextBox)(tabControl1.TabPages[i].Controls[0])).AppendText(name + ":"+"\r\n");
                        ((RichTextBox)(tabControl1.TabPages[i].Controls[0])).SelectionColor = Color.Green;
                        ((RichTextBox)(tabControl1.TabPages[i].Controls[0])).AppendText("  "+message+"\r\n");
                        if (tabControl1.TabPages[i]!=tabControl1.SelectedTab)
                        {
                            tabControl1.TabPages[i].Text = tabControl1.TabPages[i].Text.Replace("*", "");
                            tabControl1.TabPages[i].Text += "*";
                        }
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
            }
        }

        public void endChat(string name)
        {
            if (tabControl1.InvokeRequired)
            {
                OneStringInvoke _endChatInvoke = new OneStringInvoke(endChat);
                this.Invoke(_endChatInvoke, new object[] { name });
            }
            else
            {
                for (int i = 1; i < tabControl1.TabPages.Count; i++)
                {
                    if (tabControl1.TabPages[i].Name.Equals(name))
                    {
                        tabControl1.TabPages.RemoveAt(i);
                        return;
                    }
                }
            }
                
        }

        //send by button
        private void button4_Click(object sender, EventArgs e)
        {
            string name = tabControl1.SelectedTab.Name;
            GlobleVar.sh.sendMessage("chat", name, richTextBox2.Text);
            ((RichTextBox)(tabControl1.SelectedTab.Controls[0])).SelectionColor = Color.Blue;
            ((RichTextBox)(tabControl1.SelectedTab.Controls[0])).AppendText(GlobleVar.user + ":\r\n");
            ((RichTextBox)(tabControl1.SelectedTab.Controls[0])).SelectionColor = Color.Blue;
            ((RichTextBox)(tabControl1.SelectedTab.Controls[0])).AppendText("  " + richTextBox2.Text + "\r\n");
            richTextBox2.Text = "";
        }

        //send by Enter
        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string name = tabControl1.SelectedTab.Name;
                GlobleVar.sh.sendMessage("chat", name, richTextBox2.Text);
                ((RichTextBox)(tabControl1.SelectedTab.Controls[0])).SelectionColor = Color.Blue;
                ((RichTextBox)(tabControl1.SelectedTab.Controls[0])).AppendText(GlobleVar.user + ":\r\n");
                ((RichTextBox)(tabControl1.SelectedTab.Controls[0])).SelectionColor = Color.Blue;
                ((RichTextBox)(tabControl1.SelectedTab.Controls[0])).AppendText("  " + richTextBox2.Text);
                richTextBox2.Text = "";
            }
        }


        private void richTextBoxTextChanged(object sender, EventArgs e)
        {
            ((RichTextBox)sender).SelectionStart = ((RichTextBox)sender).Text.Length; //Set the current caret position at the end
            ((RichTextBox)sender).ScrollToCaret(); //Now scroll it automatically
        }

        private void tabFocus(object sender, EventArgs e)
        {
            ((TabPage)sender).Text=((TabPage)sender).Text.Replace("*", "");
        }

        //end review
        private void button6_Click(object sender, EventArgs e)
        {
            Clear_chessboard();
        }


    }
}
