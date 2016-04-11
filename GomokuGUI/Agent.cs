using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace GomokuGUI
{
    class Agent
    {
        //used for desc sorting
        public class tnMax : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                return ((TreeNode)y).tempValue - ((TreeNode)x).tempValue;
            }

        }

        //used for asc sorting
        public class tnMin : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                return ((TreeNode)x).tempValue - ((TreeNode)y).tempValue;
            }

        }

        static public int agent(int playerChoice)
        {
            /*
            if (GlobleVar.round == 3 && GlobleVar.AI == 1)
                GlobleVar.TEST = true;
            else
                GlobleVar.TEST = false;
            */

            if (playerChoice == -1)
            {
                GlobleVar.chessboard[7, 7] = GlobleVar.AI;
                GlobleVar.preValue = 200;
                GlobleVar.MostSeq[GlobleVar.AI] = 1;
                GlobleVar.AiBestChoice = null;
                GlobleVar.findChoices(112, GlobleVar.chessboard, GlobleVar.choices);
                return 112;
            }

            GlobleVar.handledNode = 0;
            GlobleVar.possibleNode = 0;

            ArrayList tempChoices = new ArrayList();
            tempChoices = (ArrayList)(GlobleVar.choices.Clone());
            GlobleVar.possibleNode = (GlobleVar.choices.Count) * (GlobleVar.choices.Count + 3) * (GlobleVar.choices.Count + 6);
            Stack<TreeNode> st = new Stack<TreeNode>();
            TreeNode root = new TreeNode(playerChoice, null);

            TreeNode temp;

            st.Push(root);


            while (st.Count != 0)
            {
                temp = st.Peek();


                if (GlobleVar.TEST)
                {
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("NEW LOOP:\n");
                    Console.WriteLine("Stack:");
                    foreach (TreeNode stackTemp in st)
                    {
                        if (stackTemp.turn == GlobleVar.AI)
                            Console.Write("choice={0}, Max, ", stackTemp.choice);
                        else
                            Console.Write("choice={0}, Min, ", stackTemp.choice);
                        if (stackTemp.valueNotEmpty)
                            Console.Write(stackTemp.value);
                        //Console.WriteLine("tempV={0} value={1} children:", stackTemp.tempValue, stackTemp.value);
                        //for (int j = 0; j < stackTemp.children.Count; j++)
                        //  Console.Write("{0}({1}),", ((TreeNode)(stackTemp.children[j])).choice,((TreeNode)(stackTemp.children[j])).tempValue);
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                }

                if (temp.depth == GlobleVar.STEPS || temp.winningPoint == 1)
                    temp.value = temp.tempValue;


                //if temp node has been expanded or it's a leaf node
                //which means value for this node has been set
                //check if the value of it's parent need to be updated
                //take alpha-beta pruning
                if (temp.expanded || temp.depth == GlobleVar.STEPS || temp.winningPoint == 1)
                {

                    GlobleVar.handledNode++;
                    if (GlobleVar.TEST)
                    {
                        Console.WriteLine("handling:");
                        if (temp.parent == null)
                            Console.WriteLine("choice={0} depth={1} parent={2} tempValue={3} value={4}\n", temp.choice, temp.depth, 0, temp.tempValue, temp.value);
                        else
                            Console.WriteLine("choice={0} depth={1} parent={2} tempValue={3} value={4}\n", temp.choice, temp.depth, temp.parent.id, temp.tempValue, temp.value);
                    }
                    st.Pop();

                    if (temp.parent == null)
                        break;

                    bool parentValueAltered = false;


                    //check if the value of it's parent need to be updated
                    if (temp.parent.valueNotEmpty == false)
                    {
                        temp.parent.value = temp.value;
                        temp.parent.valueNotEmpty = true;
                        parentValueAltered = true;

                        if (GlobleVar.TEST)
                        {
                            Console.WriteLine("update parent value\n");
                        }
                        temp.parent.bestChoice = temp;
                    }
                    else if (temp.turn == GlobleVar.AI)
                    {
                        if (temp.value < temp.parent.value)
                        {
                            temp.parent.value = temp.value;
                            parentValueAltered = true;
                            if (GlobleVar.TEST)
                            {
                                Console.WriteLine("update parent value\n");
                            }
                        }
                    }
                    else
                    {
                        if (temp.value > temp.parent.value)
                        {
                            temp.parent.value = temp.value;
                            parentValueAltered = true;
                            if (GlobleVar.TEST)
                            {
                                Console.WriteLine("update parent value\n");
                            }
                            //update the best choice for AI's turn
                            temp.parent.bestChoice = temp;
                        }
                    }


                    //if temp.parent.parent and temp.parent.value has been altered, try pruning

                    if (temp.depth > 1 && temp.parent.parent.valueNotEmpty && parentValueAltered)
                    {
                        if (temp.turn == GlobleVar.AI)
                        {
                            //alpha pruning
                            //prune the subtree rooted from it's parent
                            //push it's parent's next into stack
                            if (temp.parent.value <= temp.parent.parent.value)
                            {
                                st.Pop();
                                if (temp.parent.next != null)
                                    st.Push(temp.parent.next);
                                if (GlobleVar.TEST)
                                {
                                    Console.WriteLine("alpha pruning\n");
                                }
                            }
                            else if (temp.next != null)
                                st.Push(temp.next);

                        }

                        else
                        {
                            //beta pruning
                            //prune the subtree rooted from it's parent
                            //push it's parent's next into stack
                            if (temp.parent.value >= temp.parent.parent.value)
                            {
                                st.Pop();
                                if (temp.parent.next != null)
                                    st.Push(temp.parent.next);
                                if (GlobleVar.TEST)
                                {
                                    Console.WriteLine("beta pruning\n");
                                }
                            }
                            else if (temp.next != null)
                                st.Push(temp.next);
                        }
                    }

                    //push next sibling into stack 
                    else if (temp.next != null)
                        st.Push(temp.next);

                    //Console.WriteLine("\n");

                }

                //if temp node has not been expanded
                //create all children and put first child into stack
                else
                {
                    temp.expanded = true;


                    ArrayList children = new ArrayList();
                    //use the possible choices to create children 
                    for (int i = 0; i < temp.tempChoices.Count; i++)
                        children.Add(new TreeNode((int)(temp.tempChoices[i]), temp));

                    if (temp.possibleWinP == 1)
                    {
                        if (temp.parent == null)
                            return -1;
                        temp.tempValue *= 2;
                        temp.winningPoint = 1;
                        continue;
                    }
                    if (GlobleVar.winChoice[GlobleVar.AI] > -1)
                    {
                        GlobleVar.chessboard[GlobleVar.winChoice[GlobleVar.AI] / 15, GlobleVar.winChoice[GlobleVar.AI] % 15] = GlobleVar.AI;
                        return GlobleVar.winChoice[GlobleVar.AI];
                    }

                    //sort children
                    tnMax tnMaxCompare = new tnMax();
                    tnMin tnMinCompare = new tnMin();
                    if (temp.turn == GlobleVar.AI)
                        children.Sort(tnMaxCompare);
                    else
                        children.Sort(tnMinCompare);

                    TreeNode tempChild;
                    TreeNode pre = null;


                    if (GlobleVar.TEST)
                    {
                        Console.WriteLine();
                        Console.WriteLine("expand:");
                        Console.Write(temp.choice);
                        if (temp.turn == GlobleVar.AI)
                            Console.Write("(Max): ");
                        else
                            Console.Write("(Min): ");
                        for (int i = 0; i < children.Count; i++)
                            Console.Write("{0}({1}),", ((TreeNode)(children[i])).choice, ((TreeNode)(children[i])).tempValue);
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                    }

                    //put the first child into stack
                    if (children.Count > 0)
                    {
                        tempChild = (TreeNode)(children[0]);
                        st.Push(tempChild);
                        pre = tempChild;
                    }

                    //link all the children by a chain
                    if (children.Count > 1)
                    {
                        for (int i = 0; i < children.Count; i++)
                        {
                            tempChild = (TreeNode)(children[i]);
                            pre.next = tempChild;
                            pre = tempChild;
                        }
                    }
                }

            }



            GlobleVar.chessboard[root.bestChoice.choice / 15, root.bestChoice.choice % 15] = GlobleVar.AI;
            GlobleVar.preValue = root.bestChoice.tempValue;
            if (root.bestChoice.seq > GlobleVar.MostSeq[GlobleVar.AI])
                GlobleVar.MostSeq[GlobleVar.AI] = root.bestChoice.seq;
            GlobleVar.AiBestChoice = root.bestChoice;
            GlobleVar.findChoices(root.bestChoice.choice, GlobleVar.chessboard, GlobleVar.choices);
            return root.bestChoice.choice;
        }
    }
}
