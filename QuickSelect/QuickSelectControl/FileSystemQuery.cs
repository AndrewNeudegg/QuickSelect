#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Etier.IconHelper;

#endregion

namespace QuickSelectControl
{
    internal class FileSystemQuery
    {
        public FileSystemQuery(NodeTheme _defaultNodeTheme, Font _defualtNodeFont)
        {
            DefaultNodeTheme = _defaultNodeTheme;
            DefaultNodeFont = _defualtNodeFont;
        }

        public NodeTheme DefaultNodeTheme { get; set; }
        public Font DefaultNodeFont { get; set; }


        public List<INode> GetDescendantNodes(string _directory, string _filter, bool getDirectories = true,
            bool getFiles = true)
        {
            // TODO: Potential bug with a file showing allowing further indexing.
            // TODO: Implement .lnk and symbolic link handling.
            // TODO: Fix file handling.

            // Define the return value.
            var returnNodesList = new List<INode>();
            var pathsList = new List<string>();
            if (getDirectories)
            {
                pathsList.AddRange(Directory.GetDirectories(_directory, _filter, SearchOption.TopDirectoryOnly));
                foreach (var path in pathsList)
                {
                    var node = new Node(GetDistinctName(path), "", "", path, DefaultNodeTheme, DefaultNodeFont,
                        GetFolderIconBitmap(path), true, true);
                    returnNodesList.Add(node);
                }
                pathsList.Clear();
            }

            if (getFiles)
            {
                pathsList.AddRange(Directory.GetFiles(_directory, _filter, SearchOption.TopDirectoryOnly));
                foreach (var path in pathsList)
                {
                    var node = new Node(GetDistinctName(path), "", "", path, DefaultNodeTheme, DefaultNodeFont,
                        GetFileIconBitmap(path), true, true);
                    node.HasDecendants = false;
                    returnNodesList.Add(node);
                }
                pathsList.Clear();
            }
            return returnNodesList;
        }

        public List<INode> GetDescendantNodes(INode inode, string _filter, bool getDirectories = true,
            bool getFiles = true)
        {
            // TODO: Potential bug with a fileq showing allowing further indexing.
            // TODO: Implement .lnk and symbolic link handling.
            // TODO: Fix file handling.
            // Define the return value.
            var returnNodesList = new List<INode>();
            var pathsList = new List<string>();
            if (inode.HasDecendants)
            {
                if (getDirectories)
                {
                    pathsList.AddRange(Directory.GetDirectories(inode.FileSystemUri, _filter,
                        SearchOption.TopDirectoryOnly));
                    foreach (var path in pathsList)
                    {
                        var node = new Node(GetDistinctName(path), "", "", path, DefaultNodeTheme, DefaultNodeFont,
                            GetFolderIconBitmap(path), true, true);
                        returnNodesList.Add(node);
                    }
                    pathsList.Clear();
                }

                if (getFiles)
                {
                    pathsList.AddRange(Directory.GetFiles(inode.FileSystemUri, _filter, SearchOption.TopDirectoryOnly));
                    foreach (var path in pathsList)
                    {
                        var node = new Node(GetDistinctName(path), "", "", path, DefaultNodeTheme, DefaultNodeFont,
                            GetFileIconBitmap(path), true, false);
                        node.HasDecendants = false;
                        returnNodesList.Add(node);
                    }
                    pathsList.Clear();
                }

                if (returnNodesList.Count == 0)
                {
                    // here
                    var nt = new NodeTheme(Color.LightGray, Color.DarkSlateGray, Color.Beige, Color.Black, Color.Black,
                        Color.Black);
                    var node = new Node("Empty", "", "", "", DefaultNodeTheme, DefaultNodeFont,
                        SystemIcons.Exclamation.ToBitmap(), true, true);

                    node.State = NodeState.None;
                    node.HasDecendants = false;
                    returnNodesList.Add(node);
                }
            }


            return returnNodesList;
        }

        /// <summary>
        ///     Gets the significant portion of a path.
        /// </summary>
        /// <param name="path">The path to evaluate</param>
        /// <returns>The file or directory name</returns>
        public string GetDistinctName(string path)
        {
            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path).Name;
            }
            if (File.Exists(path))
            {
                return new FileInfo(path).Name;
            }
            throw new NotSupportedException();
        }


        /// <summary>
        ///     Grabs the associated icon with a path.
        /// </summary>
        /// <param name="path">The path to grab.</param>
        /// <returns>Bitmap icon object.</returns>
        public Bitmap GetFileIconBitmap(string path)
        {
            try
            {
                return IconReader.GetFileIcon(path, IconReader.IconSize.Large, false).ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return SystemIcons.WinLogo.ToBitmap();
            }
        }

        public Bitmap GetFolderIconBitmap(string path)
        {
            try
            {
                return IconReader.GetFolderIcon(IconReader.IconSize.Large, IconReader.FolderType.Open).ToBitmap();
            }
            catch (Exception)
            {
                return SystemIcons.WinLogo.ToBitmap();
            }
        }
    }
}