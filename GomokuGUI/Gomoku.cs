using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


namespace GomokuGUI
{
    public class Gomoku
    {

        //static void Main(string[] args)
        //{

            //game(3600);
            /*
            double param = 1500;
            while (param < 300000)
            {
                int result = game((int)(param));
                if(result==0)
                    Console.WriteLine("param = {0} : first AI wins", (int)(param));
                else
                    Console.WriteLine("param = {0} : Second AI wins", (int)(param));
                param *= 1.1;
            }

            Console.ReadLine();
            Console.ReadLine();
            */
        //}
        static private int game(int param)
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
            GlobleVar.STEPS = 3;
            GlobleVar.choices = new ArrayList();

            GlobleVar.posHistory = "";
            int choice = -1;
            //display();

            DateTime startTime = DateTime.Now;
            
            

            while (true)
            {
                GlobleVar.sw = new System.Diagnostics.Stopwatch();

                GlobleVar.AI = 3 - GlobleVar.AI;
                GlobleVar.PLAYER = 3 - GlobleVar.PLAYER;
                GlobleVar.preValue = 0 - GlobleVar.preValue;
                GlobleVar.nodeCount = 0;
                GlobleVar.STEPS = 3;
                GlobleVar.EMPTY_3 = param;

                //choice = Agent.agent(choice);
                choice=player();
                GlobleVar.posHistory += choice.ToString() + " ";
                if (GlobleVar.MostSeq[GlobleVar.AI] >= 5)
                {
                    display();
                    //Console.WriteLine("Congratulations! You Win!\n\n");
                    //TimeSpan ts = DateTime.Now - startTime;
                    //MySql mysql = new MySql();
                    //mysql.insert(0, "QianMa", "QianMa", Convert.ToInt32(ts.TotalSeconds), GlobleVar.round, posHistory);
                    Console.ReadLine();
                    //Console.ReadLine();
                    return 0;
                }
                //display();
                //Console.ReadLine();

                GlobleVar.AI = 3 - GlobleVar.AI;
                GlobleVar.PLAYER = 3 - GlobleVar.PLAYER;
                GlobleVar.preValue = 0 - GlobleVar.preValue;
                GlobleVar.nodeCount = 0;
                GlobleVar.STEPS = 3;
                GlobleVar.EMPTY_3 = param;

                GlobleVar.sw.Start();
                choice = Agent.agent(choice);
                GlobleVar.sw.Stop();

                GlobleVar.posHistory += choice.ToString() + " ";
                if (GlobleVar.MostSeq[GlobleVar.AI] >= 5)
                {
                    display();
                    //Console.WriteLine("Unfortunately, AI Wins\n");
                    //TimeSpan ts = DateTime.Now - startTime;
                    //MySql mysql = new MySql();
                    //mysql.insert(0, "QianMa", "AI", Convert.ToInt32(ts.TotalSeconds), GlobleVar.round, posHistory);
                    Console.ReadLine();
                    //Console.ReadLine();
                    return 1;
                }

                GlobleVar.round++;

                display();

                //Console.ReadLine();

            }
        }



        static private int player()
        {
            Console.WriteLine("input position:");
            String input = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("");
            String[] axis = input.Split(' ');

            int x;
            if (axis[0][0] >= 'a' && axis[0][0] <= 'z')
                x = axis[0][0] - 'a';
            else
                x = axis[0][0] - 'A';
            int y = int.Parse(axis[1]) - 1;
            if (!(x < 15 && x >= 0 && y < 15 && y >= 0))
            {
                Console.WriteLine("Invalid input, type again.");
                Console.WriteLine();
                return player();
            }
            else
                if (GlobleVar.chessboard[x, y] == GlobleVar.PLAYER || GlobleVar.chessboard[x, y] == GlobleVar.AI)
                {
                    Console.WriteLine("Already has a piece, change a place.");
                    Console.WriteLine();
                    return player();
                }

            int choice = x * 15 + y;
            GlobleVar.chessboard[x, y] = GlobleVar.AI;
            TreeNode root = new TreeNode(choice, null);
            //root.turn = 3 - root.turn;
            root.eval(false);
            GlobleVar.findChoices(choice, GlobleVar.chessboard, GlobleVar.choices);
            GlobleVar.preValue = root.tempValue;
            return choice;
        }


        static private void display()
        {
            Console.Write("  ");
            for (int i = 0; i < GlobleVar.boardSize; i++)
            {
                if (i < 9)
                    Console.Write("  {0} ", i + 1);
                else
                    Console.Write(" {0} ", i + 1);
            }

            Console.WriteLine("");
            for (int i = 0; i < GlobleVar.boardSize; i++)
            {
                Console.Write("  ");
                for (int j = 0; j < GlobleVar.boardSize; j++)
                {
                    Console.Write("|---");
                }
                Console.WriteLine("|");
                Console.Write("{0} ", (char)('A' + i));
                for (int j = 0; j < GlobleVar.boardSize; j++)
                {
                    if (GlobleVar.chessboard[i, j] == 1)
                        Console.Write("| x ");
                    else if (GlobleVar.chessboard[i, j] == 2)
                        Console.Write("| o ");
                    //else if (GlobleVar.chessboard[i, j] == -1)
                    //Console.Write("| ? ");
                    else
                    {
                        if (GlobleVar.TEST)
                        {
                            string sTemp = "";
                            if (i * 15 + j < 10)
                                sTemp += " ";
                            if (i * 15 + j < 100)
                                sTemp += " ";
                            sTemp += (i * 15 + j).ToString();
                            Console.Write("|" + sTemp);
                        }
                        else
                            Console.Write("|   ");
                    }
                }
                Console.WriteLine("|");
            }
            Console.Write("  ");
            for (int j = 0; j < GlobleVar.boardSize; j++)
            {
                Console.Write("|---");
            }
            Console.WriteLine("|  ");
            Console.WriteLine("");

            //Console.WriteLine("");
            Console.WriteLine("");
            if (GlobleVar.round > 0)
            {
                Console.WriteLine("Player Longest: {0}", GlobleVar.MostSeq[GlobleVar.PLAYER]);
                Console.WriteLine("AI Longest: {0}", GlobleVar.MostSeq[GlobleVar.AI]);
                Console.WriteLine("AI value: {0} Layout value: {1}", GlobleVar.AiBestChoice.value, GlobleVar.AiBestChoice.tempValue);
                Console.WriteLine("sum of pieces: {0}", GlobleVar.round * 2);
                Console.WriteLine("handled node: {0}", GlobleVar.handledNode);
                Console.WriteLine("possible node: {0}", GlobleVar.possibleNode);
                Console.WriteLine("rate: {0}%", Math.Round(GlobleVar.handledNode * 100.0 / GlobleVar.possibleNode, 2));
                Console.WriteLine("agent cost: {0}ms\n", GlobleVar.sw.ElapsedMilliseconds);
            }

            GlobleVar.s1 += GlobleVar.round * 2 + " ";
            GlobleVar.s1 += GlobleVar.handledNode + " ";
            GlobleVar.s2 += GlobleVar.possibleNode + " ";
            GlobleVar.s3 += GlobleVar.sw.ElapsedMilliseconds + " ";
            Console.WriteLine(GlobleVar.s1);
            Console.WriteLine(GlobleVar.s2);
            Console.WriteLine(GlobleVar.s3);
            Console.WriteLine(GlobleVar.s4);

        }

    }
}
