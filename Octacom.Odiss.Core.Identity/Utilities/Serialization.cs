using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Octacom.Odiss.Core.Identity.Utilities
{
    /// <summary>
    /// Copy paste from the existing Odiss.Library code
    /// </summary>
    public class Serialization
    {
        public byte[] Serialize<T>(T data)
        {
            var serializable = new SerializableData<T>(data);

            using (MemoryStream mem = new MemoryStream())
            {
                using (DeflateStream zip = new DeflateStream(mem, CompressionMode.Compress, true))
                {
                    using (MemoryStream ser = new MemoryStream())
                    {
                        new BinaryFormatter().Serialize(ser, serializable);
                        ser.Flush();
                        ser.WriteTo(zip);
                    }
                    zip.Flush();
                }

                mem.Flush();

                return mem.ToArray();
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null || !data.Any())
            {
                return default(T);
            }

            using (MemoryStream mem = new MemoryStream())
            {
                using (MemoryStream memcrypt = new MemoryStream(data))
                {
                    using (DeflateStream zip = new DeflateStream(memcrypt, CompressionMode.Decompress, false))
                    {
                        zip.CopyTo(mem);
                    }
                }

                mem.Flush();
                mem.Position = 0L;

                var serializable = (SerializableData<T>)new BinaryFormatter().Deserialize(mem);

                if (serializable == null)
                {
                    return default(T);
                }

                return serializable.Data;
            }
        }
    }

    [Serializable]
    internal class SerializableData<T>
    {
        public SerializableData(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
