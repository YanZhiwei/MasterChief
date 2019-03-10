using System;
using System.Xml.Serialization;

namespace MasterChief.DotNet.Core.ConfigTests
{
    [Serializable]
    public class RedisItemConfig
    {
        [XmlAttribute("txt")]
        public string Text { get; set; }
    }
}