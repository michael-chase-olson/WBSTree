using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WBSTree.Tree
{
    public class IdNode : IEquatable<IdNode>
    {
        public IdNode()
        {
            Children = new List<IdNode>();
        }

        public int Id { get; set; }
        public IdNode Parent { get; set; }
        public List<IdNode> Children { get; set; }


        public bool Equals(IdNode other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Id != other.Id)
                return false;
            if (Parent is null && !(other.Parent is null))
                return false;
            if (Parent != null && other.Parent != null && Parent.Id != other.Parent.Id)
                return false;
            if (Children.Count != other.Children.Count)
                return false;

            return !Children.Where((t, j) => !t.Equals(other.Children[j])).Any();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IdNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Parent != null ? Parent.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Children != null ? Children.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
