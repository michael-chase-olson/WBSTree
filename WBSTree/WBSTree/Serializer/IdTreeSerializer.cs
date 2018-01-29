using System.Collections.Generic;
using System.IO;
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

        public IdTree Deserialize(byte[] treeBytes)
        {
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

            return deserializedTree;
        }

        private static void DeserializeIntoTree(BinaryReader binaryReader, IdTree tree)
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

        private static void AddChildNodeToParent(int parentId, IdNode node, IdTree tree)
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

        private static void AddChildToNonRootParent(int parentId, IdNode node, IdTree tree)
        {
            var parentNode = FindParentIdNode(parentId, tree);
            if (parentNode == null)
                return;

            node.Parent = parentNode;
            parentNode.Children.Add(node);
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

        private static int ReadInt(BinaryReader reader)
        {
            return reader.BaseStream.Position != reader.BaseStream.Length ? reader.ReadInt32() : -1;
        }
    }
}

