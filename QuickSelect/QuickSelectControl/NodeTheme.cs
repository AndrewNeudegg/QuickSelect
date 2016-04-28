#region

using System.Drawing;

#endregion

namespace QuickSelectControl
{
    /// <summary>
    ///     Themes a node object.
    /// </summary>
    internal class NodeTheme
    {
        public NodeTheme(Color _backColor, Color _foreColor, Color _borderColor, Color _highlightColor,
            Color _mouseDownColor, Color _selectColor)
        {
            BackColor = _backColor;
            ForeColor = _foreColor;
            BorderColor = _borderColor;
            HighlightColor = _highlightColor;
            MouseDownColor = _mouseDownColor;
            SelectColor = _selectColor;
        }

        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        public Color BorderColor { get; set; }
        public Color HighlightColor { get; set; }
        public Color MouseDownColor { get; set; }
        public Color SelectColor { get; set; }
    }
}