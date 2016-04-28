#region

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace QuickSelectControl
{
    internal class HorizonatalTreeRender_Left_To_Right : IRender
    {
        public HorizonatalTreeRender_Left_To_Right(RectangleF _clientRectangleF)
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
            var fixedWidth = ClientRectangleF.Width/nodeTiers.Count();

            if (fixedWidth < 20) // TODO: Make this magic numberless.
            {
                fixedWidth = 20;
            }

            var i = 0;
            foreach (var source in nodeTiers.Reverse())
            {
                var height = ClientRectangleF.Height/source.Nodes.Count();
                for (var j = 0; j < source.Nodes.Count(); j++)
                {
                    var AWholeNewRegion = new Rectangle((int) (ClientRectangleF.Width - (i + 1)*fixedWidth),
                        (int) (j*height), (int) fixedWidth, (int) height);

                    // Begin Draw
                    switch (source.Nodes[j].State)
                    {
                        case NodeState.Normal:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.BackColor), AWholeNewRegion);
                            break;

                        case NodeState.Hover:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.HighlightColor), AWholeNewRegion);
                            break;

                        case NodeState.MouseDown:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.MouseDownColor), AWholeNewRegion);
                            break;

                        case NodeState.Selected:
                            graphics.FillRectangle(new SolidBrush(source.Nodes[j].Theme.SelectColor), AWholeNewRegion);
                            break;
                    }
                    graphics.DrawRectangle(new Pen(source.Nodes[j].Theme.BorderColor, 2), AWholeNewRegion);

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