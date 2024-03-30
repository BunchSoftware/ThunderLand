using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Packages
{
    public class StreamPackager
    {
        private readonly BinaryReader binaryReader;
        private readonly MemoryStream memoryStream = new MemoryStream();
        private readonly Queue<byte[]> outputQueue = new Queue<byte[]>();
        public int PackagerCount => outputQueue.Count;

        public StreamPackager()
        {
            binaryReader = new BinaryReader(memoryStream);
        }

        public void Write(byte[] data, int countBytes)
        {
            lock (memoryStream)
            {
                memoryStream.Write(data, 0, countBytes);
                memoryStream.Seek(0, SeekOrigin.Begin);

                while (memoryStream.Position < memoryStream.Length)
                {
                    int size = binaryReader.ReadInt32();
                    outputQueue.Enqueue(binaryReader.ReadBytes(size));
                }
            }
            memoryStream.SetLength(memoryStream.Length - memoryStream.Position);
            memoryStream.Seek(0, SeekOrigin.Begin);
        }

        public byte[] Read()
        {
                return outputQueue.Dequeue();
        }
    }
}
