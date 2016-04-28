#region

using System.Collections.Generic;
using System.Drawing;

#endregion

namespace QuickSelectControl
{
    internal interface IRender
    {
        RectangleF ClientRectangleF { get; set; }
        void RenderBackground(Graphics graphics);
        void RenderNodes(Graphics graphics, IEnumerable<NodeTier> nodeTiers);
        void RenderOverlays(Graphics graphics, INode node);
    }
}