using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     TreeView 序列化与反序列化
    /// </summary>
    public class TreeViewSerializer
    {
        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="path">XML序列化或者反序列化路径;eg:string.Format(@"{0}\Config\CtuPtuTreeView.xml", Application.StartupPath)</param>
        public TreeViewSerializer(string path)
        {
            _xmlSaveFullPath = path;
        }

        #endregion Constructors

        #region Fields

        private const string XmlNodeImageIndexAtt = "imageindex";
        private const string XmlNodeTag = "node";
        private const string XmlNodeTagAtt = "tag";
        private const string XmlNodeTextAtt = "text";

        private readonly string _xmlSaveFullPath;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     反序列化TreeView
        /// </summary>
        /// <param name="treeView">TreeView</param>
        public void DeserializeTreeView(TreeView treeView)
        {
            XmlTextReader reader = null;
            try
            {
                treeView.BeginUpdate();
                reader = new XmlTextReader(_xmlSaveFullPath);

                TreeNode parentNode = null;

                while (reader.Read())
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == XmlNodeTag)
                        {
                            var newNode = new TreeNode();
                            var isEmptyElement = reader.IsEmptyElement;

                            var attributeCount = reader.AttributeCount;
                            if (attributeCount > 0)
                                for (var i = 0; i < attributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    SetAttributeValue(newNode, reader.Name, reader.Value);
                                }

                            if (parentNode != null)
                                parentNode.Nodes.Add(newNode);
                            else
                                treeView.Nodes.Add(newNode);

                            if (!isEmptyElement) parentNode = newNode;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        if (reader.Name == XmlNodeTag) parentNode = parentNode.Parent;
                    }
                    else if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    {
                    }
                    else if (reader.NodeType == XmlNodeType.None)
                    {
                        return;
                    }
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        parentNode.Nodes.Add(reader.Value);
                    }
            }
            finally
            {
                treeView.EndUpdate();
                reader?.Close();
            }
        }

        /// <summary>
        ///     将XML文件配置节点信息映射到TreeView
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="fileName">XML路径</param>
        public void LoadXmlFileInTreeView(TreeView treeView, string fileName)
        {
            XmlTextReader xmlTextReader = null;
            try
            {
                treeView.BeginUpdate();
                xmlTextReader = new XmlTextReader(fileName);

                var node = new TreeNode(fileName);
                treeView.Nodes.Add(node);
                while (xmlTextReader.Read())
                    if (xmlTextReader.NodeType == XmlNodeType.Element)
                    {
                        var isEmptyElement = xmlTextReader.IsEmptyElement;
                        var builder = new StringBuilder();
                        builder.Append(xmlTextReader.Name);
                        var attributeCount = xmlTextReader.AttributeCount;
                        if (attributeCount > 0)
                        {
                            builder.Append(" ( ");
                            for (var i = 0; i < attributeCount; i++)
                            {
                                if (i != 0) builder.Append(", ");
                                xmlTextReader.MoveToAttribute(i);
                                builder.Append(xmlTextReader.Name);
                                builder.Append(" = ");
                                builder.Append(xmlTextReader.Value);
                            }

                            builder.Append(" ) ");
                        }

                        if (isEmptyElement)
                            node.Nodes.Add(builder.ToString());
                        else
                            node = node.Nodes.Add(builder.ToString());
                    }
                    else if (xmlTextReader.NodeType == XmlNodeType.EndElement)
                    {
                        node = node.Parent;
                    }
                    else if (xmlTextReader.NodeType == XmlNodeType.XmlDeclaration)
                    {
                    }
                    else if (xmlTextReader.NodeType == XmlNodeType.None)
                    {
                        return;
                    }
                    else if (xmlTextReader.NodeType == XmlNodeType.Text)
                    {
                        node.Nodes.Add(xmlTextReader.Value);
                    }
            }
            finally
            {
                treeView.EndUpdate();
                xmlTextReader?.Close();
            }
        }

        /// <summary>
        ///     序列化TreeView
        /// </summary>
        /// <param name="treeView">TreeView</param>
        public void SerializeTreeView(TreeView treeView)
        {
            var textWriter = new XmlTextWriter(_xmlSaveFullPath, Encoding.UTF8);
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("TreeView");
            SaveNodes(treeView.Nodes, textWriter);
            textWriter.WriteEndElement();
            textWriter.Close();
        }

        /// <summary>
        ///     保存节点
        /// </summary>
        /// <param name="nodesCollection">TreeNodeCollection</param>
        /// <param name="textWriter">XmlTextWriter</param>
        private void SaveNodes(TreeNodeCollection nodesCollection,
            XmlTextWriter textWriter)
        {
            for (var i = 0; i < nodesCollection.Count; i++)
            {
                var node = nodesCollection[i];
                textWriter.WriteStartElement(XmlNodeTag);
                textWriter.WriteAttributeString(XmlNodeTextAtt, node.Text);
                textWriter.WriteAttributeString(XmlNodeImageIndexAtt, node.ImageIndex.ToString());
                if (node.Tag != null)
                    textWriter.WriteAttributeString(XmlNodeTagAtt, node.Tag.ToString());

                if (node.Nodes.Count > 0) SaveNodes(node.Nodes, textWriter);
                textWriter.Formatting = Formatting.Indented;
                textWriter.WriteEndElement();
            }
        }

        /// <summary>
        ///     反序列化TreeView时候，设置节点属性
        /// </summary>
        /// <param name="node">TreeNode</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性数值</param>
        private void SetAttributeValue(TreeNode node, string propertyName, string value)
        {
            if (propertyName == XmlNodeTextAtt)
                node.Text = value;
            else if (propertyName == XmlNodeImageIndexAtt)
                node.ImageIndex = int.Parse(value);
            else if (propertyName == XmlNodeTagAtt) node.Tag = value;
        }

        #endregion Methods
    }
}