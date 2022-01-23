using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class TreeNode
    {
        public TreeNode Head { get; set; }
        public TreeNode Parent { get; set; }


        public ConcurrentBag<TreeNode> childNodes;
        public ConcurrentBag<TreeNode> ChildNodes { get => childNodes; }

        public void Add(TreeNode item)
        {
            if (childNodes == null)
                childNodes = new ConcurrentBag<TreeNode>();
            item.Parent = this;
            childNodes.Add(item);
        }

        public void Clear()
        {
            childNodes = null;
        }
        public IASimple Data { get; set; } 
    }
}
