using System.Collections.Generic;

namespace WBSTree.Tree
{
    public class IdNode
    {
        public int Id { get; set; }
        public IdNode Parent { get; set; }
        public List<IdNode> Children { get; set; }
    }
}
