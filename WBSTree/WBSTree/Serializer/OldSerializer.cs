using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WBSTree.Tree;

namespace WBSTree.Serializer
{
    public class OldSerializer
    {
        public byte[] Serialize(IdTree tree)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    SerializeNode(tree.RootNode, binaryWriter);
                }

                return memoryStream.ToArray();
            }
        }


        private static void SerializeNode(IdNode node, BinaryWriter binaryWriter)
        {
            //serialize the parent Id first, if it exists, then the current node id
            //Only the root node has no parent
            if (node.Parent != null)
                binaryWriter.Write(node.Parent.Id);

            binaryWriter.Write(node.Id);

            foreach (var child in node.Children)
            {
                SerializeNode(child, binaryWriter);
            }
        }

        public double GetTime()
        {
            return _watch.ElapsedMilliseconds;
        }

        private readonly Stopwatch _watch = new Stopwatch();

        public IdTree Deserialize(byte[] treeBytes)
        {
            _watch.Reset();
            _watch.Start();
            var deserializedTree = new IdTree();

            using (var memoryStream = new MemoryStream(treeBytes))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    var rootnode = new IdNode { Id = binaryReader.ReadInt32() };
                    deserializedTree.RootNode = rootnode;
                    DeserializeIntoTree(binaryReader, deserializedTree);

                }
            }
            _watch.Stop();

            return deserializedTree;
        }


        private void DeserializeIntoTree(BinaryReader binaryReader, IdTree tree)
        {
            //nodes need to be deserialized in the same order they were serialized.  In this case, the parent node id is serialized first
            //followed by the current node id.  Only the root node has no parent.
            var parentId = ReadInt(binaryReader);
            var currentNodeId = ReadInt(binaryReader);

            if (parentId == -1 || currentNodeId == -1)
                return;

            var node = new IdNode { Id = currentNodeId };

            AddChildNodeToParent(parentId, node, tree);

            if (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                DeserializeIntoTree(binaryReader, tree);
        }

        private void AddChildNodeToParent(int parentId, IdNode node, IdTree tree)
        {
            if (parentId == tree.RootNode.Id)
            {
                AddChildToRoot(node, tree);
            }
            else
            {
                AddChildToNonRootParent(parentId, node, tree);
            }
        }

        private static void AddChildToRoot(IdNode node, IdTree tree)
        {
            node.Parent = tree.RootNode;
            tree.RootNode.Children.Add(node);
        }

        private void AddChildToNonRootParent(int parentId, IdNode node, IdTree tree)
        {
                                    var parentNode = tree.FindNode(parentId);
                                    if (parentNode == null)
                                        return;
                        
                                    node.Parent = parentNode;
                                    parentNode.Children.Add(node);
        }

        private static int ReadInt(BinaryReader reader)
        {
            return reader.BaseStream.Position != reader.BaseStream.Length ? reader.ReadInt32() : -1;
        }
    }
}

