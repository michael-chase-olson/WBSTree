using WBSTreeView.Tree;

namespace WBSTreeView.Serializer
{
    public interface IBinaryIdTreeSerializer
    {
        byte[] Serialize(IdTree tree);
        IdTree Deserialize(byte[] treeBytes);
    }
}
