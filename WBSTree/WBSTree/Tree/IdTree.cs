using System;
using System.Collections.Generic;

namespace WBSTree.Tree
{
    public class IdTree : IEquatable<IdTree>
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

        public bool Equals(IdTree other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(RootNode, other.RootNode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IdTree) obj);
        }

        public override int GetHashCode()
        {
            return (RootNode != null ? RootNode.GetHashCode() : 0);
        }
    }
}
