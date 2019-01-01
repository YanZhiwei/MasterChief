using MasterChief.DotNet.Infrastructure.Serializer;
using System;
using System.IO;

namespace MasterChief.DotNet.Infrastructure.ProtobufSerializer
{
    public class ProtobufSerializer : ISerializer
    {
        public T Deserialize<T>(string data)
        {
            byte[] buffer = Convert.FromBase64String(data);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }

        public string Serialize(object serializeObject)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, serializeObject);
                return Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }
    }
}