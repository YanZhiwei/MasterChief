using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     TreeView帮助类
    /// </summary>
    public static class TreeViewHelper
    {
        #region Methods

        /// <summary>
        ///     选中节点高亮
        ///     <para>eg: treeView1.ApplyNodeHighLight(Color.Red);</para>
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="highLightColor">高亮的颜色</param>
        public static void ApplyNodeHighLight(this TreeView treeView, Brush highLightColor)
        {
            if (treeView.DrawMode != TreeViewDrawMode.OwnerDrawText) treeView.DrawMode = TreeViewDrawMode.OwnerDrawText;

            if (treeView.HideSelection) treeView.HideSelection = false;

            treeView.DrawNode += (sender, e) =>
            {
                e.Graphics.FillRectangle(Brushes.White, e.Node.Bounds);

                if (e.State == TreeNodeStates.Selected)
                {
                    e.Graphics.FillRectangle(highLightColor,
                        new Rectangle(e.Node.Bounds.Left, e.Node.Bounds.Top, e.Node.Bounds.Width,
                            e.Node.Bounds.Height));
                    e.Graphics.DrawString(e.Node.Text, treeView.Font, Brushes.White, e.Bounds);
                }
                else
                {
                    e.DrawDefault = true;
                }
            };
        }

        /// <summary>
        ///     添加右键菜单
        ///     <para>eg: treeF18.AttachMenu(contextMenuTree, n => n != null);</para>
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="contextMenu">ContextMenuStrip</param>
        /// <param name="showContextMenuFactory">显示ContextMenuStrip规则委托</param>
        public static void AttachMenu(this TreeView treeView, ContextMenuStrip contextMenu,
            Predicate<TreeNode> showContextMenuFactory)
        {
            treeView.MouseDown += (sender, e) =>
            {
                var curTree = sender as TreeView;

                if (e.Button == MouseButtons.Right)
                {
                    var clickPoint = new Point(e.X, e.Y);
                    // ReSharper disable once PossibleNullReferenceException
                    var curNode = curTree.GetNodeAt(clickPoint);

                    if (showContextMenuFactory != null)
                        if (showContextMenuFactory(curNode))
                        {
                            curTree.SelectedNode = curNode;
                            curNode.ContextMenuStrip = contextMenu;
                        }
                }
            };
        }

        /// <summary>
        ///     检查节点是否存在
        /// </summary>
        /// <param name="tree">TreeView</param>
        /// <param name="nodeCompareFactory">节点判断委托</param>
        /// <param name="findedNode">找到节点</param>
        /// <returns>是否存在目标节点</returns>
        public static bool CheckNodeExist(this TreeView tree, Predicate<TreeNode> nodeCompareFactory,
            out TreeNode findedNode)
        {
            var exists = false;
            findedNode = null;

            for (var i = 0; i < tree.Nodes.Count; i++)
            {
                var curNode = tree.Nodes[i];

                if (nodeCompareFactory(curNode))
                {
                    findedNode = curNode;
                    exists = true;
                    break;
                }

                exists = CheckNodeExist(tree.Nodes[i], nodeCompareFactory, out findedNode);

                if (exists) break;
            }

            return exists;
        }

        /// <summary>
        ///     查找子节点是否存在
        /// </summary>
        /// <param name="node">目标节点</param>
        /// <param name="nodeCompareFactory">节点判断委托</param>
        /// <param name="findedNode">找到节点</param>
        /// <returns>是否存在目标节点</returns>
        public static bool CheckNodeExist(this TreeNode node, Predicate<TreeNode> nodeCompareFactory,
            out TreeNode findedNode)
        {
            findedNode = null;
            var result = false;

            for (var i = 0; i < node.Nodes.Count; i++)
            {
                var curNode = node.Nodes[i];

                if (nodeCompareFactory(curNode))
                {
                    findedNode = curNode;
                    result = true;
                    break;
                }

                if (!result && curNode.Nodes.Count > 0)
                    result = CheckNodeExist(curNode, nodeCompareFactory, out findedNode);
            }

            return result;
        }

        /// <summary>
        ///     将目标文件夹递归加载Tree
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="folder">目标文件夹</param>
        public static void LoadDirectory(TreeView treeView, string folder)
        {
            var root = new DirectoryInfo(folder);
            var parentNode = treeView.Nodes.Add(root.Name);
            parentNode.Tag = root.FullName;
            parentNode.StateImageIndex = 0;
            LoadFiles(folder, parentNode);
            LoadSubDirectories(folder, parentNode);
            treeView.ExpandAll();
        }

        private static void LoadFiles(string dir, TreeNode node)
        {
            var files = Directory.GetFiles(dir, "*.*");
            foreach (var item in files)
            {
                var fileInfo = new FileInfo(item);
                var treeNode = node.Nodes.Add(fileInfo.Name);
                treeNode.Tag = fileInfo.FullName;
                treeNode.StateImageIndex = 1;
            }
        }

        private static void LoadSubDirectories(string folder, TreeNode treeNode)
        {
            var subFolder = Directory.GetDirectories(folder);

            foreach (var item in subFolder)
            {
                var directory = new DirectoryInfo(item);
                var node = treeNode.Nodes.Add(directory.Name);
                node.StateImageIndex = 0;
                node.Tag = directory.FullName;
                LoadFiles(item, node);
                LoadSubDirectories(item, node);
            }
        }

        #endregion Methods
    }
}