namespace MasterChief.DotNet4.Utilities.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// 适用于Jquery jsTree
    /// </summary>
    public class JsTreeNode
    {
        #region Fields

        /// <summary>
        /// 子节点集合
        /// </summary>
        public List<JsChildNode> children;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 节点图标路径
        /// </summary>
        public string icon
        {
            get; set;
        }

        /// <summary>
        /// 键
        /// </summary>
        public int id
        {
            get; set;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string text
        {
            get; set;
        }

        #endregion Properties
    }
}