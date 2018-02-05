using System.Collections.Generic;

namespace WBSTreeView.Tree
{
    public class IdTree
    {
        public IdNode RootNode { get; set; }

        public IdNode FindNode(int nodeId)
        {
            var searchQueue = new Queue<IdNode>();
            searchQueue.Enqueue(RootNode);

            while (searchQueue.Count > 0)
            {
                var current = searchQueue.Dequeue();
                if (current.Id == nodeId)
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
