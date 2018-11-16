namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 适用于Jquery jsTree
    /// </summary>
    public class JsChildNode
    {
        #region Properties

        /// <summary>
        /// 是否包含子节点
        /// </summary>
        public bool children
        {
            get;
            set;
        }

        /// <summary>
        /// 节点图片路径
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