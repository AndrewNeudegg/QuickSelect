#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace QuickSelectControl
{
    public class QuickSelectPanel : Control
    {
        public delegate void _HoverPath(string path);

        public delegate void _PathSelected(string path);

        private static readonly NodeTheme defaultNodeTheme = new NodeTheme(Color.White, Color.CornflowerBlue,
            Color.LightGray, Color.Aquamarine, Color.DarkSeaGreen, Color.BurlyWood); // Defualt theme.

        private static readonly Font defaultFont = new Font("Arial", 10);
        private readonly string defaultFilter = "*.*";
        private readonly FileSystemQuery fsQuery = new FileSystemQuery(defaultNodeTheme, defaultFont);
        private INode hoverNode; // The current hovered node.
        private List<NodeTier> NodeTiers; // Holds the current node tiers 
        private readonly IRender Renderer; // Contains the renderer.

        // Initiliser
        public QuickSelectPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserPaint, true);
            Renderer = new L2RNiceRender(ClientRectangle); // This controls the current rendering schema.
            NodeTiers = new List<NodeTier>();
            // Add the initial Tier
            AddBaseTier();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Renderer.RenderBackground(e.Graphics);
            Renderer.RenderNodes(e.Graphics, NodeTiers);
        }

        private void AddBaseTier()
        {
            var nodes = new List<INode>();
            nodes.AddRange(fsQuery.GetDescendantNodes("C:\\users\\andrew\\desktop\\folder\\", "*.*"));
            //Node node = new Node("Home","The Home Directory","Whoaa Description","C:\\users\\andrew\\", defaultNodeTheme, new Font("Arial",10),null, true,true);

            // nodes.Add(node);
            var OriginNodeTier = new NodeTier(nodes, 0);
            NodeTiers.Add(OriginNodeTier);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var node = DetectNode(e.Location);
            if (node != null)
            {
                if (node.State == NodeState.None)
                {
                    // We aren't interested in this node. (Just stating its empty).
                    return;
                }


                if (!node.Equals(hoverNode))
                {
                    if (hoverNode != null)
                    {
                        hoverNode.State = NodeState.Normal;
                        node.State = NodeState.Hover;
                    }


                    var row = GetLayerID(node);
                    var diff = NodeTiers.Count - row;
                    var hoverRow = GetLayerID(hoverNode);

                    if (row == hoverRow)
                    {
                        if (!node.HasDecendants)
                        {
                            NodeTiers = NodeTiers.Take(hoverRow + 1).ToList();
                        }
                        else
                        {
                            NodeTiers = NodeTiers.Take(hoverRow + 1).ToList();
                            var nodeTier =
                                new NodeTier(
                                    node.GenererateChildren(fsQuery, defaultFilter, defaultNodeTheme, defaultFont),
                                    NodeTiers[NodeTiers.Count - 1].LayerID + 1); // Reupdate the last row
                            NodeTiers.Add(nodeTier);
                        }
                    }
                    else
                    {
                        if (diff > 1) // then we are cutting off
                        {
// Progressive prune. WE HAVE ASCENDED
                            NodeTiers = NodeTiers.Take(row + 2).ToList();
                        }
                        else // we are adding on
                        {
                            // WE HAVE DESCENDED
                            var nodeTier =
                                new NodeTier(
                                    node.GenererateChildren(fsQuery, defaultFilter, defaultNodeTheme, defaultFont),
                                    NodeTiers[NodeTiers.Count - 1].LayerID + 1); // Reupdate the last row
                            NodeTiers.Add(nodeTier);
                        }
                    }


                    hoverNode = node;
                    if (HoverPath != null) HoverPath(node.FileSystemUri);


                    Invalidate();
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            var node = DetectNode(e.Location);
            var LayerID = GetLayerID(node);

            if (NodeTiers[LayerID].RequiresScroll)
            {
                NodeTiers[LayerID].CaretPosition += e.Delta;
            }
            Invalidate();
        }

        private int GetLayerID(INode parentNode)
        {
            // We want to trim everything but the direct descendant of the node.
            var rowFoundAt = 0;
            foreach (var nodeTier in NodeTiers)
            {
                foreach (var node in nodeTier.Nodes)
                {
                    if (node.Equals(parentNode))
                    {
                        return nodeTier.LayerID;
                    }
                }
            }
            return rowFoundAt; // Case not found. handle -1 ;(
        }


        private int RowDiff(INode oneNode, INode twoNode)
        {
            var startNode = 0;
            var endNode = 0;
            var i = 0;
            foreach (var tier in NodeTiers)
            {
                foreach (var node in tier.Nodes)
                {
                    if (node.Equals(oneNode))
                    {
                        startNode = i;
                    }

                    if (node.Equals(twoNode))
                    {
                        endNode = i;
                    }
                }
                i++;
            }
            return endNode - startNode;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var node = DetectNode(e.Location);
            if (node != null)
            {
                if (PathSelected != null) PathSelected(node.FileSystemUri);
                node.State = NodeState.MouseDown;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Renderer.ClientRectangleF = ClientRectangle;
        }

        private INode DetectNode(PointF point)
        {
            // TODO: Implement Efficent Indexing by search within tiers 
            var _hasHoverTarget = false;
            foreach (var nodeTier in NodeTiers)
            {
                foreach (var node in nodeTier.Nodes)
                {
                    if (node.Bounds.Contains(point))
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        public event _PathSelected PathSelected;
        public event _HoverPath HoverPath;

        private enum MovementType
        {
            None = 0,
            Up = 1,
            Horizontal = 2,
            Down = 3
        }
    }
}