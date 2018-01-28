using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using WBSTree.Tree;

namespace WBSTree.Serializer
{
    public class IdTreeSerializer : IBinaryIdTreeSerializer
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
            if (node.Parent != null)
                binaryWriter.Write(node.Parent.Id);

            binaryWriter.Write(node.Id);

            foreach (var child in node.Children)
            {
                SerializeNode(child, binaryWriter);
            }
        }

        public IdTree Deserialize(byte[] treeBytes)
        {
            var deserializedTree = new IdTree();

            using (var memoryStream = new MemoryStream(treeBytes))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    var rootnode = new IdNode { Id = binaryReader.ReadInt32() };
                    deserializedTree.RootNode = rootnode;
                    DeserializeFromRoot(binaryReader, deserializedTree);

                }
            }

            return deserializedTree;
        }

        private static void DeserializeFromRoot(BinaryReader binaryReader, IdTree tree)
        {
            var parentId = binaryReader.ReadInt32();
            var currentNodeId = binaryReader.ReadInt32();

            var node = new IdNode { Id = currentNodeId };

            if (parentId == tree.RootNode.Id)
            {
                node.Parent = tree.RootNode;
                tree.RootNode.Children.Add(node);
            }
            else
            {
                var parentNode = FindParentIdNode(parentId, tree);
                if (parentNode != null)
                {
                    node.Parent = parentNode;
                    parentNode.Children.Add(node);
                }
            }

            if (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                DeserializeFromRoot(binaryReader, tree);
        }

        private static IdNode FindParentIdNode(int idOfParent, IdTree tree)
        {
            var searchQueue = new Queue<IdNode>();
            searchQueue.Enqueue(tree.RootNode);

            while (searchQueue.Count > 0)
            {
                var current = searchQueue.Dequeue();
                if (current.Id == idOfParent)
                    return current;

                foreach (var child in current.Children)
                {
                    searchQueue.Enqueue(child);
                }
            }

            return null;
        }
    }
}

