#region

using System.Collections.Generic;
using System.Drawing;

#endregion

namespace QuickSelectControl
{
    internal class Node : INode
    {
        public Node(string _text, string _description, string _hoverDescription, string _fileSystemUri, NodeTheme _theme,
            Font _fontFace, Bitmap _iconBitmap, bool _shouldBeRendered, bool _hasDecendants,
            List<string> _potentialChildren = null)
        {
            Text = _text;
            Description = _description;
            HoverDescription = _hoverDescription;
            FileSystemUri = _fileSystemUri;
            Theme = _theme;
            FontFace = _fontFace;
            IconBitmap = _iconBitmap;
            Bounds = new RectangleF(0, 0, 0, 0);
            ShouldBeRendered = _shouldBeRendered;
            HasDecendants = _hasDecendants;
            PotentialChildren = _potentialChildren;
        }

        // Bool
        public bool ShouldBeRendered { get; set; }
        public bool HasDecendants { get; set; }


        // Strings
        public string Text { get; set; }
        public string Description { get; set; }
        public string HoverDescription { get; set; }
        public string FileSystemUri { get; set; }
        public List<string> PotentialChildren { get; set; }

        // Theme
        public NodeTheme Theme { get; set; }

        // Fonts
        public Font FontFace { get; set; }

        // Image Data
        public Bitmap IconBitmap { get; set; }

        // Bounds
        public RectangleF Bounds { get; set; }

        // State
        public NodeState State { get; set; }

        public INode[] GenererateChildren(FileSystemQuery fsQuery, string filter, NodeTheme _defaultNodeTheme,
            Font _defaultFont)
        {
            return fsQuery.GetDescendantNodes(this, filter).ToArray();
        }
    }
}