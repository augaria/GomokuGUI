using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuGUI
{
    public class TreeNode
    {
        public TreeNode parent;
        public TreeNode next;
        public ArrayList children;
        public int turn;
        public bool valueNotEmpty;
        public int tempValue;
        public int value;
        public int depth;
        public bool expanded;
        //public int tempChessboard[][];
        public ArrayList tempChoices;
        public int[] choiceHistory;
        public int choice;

        public TreeNode bestChoice;


        public int seq;

        public int id;

        public int winningPoint;
        public int possibleWinP;

        //variables or struct used for eval

        public TreeNode(int choice, TreeNode parent)
        {
            //this.choice=choice;
            this.parent = parent;
            this.next = null;

            this.expanded = false;
            this.children = new ArrayList();
            this.tempChoices = new ArrayList();

            this.valueNotEmpty = false;
            this.bestChoice = null;

            this.choice = choice;
            this.seq = 0;
            this.possibleWinP = 0;
            this.winningPoint = 0;

            GlobleVar.nodeCount++;
            id = GlobleVar.nodeCount;

            if (parent == null)
            {
                this.turn = GlobleVar.AI;
                this.depth = 0;
                //this.tempChessboard=chessboard.clone();
                this.tempChoices = (ArrayList)(GlobleVar.choices.Clone());
                this.choiceHistory = new int[GlobleVar.STEPS];
                for (int i = 0; i < GlobleVar.STEPS; i++)
                    this.choiceHistory[i] = -1;
                tempValue = GlobleVar.preValue;
            }

            else
            {
                parent.children.Add(this);
                this.turn = 3 - parent.turn;
                this.depth = parent.depth + 1;
                this.choiceHistory = new int[GlobleVar.STEPS];
                for (int i = 0; i < GlobleVar.STEPS; i++)
                    this.choiceHistory[i] = parent.choiceHistory[i];
                this.choiceHistory[depth - 1] = choice;


                //this.tempChessboard=parent.tempChessboard.clone();
                //this.tempChessboard[choice/15][choice%15]=AI;

                this.tempChoices = (ArrayList)parent.tempChoices.Clone();
                //findChoices(choice, this.tempChessboard, this.tempChoices);

                eval(true);
            }


        }

        //evaluation function
        public void eval(bool reverse)
        {
            int[,] tempChessboard = (int[,])(GlobleVar.chessboard.Clone());

            int i;

            //display();

            //update tempChessboard only used for eval() function

            for (i = 0; i < depth; i++)
            {
                if (GlobleVar.AI == 2)
                    tempChessboard[choiceHistory[i] / 15, choiceHistory[i] % 15] = 2 - (i % 2);
                else
                    tempChessboard[choiceHistory[i] / 15, choiceHistory[i] % 15] = 1 + (i % 2);
            }

            //display();

            //update tempChoices
            GlobleVar.findChoices(choice, tempChessboard, tempChoices);

            int[] left = new int[3];
            int[] right = new int[3];
            int[] all = new int[3];

            if (parent != null)
                tempValue = parent.tempValue;

            int maxSeq = 0;
            for (int direction = 0; direction < 4; direction++)
            {
                all[0] = 1;
                all[1] = 0;
                all[2] = GlobleVar.PLAYER;
                left = layout(direction, choice, tempChessboard, reverse);
                right = layout(direction + 4, choice, tempChessboard, reverse);
                tempValue -= valueMap(left);
                tempValue -= valueMap(right);

                if (left[2] == 0)
                    all[1]++;
                else if (left[2] == GlobleVar.AI)
                {
                    all[1]++;
                    left[1]++;
                    tempValue += valueMap(left);
                }
                else
                {
                    all[0] += left[0];
                    all[1] += left[1];
                }
                if (right[2] == 0)
                    all[1]++;
                else if (right[2] == GlobleVar.AI)
                {
                    all[1]++;
                    right[1]++;
                    tempValue += valueMap(right);
                }
                else
                {
                    all[0] += right[0];
                    all[1] += right[1];
                }

                if (all[0] > maxSeq)
                    maxSeq = all[0];

                int newValue = valueMap(all);

                if (newValue == 500000 || newValue == -500000)
                {
                    tempValue = newValue;
                    possibleWinP = 1;
                }
                else if (newValue == 1000000 || newValue == -1000000)
                {
                    tempValue = newValue;
                    winningPoint = 1;
                    if (parent!=null&&parent.possibleWinP == 1)
                        parent.possibleWinP = 0;
                    break;
                }
                else
                    tempValue += newValue;
            }


            if (parent == null && maxSeq > GlobleVar.MostSeq[GlobleVar.AI])
                GlobleVar.MostSeq[GlobleVar.AI] = maxSeq;

            if (depth == 1)
            {
                seq = maxSeq;
                if (maxSeq >= 5)
                {
                    GlobleVar.winChoice[GlobleVar.AI] = choice;
                    GlobleVar.MostSeq[GlobleVar.AI] = maxSeq;
                }
            }

        }

        private int valueMap(int[] layoutValue)
        {
            int evalValue;
            switch (layoutValue[0])
            {
                case 0:
                    evalValue = 0;
                    break;
                case 1:
                    //-o-
                    if (layoutValue[1] == 0)
                        evalValue = 50;
                    //-ox or xo-
                    else if (layoutValue[1] == 1)
                        evalValue = 30;
                    //xox
                    else
                        evalValue = 10;
                    break;
                case 2:
                    //-oo-
                    if (layoutValue[1] == 0)
                        evalValue = 1000;
                    //-oox or xoo-
                    else if (layoutValue[1] == 1)
                        evalValue = 100;
                    //xoox
                    else
                        evalValue = 10;
                    break;
                case 3:
                    //-ooo-
                    if (layoutValue[1] == 0)
                        evalValue = GlobleVar.EMPTY_3;
                    //-ooox or xooo-
                    else if (layoutValue[1] == 1)
                        evalValue = 500;
                    //xooox
                    else
                        evalValue = 10;
                    break;
                case 4:
                    //-oooo-
                    if (layoutValue[1] == 0)
                        evalValue = 500000;
                    //-oooox or xoooo-
                    else if (layoutValue[1] == 1)
                        evalValue = 50000;
                    //xoooox
                    else
                        evalValue = 10;
                    break;
                //ooooo or more
                default:
                    evalValue = 1000000;
                    break;

            }
            if (layoutValue[2] == GlobleVar.AI)
                evalValue = 0 - evalValue;
            if (turn == GlobleVar.AI)
                evalValue = 0 - evalValue;
            return evalValue;
        }

        //  number used for indicating direction
        //    \  |  /
        //     2 1 7
        //  ---0 x 4 ---
        //     3 5 6 
        //    /  |  \

        //return x means having x sequential pieces ended by an empty position on the given direction
        //return -x means having x sequential pieces ended by a opponent's piece or edge on the given direction
        private int[] layout(int direction, int position, int[,] tempChessboard, bool reverse)
        {
            int nextPosition = position;
            int[] layoutValue = new int[3];

            layoutValue[0] = 0;
            layoutValue[1] = 0;
            layoutValue[2] = GlobleVar.PLAYER;

            int tempTurn;
            if (reverse)
                tempTurn = 3 - turn;
            else
                tempTurn = turn;

            nextPosition = nextOnDirection(direction, nextPosition);
            if (nextPosition == -1)
            {
                layoutValue[1] = 1;
                layoutValue[2] = 0;
                return layoutValue;
            }

            if (tempChessboard[nextPosition / 15, nextPosition % 15] == 0 || tempChessboard[nextPosition / 15, nextPosition % 15] == -1)
                return layoutValue;

            layoutValue[0]++;
            if (tempChessboard[nextPosition / 15, nextPosition % 15] == 3 - tempTurn)
            {
                layoutValue[2] = GlobleVar.AI;
                tempTurn = 3 - tempTurn;
            }


            while (true)
            {
                nextPosition = nextOnDirection(direction, nextPosition);

                //opponent's piece or meeting edge
                if (nextPosition == -1 || tempChessboard[nextPosition / 15, nextPosition % 15] == 3 - tempTurn)
                {
                    layoutValue[1] = 1;
                    break;
                }

                //own piece
                if (tempChessboard[nextPosition / 15, nextPosition % 15] == tempTurn)
                    layoutValue[0]++;

                //empty position
                else
                    break;
            }
            return layoutValue;
        }

        //find next position on the given direction
        private int nextOnDirection(int direction, int position)
        {
            int nextPosition;
            switch (direction)
            {
                case 0:
                    if (position % 15 == 0)
                        nextPosition = -1;
                    else
                        nextPosition = position - 1;
                    break;
                case 1:
                    if (position < 15)
                        nextPosition = -1;
                    else
                        nextPosition = position - 15;
                    break;
                case 2:
                    if (position < 15 || position % 15 == 0)
                        nextPosition = -1;
                    else
                        nextPosition = position - 16;
                    break;
                case 3:
                    if (position >= 210 || position % 15 == 0)
                        nextPosition = -1;
                    else
                        nextPosition = position + 14;
                    break;
                case 4:
                    if (position % 15 == 14)
                        nextPosition = -1;
                    else
                        nextPosition = position + 1;
                    break;
                case 5:
                    if (position >= 210)
                        nextPosition = -1;
                    else
                        nextPosition = position + 15;
                    break;
                case 6:
                    if (position % 15 == 14 || position >= 210)
                        nextPosition = -1;
                    else
                        nextPosition = position + 16;
                    break;
                case 7:
                    if (position % 15 == 14 || position < 15)
                        nextPosition = -1;
                    else
                        nextPosition = position - 14;
                    break;
                default:
                    nextPosition = -1;
                    break;

            }
            return nextPosition;
        }

    }
}
