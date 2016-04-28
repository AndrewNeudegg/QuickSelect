#region

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace QuickSelectControl
{
    internal class L2RNiceRender : IRender
    {
        public L2RNiceRender(RectangleF _clientRectangleF)
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
                Color.Gainsboro, 2, ButtonBorderStyle.Inset,
                Color.Gainsboro, 2, ButtonBorderStyle.Inset,
                Color.Gainsboro, 2, ButtonBorderStyle.Inset,
                Color.Gainsboro, 2, ButtonBorderStyle.Inset);
        }

        public void RenderNodes(Graphics graphics, IEnumerable<NodeTier> nodeTiers)
        {
            var fixedWidth = ClientRectangleF.Width/nodeTiers.Count();

            if (fixedWidth < 10) // TODO: Make this magic numberless.
            {
                fixedWidth = 10;
            }

            var i = 0;
            foreach (var source in nodeTiers.Reverse())
            {
                var height = ClientRectangleF.Height/source.Nodes.Count();
                if (height < 20)
                {
                    height = 20;
                }
                var nodes = source.GetLayout(ClientRectangleF.Height, height).ToArray();

                if (nodes.Count() != source.Nodes.Count())
                {
                    source.RequiresScroll = true;
                }

                for (var j = 0; j < nodes.Count(); j++)
                {
                    Rectangle AWholeNewRegion;
                    if (nodes[j].IconBitmap != null)
                    {
                        AWholeNewRegion = new Rectangle((int) (ClientRectangleF.Width - (i + 1)*fixedWidth),
                            (int) (j*height), (int) fixedWidth, (int) height);
                    }
                    else
                    {
                        AWholeNewRegion = new Rectangle((int) (ClientRectangleF.Width - (i + 1)*fixedWidth),
                            (int) (j*height), (int) fixedWidth, (int) height);
                    }

                    var columnContainsMouse = false;


                    // Begin Draw
                    switch (source.Nodes[j].State)
                    {
                        case NodeState.Normal:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.BackColor), AWholeNewRegion);
                            break;

                        case NodeState.Hover:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.HighlightColor), AWholeNewRegion);
                            columnContainsMouse = true;
                            break;

                        case NodeState.MouseDown:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.MouseDownColor), AWholeNewRegion);
                            break;

                        case NodeState.Selected:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.SelectColor), AWholeNewRegion);
                            break;
                        case NodeState.None:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.BackColor), AWholeNewRegion);
                            break;
                    }
                    graphics.DrawRectangle(new Pen(source.Nodes[j].Theme.BorderColor, 2), AWholeNewRegion);

                    if (nodes[j].IconBitmap != null)
                    {
                        graphics.DrawImage(nodes[j].IconBitmap, (int) (ClientRectangleF.Width - (i + 1)*fixedWidth),
                            (int)(j*height));
                    }

                    if (columnContainsMouse)
                    {
                        // Render Scroll bar and scroll signs.
                    }

                    var format = new StringFormat();
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Center;
                    graphics.DrawString(source.Nodes[j].Text, source.Nodes[j].FontFace,
                        new SolidBrush(source.Nodes[j].Theme.ForeColor), AWholeNewRegion, format);
                    source.Nodes[j].Bounds = AWholeNewRegion;
                    // End Draw
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