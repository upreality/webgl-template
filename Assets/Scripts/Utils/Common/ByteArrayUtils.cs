using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils.Misc
{
    public static class ByteArrayUtils
    {
        public static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default;
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream(data);
            var obj = bf.Deserialize(ms);
            return (T)obj;
        }
    }
}