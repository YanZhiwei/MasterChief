namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// MenuStrip 构建实体类
    /// </summary>
    public class MenuStripItem
    {
        #region Properties

        /// <summary>
        /// 关联命令名称
        /// </summary>
        public string ACTION_NAME
        {
            get; set;
        }

        /// <summary>
        /// 父ID
        /// </summary>
        public int FATHER_ID
        {
            get; set;
        }

        /// <summary>
        /// 标识ID
        /// </summary>
        public int ID
        {
            get; set;
        }

        /// <summary>
        /// MENU名称
        /// </summary>
        public string MENU_NAME
        {
            get; set;
        }

        /// <summary>
        /// MENU文本描述
        /// </summary>
        public string MENU_TEXT
        {
            get; set;
        }

        #endregion Properties
    }
}