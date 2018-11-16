namespace MasterChief.DotNet4.Utilities.Model
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// 控制复制克隆的实体类
    /// 参考:http://www.codeproject.com/Articles/12976/How-to-Clone-Serialize-Copy-Paste-a-Windows-Forms
    /// </summary>
    [Serializable]
    public class ClipboardCtrl
    {
        #region Fields

        private static DataFormats.Format format;

        private Hashtable propertyList = new Hashtable();

        #endregion Fields

        #region Constructors

        static ClipboardCtrl()
        {
            format = DataFormats.GetFormat(typeof(ClipboardCtrl).FullName);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClipboardCtrl()
        {
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="ctrl">控件</param>
        public ClipboardCtrl(Control ctrl)
        {
            CtrlName = ctrl.GetType().Name;
            CtrlNamespace = ctrl.GetType().Namespace;
            PropertyDescriptorCollection _properties = TypeDescriptor.GetProperties(ctrl);
            foreach (PropertyDescriptor property in _properties)
            {
                if (property.PropertyType.IsSerializable)
                    propertyList.Add(property.Name, property.GetValue(ctrl));
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// DataFormats.Format
        /// </summary>
        public static DataFormats.Format Format
        {
            get
            {
                return format;
            }
        }

        /// <summary>
        /// 空间类型名称
        /// eg:Combox
        /// </summary>
        public string CtrlName
        {
            get;
            set;
        }

        /// <summary>
        ///控件的名称空间
        ///eg:System.Windows.Forms
        /// </summary>
        public string CtrlNamespace
        {
            get;
            set;
        }

        /// <summary>
        /// 控件的属性集合
        /// </summary>
        public Hashtable CtrlPropertyList
        {
            get
            {
                return propertyList;
            }
        }

        #endregion Properties
    }
}