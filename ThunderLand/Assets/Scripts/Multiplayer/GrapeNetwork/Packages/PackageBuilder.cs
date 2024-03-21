using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Packages
{
    public class PackageBuilder
    {
        private readonly BinaryWriter binaryWriter;
        private readonly Package package;


        public PackageBuilder(Stream stream, Package package) 
        { 
            this.binaryWriter = new BinaryWriter(stream);
            this.package = package;       
        }

        public static byte[] ToByteArray(Package package)
        {
            MemoryStream memoryStream = new MemoryStream();
            WriteToStream(memoryStream, package);
            return memoryStream.ToArray();
        }
        public static void WriteToStream(Stream stream, Package package)
        {
            PackageBuilder packageBuilder = new PackageBuilder(stream, package);
            packageBuilder.Process();
        }
        public void Process()
        {
            Process(binaryWriter, package);
        }   
        private void Process(BinaryWriter binaryWriter, Package package)
        {
            string message = JsonConvert.SerializeObject(package);
            // Избавляемся от префикса длины, который не нужен нам с помощью костыля
            byte[] messageToByte = Encoding.UTF8.GetBytes(message);
            binaryWriter.Write(messageToByte.Length);
            binaryWriter.Write(messageToByte);
        }
    }
}
