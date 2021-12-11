using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class TreeNode
    {
        public TreeNode Parent { get; set; }


        public List<TreeNode> childNodes;
        public IEnumerable<TreeNode> ChildNodes { get => childNodes; }

        public void Add(TreeNode item)
        {
            if (childNodes == null)
                childNodes = new List<TreeNode>();
            item.Parent = this;
            childNodes.Add(item);
        }

        public IASimple2 Data { get; set; } 
    }
}
