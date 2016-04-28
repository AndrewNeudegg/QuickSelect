#region

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace QuickSelectControl
{
    /// <summary>
    ///     Renders the quick select control.
    /// </summary>
    internal class VerticalTreeRender : IRender
    {
        public VerticalTreeRender(RectangleF _clientRectangleF)
        {
            ClientRectangleF = _clientRectangleF;
        }

        public RectangleF ClientRectangleF { get; set; }

        // TODO: Design Render
        public void RenderBackground(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.DrawRectangle(new Pen(Color.Black), ClientRectangleF.X, ClientRectangleF.Y, ClientRectangleF.Width,
                ClientRectangleF.Height);
            ControlPaint.DrawBorder(graphics, ToRectangle(ClientRectangleF),
                Color.Black, 2, ButtonBorderStyle.Inset,
                Color.Black, 2, ButtonBorderStyle.Inset,
                Color.Black, 2, ButtonBorderStyle.Inset,
                Color.Black, 2, ButtonBorderStyle.Inset);
        }

        public void RenderNodes(Graphics graphics, IEnumerable<NodeTier> nodeTiers)
        {
            var linearHeight = ClientRectangleF.Height/nodeTiers.Count();

            if (linearHeight < 20) // TODO: Make this magic numberless.
            {
                linearHeight = 20;
            }

            var i = 0;
            foreach (var source in nodeTiers.Reverse())
            {
                var width = ClientRectangleF.Width/source.Nodes.Count();
                for (var j = 0; j < source.Nodes.Count(); j++)
                {
                    var regionRectangle =
                        new Rectangle((int) (j*width), (int) (ClientRectangleF.Height - (i + 1)*linearHeight),
                            (int) width, (int) linearHeight);
                    switch (source.Nodes[j].State)
                    {
                        case NodeState.Normal:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.BackColor), regionRectangle);
                            break;

                        case NodeState.Hover:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.HighlightColor), regionRectangle);
                            break;

                        case NodeState.MouseDown:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.MouseDownColor), regionRectangle);
                            break;

                        case NodeState.Selected:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.SelectColor), regionRectangle);
                            break;
                    }
                    graphics.DrawRectangle(new Pen(source.Nodes[j].Theme.BorderColor, 2), regionRectangle);
                    graphics.DrawString(source.Nodes[j].Text, source.Nodes[j].FontFace,
                        new SolidBrush(source.Nodes[j].Theme.ForeColor), regionRectangle);
                    source.Nodes[j].Bounds = regionRectangle;
                }
                i++;
            }
        }

        public void RenderOverlays(Graphics graphics, INode node)
        {
        }

        public Rectangle ToRectangle(RectangleF rect)
        {
            return new Rectangle((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height);
        }
    }
}