using System;
using System.Collections.Generic;
using System.Linq;
using WBSTree.Serializer;
using WBSTree.Tree;

namespace WBSTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = CreateStaticTestTree();
            Console.WriteLine("Printing Original Tree");
            PrintTree(tree);

            Console.WriteLine();

            var serializer = new IdTreeSerializer();

            var serialized = serializer.Serialize(tree);
            var deserialized = serializer.Deserialize(serialized);

            Console.WriteLine("Printing Tree After Serialization/Deserialization");
            PrintTree(deserialized);

            Console.WriteLine($"Time: {serializer.GetTime()}");

            Console.ReadLine();
        }


        private static void PrintTree(IdTree tree)
        {
            string indent = string.Empty;
            Print(tree.RootNode, indent, tree.RootNode.Children.Any());
        }

        private static void Print(IdNode node, string printString, bool last)
        {
            Console.Write(printString);

            if (last)
            {
                Console.Write(@"\-");
                printString += "  ";
            }
            else
            {
                Console.Write("|-");
                printString += " ";
            }
            Console.WriteLine(node.Id);

            for (int i = 0; i < node.Children.Count; i++)
            {
                Print(node.Children[i], printString, i == node.Children.Count - 1);
            }
        }

        private static IdTree CreateStaticTestTree()
        {
            var tree = new IdTree();

            var rootNode = new IdNode { Id = 1 };

            var secondNode = new IdNode { Id = 2, Parent = rootNode };
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

            var forthNodeChildWithChildren = new IdNode { Id = 12, Parent = fourthNode };
            forthNodeChildWithChildren.Children = new List<IdNode>
            {
                new IdNode{Id = 13, Parent = forthNodeChildWithChildren},
                new IdNode{Id = 14, Parent = forthNodeChildWithChildren}
            };
            fourthNode.Children.Add(forthNodeChildWithChildren);

            rootNode.Children.Add(fourthNode);

            rootNode.Children.Add(new IdNode { Id = 15, Parent = rootNode });

            tree.RootNode = rootNode;

            return tree;
        }
    }
}
