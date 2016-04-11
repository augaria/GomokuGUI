using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace GomokuGUI
{
    class GlobleVar
    {
        public static bool TEST = false;


        public static DateTime startTime;
        public static int nodeCount = 0;
        public static int preValue = 0;

        public static int round = 0;

        public static int handledNode = 0;
        public static int possibleNode = 0;

        public static int[] MostSeq = new int[3] { 0, 0, 0 };
        public static int[] winChoice = new int[3] { -1, -1, -1 };

        public static int AI = 2;
        public static int PLAYER = 1;
        public static int STEPS = 3;
        public const int boardSize = 15;

        public static int EMPTY_3 = 3600;

        public static String s1 = "";
        public static String s2 = "";
        public static String s3 = "";
        public static String s4 = "";

        public static String posHistory = "";
        public static int choice = -1;

        public static System.Diagnostics.Stopwatch sw = null;

        public static TreeNode AiBestChoice = null;

        public static int[,] chessboard = new int[boardSize, boardSize];
        public static ArrayList choices = new ArrayList();

        public static string user = "";

        //public static MySql mysql;

        public static string ip = "";
        public static SocketHandler sh;

        public static string friend = "";
        public static Invitation it = null;

        public static bool playerFirst;

        public static Review rv;

        //recursively find possible positions for next step
        public static void findChoices(int choice, int[,] tempChessboard, ArrayList tempChoices)
        {
            tempChoices.Remove(choice);

            int x, y;
            x = choice / 15;
            y = choice % 15;
            if (x != 0 && y != 0 && tempChessboard[x - 1, y - 1] == 0)
            {
                tempChoices.Add((x - 1) * 15 + y - 1);
                tempChessboard[x - 1, y - 1] = -1;
            }
            if (x != 0 && tempChessboard[x - 1, y] == 0)
            {
                tempChoices.Add((x - 1) * 15 + y);
                tempChessboard[x - 1, y] = -1;
            }
            if (x != 0 && y != 14 && tempChessboard[x - 1, y + 1] == 0)
            {
                tempChoices.Add((x - 1) * 15 + y + 1);
                tempChessboard[x - 1, y + 1] = -1;
            }
            if (x != 14 && y != 0 && tempChessboard[x + 1, y - 1] == 0)
            {
                tempChoices.Add((x + 1) * 15 + y - 1);
                tempChessboard[x + 1, y - 1] = -1;
            }
            if (x != 14 && tempChessboard[x + 1, y] == 0)
            {
                tempChoices.Add((x + 1) * 15 + y);
                tempChessboard[x + 1, y] = -1;
            }
            if (x != 14 && y != 14 && tempChessboard[x + 1, y + 1] == 0)
            {
                tempChoices.Add((x + 1) * 15 + y + 1);
                tempChessboard[x + 1, y + 1] = -1;
            }
            if (y != 0 && tempChessboard[x, y - 1] == 0)
            {
                tempChoices.Add(x * 15 + y - 1);
                tempChessboard[x, y - 1] = -1;
            }
            if (y != 14 && tempChessboard[x, y + 1] == 0)
            {
                tempChoices.Add(x * 15 + y + 1);
                tempChessboard[x, y + 1] = -1;
            }

        }
    }
}
