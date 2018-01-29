using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WBSTree.Serializer;
using WBSTree.Tree;

namespace WBSTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = CreateTestTree();
            PrintTree(tree);

            Console.WriteLine();

            var serializer = new IdTreeSerializer();

            var serialized = serializer.Serialize(tree);
            var deserialized = serializer.Deserialize(serialized);

            PrintTree(deserialized);

            Console.ReadLine();
        }


        private static void PrintTree(IdTree tree)
        {
            string indent = string.Empty;
            Print(tree.RootNode, indent, tree.RootNode.Children.Any());
        }

        private static void Print(IdNode node, string indent, bool last)
        {
            Console.Write(indent);

            if (last)
            {
                Console.Write(@"\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += " ";
            }
            Console.WriteLine(node.Id);

            for (int i = 0; i < node.Children.Count; i++)
            {
                Print(node.Children[i], indent, i == node.Children.Count - 1);
            }
        }

        private static IdTree CreateTestTree()
        {
            var tree = new IdTree();

            int idCounter = 1;
            int nodeCounter = 0;
            tree.RootNode = CreateNode(idCounter, nodeCounter, 5);

            return tree;
        }

        private static IdNode CreateNode(int idCounter, int nodeCounter, int nodeMax, IdNode parent = null)
        {
            var node = new IdNode{Id = idCounter++};

            if (parent != null)
                node.Parent = parent;

            if (nodeCounter == nodeMax)
                return node;

            nodeCounter++;
            var random = new Random();
            var childrenNodes = random.Next(0, 3);
            for (int i = 0; i < childrenNodes; i++)
            {
                node.Children.Add(CreateNode(idCounter, nodeCounter, nodeMax, node));
            }

            return node;
        }

        private static IdTree CreateStaticTestTree()
        {
            var tree = new IdTree();

            var rootNode = new IdNode {Id = 1};

            var secondNode = new IdNode {Id = 2, Parent = rootNode};
            secondNode.Children = new List<IdNode>
            {
                new IdNode{Id = 5, Parent = secondNode},
                new IdNode{Id = 6, Parent = secondNode},
                new IdNode{Id = 7, Parent = secondNode}
            };

            rootNode.Children.Add(secondNode);

            var thirdNode = new IdNode { Id = 3, Parent = rootNode };
            thirdNode.Children = new List<IdNode>
            {
                new IdNode{Id = 8, Parent = thirdNode},
                new IdNode{Id = 9, Parent = thirdNode},
            };

            rootNode.Children.Add(thirdNode);

            var fourthNode = new IdNode { Id = 4, Parent = rootNode };
            fourthNode.Children = new List<IdNode>
            {
                new IdNode{Id = 10, Parent = fourthNode},
                new IdNode{Id = 11, Parent = fourthNode},
            };

            rootNode.Children.Add(fourthNode);

            tree.RootNode = rootNode;

            return tree;
        }
    }
}
