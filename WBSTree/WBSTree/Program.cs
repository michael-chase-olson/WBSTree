using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using WBSTree.Serializer;
using WBSTree.Tree;

namespace WBSTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = CreateRandomTree(10, 10000);
            //var tree = CreateStaticTestTree();
            Console.WriteLine("Printing Original Tree");
           // PrintTree(tree);

            Console.WriteLine();

            var serializer = new IdTreeSerializer();

            var serialized = serializer.Serialize(tree);
            var deserialized = serializer.Deserialize(serialized);

            Console.WriteLine("Printing Tree After Serialization/Deserialization");
            //PrintTree(deserialized);

            Console.WriteLine($"Trees are the same: {tree.Equals(deserialized)}");
            Console.WriteLine($"Time with current serializer: {serializer.GetTime()}");

            var oldSerializer = new OldSerializer();
            var oldSerialized = oldSerializer.Serialize(tree);
            var oldDeserialized = oldSerializer.Deserialize(oldSerialized);
            Console.WriteLine($"Trees are the same: {tree.Equals(oldDeserialized)}");
            Console.WriteLine($"Time with old serializer: {oldSerializer.GetTime()}");

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

        private static int _idCount = 1;

        private static IdTree CreateRandomTree(int numberOfLevels, int maximumNumberOfNodes)
        {
            if (_idCount >= maximumNumberOfNodes)
                return null;

            var randomTree = new IdTree {RootNode = new IdNode {Id = _idCount++}};

            if (numberOfLevels == 0)
            {
                return randomTree;
            }

            var numberOfChildren = RandomNumber.Between(1, 5);
            var nextLevel = numberOfLevels - 1;

            for (int i = 0; i < numberOfChildren; i++)
            {
                var tree = CreateRandomTree(nextLevel, maximumNumberOfNodes);
                if (tree != null)
                {
                    tree.RootNode.Parent = randomTree.RootNode;
                    randomTree.RootNode.Children.Add(tree.RootNode);
                }
            }

            return randomTree;
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

        public static class RandomNumber
        {
            private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

            public static int Between(int minimumValue, int maximumValue)
            {
                byte[] randomNumber = new byte[1];

                _generator.GetBytes(randomNumber);

                double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);


                //We are using Math.Max, and substracting 0.00000000001,
                //to ensuer "multiplier" will always be between 0.0 and .99999999999
                //Otherwise, it's possible for it to be "1", which causes problems in our rounding
                double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

                //We need to add one to the range, to allow for the rounding done with Math.Floor
                int range = maximumValue - minimumValue + 1;

                double randomValueInRange = Math.Floor(multiplier * range);
                return (int) (minimumValue + randomValueInRange);
            }
        }
    }
}
