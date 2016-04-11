using System;


using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace GomokuGUI
{
    class SocketHandler
    {

        IPAddress server_ip;

        TcpClient socket5500;
        TcpClient socket5501;

        StreamReader sr5501;
        StreamWriter sw5501;

        StreamReader sr5500;
        StreamWriter sw5500;

        //TcpListener serverSocket;

        public SocketHandler() //constructor, set up connection to the server
        {
            server_ip = IPAddress.Parse("*************");
            try
            {
                //setup client socket
                socket5500 = new TcpClient();
                socket5500.Connect(new IPEndPoint(server_ip, 5500)); //port 5500 for online & offline notice

                NetworkStream ns = socket5500.GetStream();
                sr5500 = new StreamReader(ns);
                sw5500 = new StreamWriter(ns); 
            }
            catch
            {
                throw;
            }
        }

        public bool start(String usr, String pwd) //register to the server and fetch the online friend list
        {
            sw5500.WriteLine(usr);
            sw5500.WriteLine(pwd);
            sw5500.Flush();

            GlobleVar.ip = sr5500.ReadLine();

            if (GlobleVar.ip.Equals("failed"))
                return false;

            socket5501 = new TcpClient();
            socket5501.Connect(new IPEndPoint(server_ip, 5501));

            NetworkStream ns = socket5501.GetStream();
            sr5501 = new StreamReader(ns);
            sw5501 = new StreamWriter(ns);

            sw5501.WriteLine(usr);
            sw5501.Flush();

            Program.form1.listBox1.Items.Add(usr);
            string tempPlayer;
            while (!(tempPlayer = sr5500.ReadLine()).Equals("end"))
                Program.form1.listBox1.Items.Add(tempPlayer);

            Thread myThread = new Thread(Listening);
            myThread.Start();
            
            

            return true;
        }

        //port 5501
        private void Listening() //listening from the server
        {
            try
            {
                while (true)
                {
                    string change = sr5501.ReadLine();

                    if (change.Equals("stop"))
                    {
                        Program.form1.listBox1.Items.Clear();
                        Program.form1.radioButton1.Checked = false;
                        Program.form1.radioButton2.Checked = true;
                        return;
                    }
                    else if (change.Equals("on"))
                        Program.form1.listBox1.Items.Add(sr5501.ReadLine());
                    else if(change.Equals("off"))
                        Program.form1.listBox1.Items.Remove(sr5501.ReadLine());
                    else if (change.Equals("position"))
                        Program.form1.friendMove(int.Parse(sr5501.ReadLine()));
                    else if (change.Equals("request"))
                    {
                        GlobleVar.friend= sr5501.ReadLine();
                        DialogResult result=MessageBox.Show(GlobleVar.friend+" invites you for a game.", "Challenge", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            sw5500.WriteLine("confirmB");
                            sw5500.Flush();
                        }
                        else
                        {
                            sw5500.WriteLine("deny");
                            sw5500.Flush();
                            GlobleVar.friend = "";
                        }
                    }
                    else if (change.Equals("notfound"))
                    {
                        GlobleVar.it.notfound();
                    }
                    else if (change.Equals("deny"))
                    {
                        GlobleVar.it.denied();
                    }
                    else if (change.Equals("confirmB"))
                    {
                        GlobleVar.it.accepted();
                    }
                    else if (change.Equals("whoFirst"))
                    {
                        WhoFirst wf = new WhoFirst();
                        wf.ShowDialog();
                    }
                    else if (change.Equals("pkStartA"))
                    {
                        String result ="";
                        int myNum = int.Parse(sr5501.ReadLine());
                        int friendNum = int.Parse(sr5501.ReadLine());
                        if(friendNum>100)
                        {
                            result += "Both claim first.\r\n";
                            result += "Your random number is " + (myNum - 100).ToString() + "\r\n";
                            result += "Your friend's random number is " + (friendNum - 100).ToString() + "\r\n";
                            result += "You first.";
                        }
                        else if (friendNum == 0)
                        {
                            result += "Your friend give up first moving. You first.";
                        }
                        else if (myNum>100)
                        {
                            result += "You claim first and your friend not. You first.";
                        }
                        else
                        {
                            result += "Your random number is " + myNum.ToString() + "\r\n";
                            result += "Your friend's random number is " + friendNum.ToString() + "\r\n";
                            result += "You first.";
                        }
                        DialogResult drResult = MessageBox.Show(result, "who first", MessageBoxButtons.OK);
                        if (drResult == DialogResult.OK)
                            Program.form1.newPvP(true);
                    }
                    else if (change.Equals("pkStartB"))
                    {
                        String result = "";
                        int myNum = int.Parse(sr5501.ReadLine());
                        int friendNum = int.Parse(sr5501.ReadLine());
                        if (myNum > 100)
                        {
                            result += "Both claim first.\r\n";
                            result += "Your random number is " + (myNum - 100).ToString() + "\r\n";
                            result += "Your friend's random number is " + (friendNum - 100).ToString() + "\r\n";
                            result += "Your friend first.";
                        }
                        else if (myNum == 0)
                        {
                            result += "You give up first moving. Your friend first.";
                        }
                        else if (friendNum > 100)
                        {
                            result += "Your friend claims first and you not. Your friend first.";
                        }
                        else
                        {
                            result += "Your random number is " + myNum.ToString() + "\r\n";
                            result += "Your friend's random number is " + friendNum.ToString() + "\r\n";
                            result += "Your friend first.";
                        }
                        DialogResult drResult = MessageBox.Show(result, "who first", MessageBoxButtons.OK);
                        if (drResult == DialogResult.OK)
                            Program.form1.newPvP(false);
                    }
                    else if (change.Equals("chat"))
                    {
                        string name = sr5501.ReadLine();
                        string message = sr5501.ReadLine();
                        Program.form1.addMessage(name, message);
                        
                    }
                    else if (change.Equals("newChat"))
                    {
                        string name = sr5501.ReadLine();
                        Program.form1.addChat(name);
                    }
                    else if (change.Equals("endChat"))
                    {
                        string name = sr5501.ReadLine();
                        Program.form1.endChat(name);
                    }
                }
            }
            catch
            {
            }
        }


        //port 5500
        public void sendMessage(string message)
        {
            sw5500.WriteLine(message);
            sw5500.Flush();
        }

        //port 5500
        public void sendMessage(string message1, string message2)
        {
            sw5500.WriteLine(message1);
            sw5500.WriteLine(message2);
            sw5500.Flush();
        }

        //port 5500
        public void sendMessage(string message1, string message2, string message3)
        {
            sw5500.WriteLine(message1);
            sw5500.WriteLine(message2);
            sw5500.WriteLine(message3);
            sw5500.Flush();
        }

        //port 5500
        public string sendMessageR(string message)
        {
            sw5500.WriteLine(message);
            sw5500.Flush();
            string result = sr5500.ReadLine();
            return result;
        }

        //port 5500
        public string sendMessageR(string message1, string message2)
        {
            sw5500.WriteLine(message1);
            sw5500.WriteLine(message2);
            sw5500.Flush();
            string result = sr5500.ReadLine();
            return result;
        }

        //port 5500
        public string sendMessageR(string message1,string message2,string message3)
        {
            sw5500.WriteLine(message1);
            sw5500.WriteLine(message2);
            sw5500.WriteLine(message3);
            sw5500.Flush();
            string result = sr5500.ReadLine();
            return result;
        }

        //port 5500
        public void record(int level, string first, string second, string winner, int duration, int round, string pos)
        {
            sw5500.WriteLine("record");
            sw5500.WriteLine(level);
            sw5500.WriteLine(first);
            sw5500.WriteLine(second);
            sw5500.WriteLine(winner);
            sw5500.WriteLine(duration);
            sw5500.WriteLine(round);
            sw5500.WriteLine(pos);
            sw5500.Flush();
        }

        


        public void stop() //stop the socket
        {
            sr5501.Close();
            sw5501.Close();
            sr5500.Close();
            sw5500.Close();
            socket5500.Close();
            socket5501.Close();
        }

    }
}
