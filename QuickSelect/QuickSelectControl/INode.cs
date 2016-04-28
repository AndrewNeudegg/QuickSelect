#region

using System.Collections.Generic;
using System.Drawing;

#endregion

namespace QuickSelectControl
{
    internal interface INode
    {
        // Bool
        bool ShouldBeRendered { get; set; }
        bool HasDecendants { get; set; }

        // Strings
        string Text { get; set; }
        string Description { get; set; }
        string HoverDescription { get; set; }
        string FileSystemUri { get; set; }
        List<string> PotentialChildren { get; set; }

        // Theme
        NodeTheme Theme { get; set; }

        // Fonts
        Font FontFace { get; set; }

        // Image Data
        Bitmap IconBitmap { get; set; }

        // Bounds
        RectangleF Bounds { get; set; }

        // State
        NodeState State { get; set; }

        // Voids
        INode[] GenererateChildren(FileSystemQuery fsQuery, string filter, NodeTheme _defaultNodeTheme,
            Font _defaultFont);
    }
}