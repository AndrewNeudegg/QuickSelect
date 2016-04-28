#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace QuickSelectControl
{
    internal class NodeSelection
    {
        public readonly INode[] Nodes;

        public NodeSelection(IEnumerable<INode> _Nodes)
        {
            Nodes = _Nodes.ToArray();
        }
    }
}