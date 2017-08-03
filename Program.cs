using System;
using System.Collections.Generic;

namespace ConsoleApplication5
{
    internal class TreeNode
    {
        public TreeNode Left;
        public TreeNode Right;

        public char Value;
        public char Operation;
    }

    internal static class Program
    {
        private static void Main()
        {
            const string expr = "(A+B)*(C+D)-E";

            const string expr2 = "AB+CD+*E-";

            var expr3 = "((((A)+(B))*((C)+(D)))-(E))";

            GenerateRpn(expr);

            Console.WriteLine("------------------------------");
            Console.WriteLine(" ");

            PrintTree(TreeBuild(expr2));

            Console.WriteLine(" ");
            Console.WriteLine("------------------------------");
            Console.WriteLine(" ");

            Lrr(TreeBuild(expr2));
            Console.WriteLine(" ");
            Console.WriteLine("------------------------------");

            Infix(TreeBuild(expr2));
            Console.WriteLine(" ");
            Console.WriteLine("------------------------------");

            GenerateRpn(expr3);
            Console.WriteLine(" ");
            Console.WriteLine("------------------------------");

            Console.WriteLine(" ");

            Console.ReadLine();
        }

        private static TreeNode TreeBuild(string rpn)
        {
            var stack = new Stack<TreeNode>();

            foreach (var ch in rpn)
            {
                if (IsOp(ch))
                {
                    var opRight = stack.Pop();
                    var opLeft = stack.Pop();

                    stack.Push(new TreeNode { Left = opLeft, Right = opRight, Value = '0', Operation = ch });
                }
                else
                {
                    stack.Push(new TreeNode { Left = null, Right = null, Value = ch, Operation = '0' });
                }
            }

            return stack.Peek();
        }

        private static void PrintTree(TreeNode root, int level = 0)
        {
            if (root == null)
            {
                return;
            }

            PrintTree(root.Right, level + 1);

            for (var i = 0; i < level; ++i)
            {
                Console.Write("    ");    
            }

            Console.WriteLine(root.Value != '0' ? root.Value : root.Operation);

            PrintTree(root.Left, level + 1); 
        }

        private static void Lrr(TreeNode root)
        {
            if (root == null)
            {
                return;
            }

            Lrr(root.Left);
            Lrr(root.Right);

            Console.Write(root.Value != '0' ? root.Value : root.Operation);
        }


        private static void Infix(TreeNode root)
        {
            if (root == null)
            {
                return;
            }

            Console.Write("(");
            Infix(root.Left);
            Console.Write(root.Value != '0' ? root.Value.ToString() : $" {root.Operation} ");
            Infix(root.Right);

            Console.Write(")");
        }

        static void GenerateRpn(string expr)
        {
            var stack = new Stack<char>();

            foreach (var ch in expr)
            {
                if (ch == ' ')
                {
                    continue;
                }

                if (IsBracket(ch))
                {
                    if (ch == '(')
                    {
                        stack.Push(ch);
                    }
                    else
                    {
                        while (stack.Peek() != '(')
                        {
                            Console.Write(stack.Pop());
                        }

                        stack.Pop();
                    }
                }
                else if (IsOp(ch))
                {
                    if (stack.Count != 0)
                    {
                        while (stack.Count > 0)
                        {
                            if (IsOpHigh(stack.Peek()) >= IsOpHigh(ch) || stack.Peek() != '(')
                            {
                                Console.Write(stack.Pop());
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    stack.Push(ch);
                }

                else
                {
                    Console.Write(ch);
                }
            }

            while (stack.Count > 0)
            {
                Console.Write(stack.Pop());
            }

            Console.WriteLine("");
        }

        private static int IsOpHigh(char op)
        {
            return op == '(' ? 0
                : (op == ')' ? 1 
                : (op == '+' || op == '-' ? 2 
                : (op == '*' || op == '/' ? 3 
                : (op == '^' ? 4 
                : -1))));
        }

        private static bool IsOp(char c)
        {
            return c == '*' || c == '/' || c == '+' || c == '-' || c == '^';
        }

        private static bool IsBracket(char c)
        {
            return c == '(' || c == ')';
        }
    }
}
