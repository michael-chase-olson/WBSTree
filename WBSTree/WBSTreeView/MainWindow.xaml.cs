using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WBSTreeView.Serializer;
using WBSTreeView.Tree;

namespace WBSTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var tree = CreateStaticTestTree();

            var sb = new StringBuilder();
            sb.Append("Printing Original Tree \n");

            
            sb.Append(PrintTree(tree));
            sb.Append("\n");

            

            var serializer = new IdTreeSerializer();

            var serialized = serializer.Serialize(tree);
            var deserialized = serializer.Deserialize(serialized);

            sb.Append("Printing Tree After Serialization/Deserialization\n");
            sb.Append(PrintTree(deserialized));

            Tree.Text = sb.ToString();
        }

        private static string PrintTree(IdTree tree)
        {
            string indent = string.Empty;
            return Print(tree.RootNode, indent, tree.RootNode.Children.Any()).ToString();
        }

        private static StringBuilder Print(IdNode node, string printString, bool last)
        {
            var sb = new StringBuilder();
            sb.Append(printString);
            //Console.Write(printString);

            if (last)
            {
                //Console.Write(@"\-");
                sb.Append(@"\-");
                printString += "  ";
            }
            else
            {
                //Console.Write("|-");
                sb.Append("|-");
                printString += " ";
            }
            //Console.WriteLine(node.Id);
            sb.Append($"{node.Id}\n");

            for (int i = 0; i < node.Children.Count; i++)
            {
                sb.Append(Print(node.Children[i], printString, i == node.Children.Count - 1));
            }

            return sb;
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
