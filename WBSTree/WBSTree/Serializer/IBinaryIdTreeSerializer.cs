using WBSTree.Tree;

namespace WBSTree.Serializer
{
    public interface IBinaryIdTreeSerializer
    {
        byte[] Serialize(IdTree tree);
        IdTree Deserialize(byte[] treeBytes);
    }
}
