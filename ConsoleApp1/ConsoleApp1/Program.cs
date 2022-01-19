using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Branch
    {

        public int brValue { get; set; }
        public Branch L = null;
        public Branch R = null;
        public Branch parent = null;

        public void GenerateTreeBranchM(Branch root)
        {
            Console.WriteLine("Value:\n");
            int line = int.Parse(Console.ReadLine());
            GenerateTreeBranch(root, line);
        }
        public void GenerateTreeBranch(Branch cBr, int arrV)
        {
            Branch branch = new Branch();
            Branch leaf = null;
            int line = arrV;
            branch.brValue = line;

            if (cBr.brValue > line)
            {
                if (cBr.L == null)
                {
                    cBr.L = branch;
                    cBr.L.parent = cBr;
                }
                else
                {
                    leaf = cBr.L;
                }
            }
            else
            {
                if (cBr.R == null)
                {
                    cBr.R = branch;
                    cBr.R.parent = cBr;
                }
                else
                {
                    leaf = cBr.R;
                }
            }
            while (leaf != null)
            {
                if (leaf.brValue > line)
                {
                    if (leaf.L != null)
                    {
                        leaf = leaf.L;
                    }
                    else { leaf.L = branch; leaf.L.parent = leaf; break; }
                }
                else
                {
                    if (leaf.R != null)
                    {
                        leaf = leaf.R;
                    }
                    else { leaf.R = branch; leaf.R.parent = leaf; break; }
                }
            }
        }
        public void Shirina(Branch root)
        {
            Queue<Branch> queue = new Queue<Branch>();
            queue.Enqueue(root);
            while (queue.Count != 0)
            {
                if (queue.Peek().L != null) queue.Enqueue(queue.Peek().L);
                if (queue.Peek().R != null) queue.Enqueue(queue.Peek().R);
                Console.WriteLine(queue.Peek().brValue);
                queue.Dequeue();
            }
        }
        public void Glubina(Branch root)
        {
            if(root != null)
            {
                Console.WriteLine(root.brValue);
                Glubina(root.L);
                Glubina(root.R);
            }
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            Branch root = new Branch();
            Console.WriteLine("Manual?(1/0)");
            if (Console.ReadLine() == "1")
            {
                ConsoleKey key = ConsoleKey.Enter;
                Console.WriteLine("Root:\n");
                string line = Console.ReadLine();
                root.brValue = int.Parse(line);
                while (key != ConsoleKey.Escape)
                {
                    Console.WriteLine("(ESC to exit)\n");
                    key = Console.ReadKey().Key;
                    if (key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    root.GenerateTreeBranchM(root);
                }
            }
            else
            {
                int[] arr = {50, 30, 55, 54, 20, 31};
                for (int i = 0; i < arr.Length; i++)
                {
                    root.GenerateTreeBranch(root, arr[i]);
                }
            }
            Console.WriteLine("gGLUBINA \n");
            root.Glubina(root);
            Console.WriteLine("SHIRINA \n");
            root.Shirina(root);
        }
    }
}
