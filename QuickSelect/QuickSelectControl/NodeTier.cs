#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace QuickSelectControl
{
    internal class NodeTier
    {
        public readonly int LayerID;
        public readonly INode[] Nodes;
        private float _caretPosition;

        public bool RequiresScroll = false;

        public NodeTier(IEnumerable<INode> _nodes, int layerNumber)
        {
            Nodes = _nodes.ToArray();
            LayerID = layerNumber;
        }

        /// <summary>
        ///     0 to 100 %%
        /// </summary>
        public float CaretPosition
        {
            get { return _caretPosition; }
            set
            {
                if (value > 100)
                {
                    _caretPosition = value;
                }

                if (value < 0)
                {
                    _caretPosition = value;
                }

                if (value >= 0 & value <= 100)
                {
                    _caretPosition = value;
                }
            }
        }

        public IEnumerable<INode> GetLayout(float avaliableHeight, float itemHeight)
        {
            if (!RequiresScroll)
            {
                return Nodes;
            }

            var number = (int) (avaliableHeight/itemHeight);
            var position = (int) CaretPosition; // BUG Fix this
            var _nodes = new List<INode>();
            var j = 0;

            for (var i = 0; i < Nodes.Count(); i++)
            {
                if (i >= position & j <= number)
                {
                    _nodes.Add(Nodes[i]);
                }
                j++;
            }
            return _nodes;
        }
    }
}