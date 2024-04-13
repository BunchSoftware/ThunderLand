using GrapeNetwork.Packages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Core.Server
{
    public class TransportProtocol
    {
        private readonly MemoryStream memoryStreamRead = new MemoryStream();
        private readonly MemoryStream memoryStreamWrite = new MemoryStream();

        private readonly BinaryReader binaryReader;
        private readonly BinaryWriter binaryWriter;

        private readonly Queue<Package> outputQueue = new Queue<Package>();
        public int RecievePackageCount => outputQueue.Count;

        public TransportProtocol()
        {
            binaryReader = new BinaryReader(memoryStreamRead);
            binaryWriter = new BinaryWriter(memoryStreamWrite);
        }
        public virtual void CreatePackage(byte[] data)
        {
            int countData = data.Length;
            while (countData != 0)
            {
                try
                {
                    byte[] dataPackage = new byte[countData];
                    Array.Copy(data, data.Length - countData, dataPackage, 0, countData);
                    Package package = ParseHeader(dataPackage);
                    package = ParseBody(package);
                    outputQueue.Enqueue(package);
                    countData -= (int)memoryStreamRead.Position;
                    memoryStreamRead.SetLength(0);
                }
                catch 
                {
                    return;
                }
            }
        }
        public Package GetLastPackage()
        {
            if(RecievePackageCount != 0)
                return outputQueue.Dequeue();
            else
                return null;
        }
        public virtual byte[] CreateBinaryData(Package package)
        {
            List<byte> data = new List<byte>();
            data.AddRange(CreateHeader(package));
            data.AddRange(CreateBody(package));
            memoryStreamWrite.SetLength(0);
            return data.ToArray();
        }
        protected byte[] CreateHeader(Package package)
        {
            binaryWriter.Write(Package.HeaderSize);
            binaryWriter.Write(package.IPConnection);
            binaryWriter.Write(package.IDConnection);
            binaryWriter.Write(package.AuthAndGetRSAKey);
            binaryWriter.Write(package.Shutdown);
            binaryWriter.Write(package.ReconnectionOtherServer);
            binaryWriter.Write(package.GroupCommand);
            binaryWriter.Write(package.Command);
            ushort ChecksumHeader =  CRC16.ComputeChecksum(memoryStreamWrite.ToArray());
            binaryWriter.Write(ChecksumHeader);

            return memoryStreamWrite.ToArray();
        }
        protected byte[] CreateBody(Package package)
        {
            memoryStreamWrite.SetLength(memoryStreamWrite.Length - memoryStreamWrite.Position);
            memoryStreamWrite.Seek(0, SeekOrigin.Begin);
            if(package.Body != null)
            {
                binaryWriter.Write((ushort)package.Body.Length);
                binaryWriter.Write(package.Body);
            }
            else
            {
                binaryWriter.Write(0);
                memoryStreamWrite.Seek(2, SeekOrigin.Current);
            }
            binaryWriter.Write(CRC16.ComputeChecksum(package.Body));
            return memoryStreamWrite.ToArray();
        }
        protected Package ParseHeader(byte[] data)
        {
            Package package = new Package();
            lock (memoryStreamRead)
            {
                memoryStreamRead.Write(data, 0, 18);
                // Пропускаем байт длины загаловка
                memoryStreamRead.Seek(1, SeekOrigin.Begin);

                package.IPConnection = binaryReader.ReadInt32();
                package.IDConnection = binaryReader.ReadInt32();
                package.AuthAndGetRSAKey = binaryReader.ReadBoolean();
                package.Shutdown = binaryReader.ReadBoolean();
                package.ReconnectionOtherServer = binaryReader.ReadBoolean();
                package.GroupCommand = binaryReader.ReadUInt16();
                package.Command = binaryReader.ReadUInt32();
                ushort ChecksumHeader = CRC16.ComputeChecksum(memoryStreamRead.ToArray());
                memoryStreamRead.Write(data, 18, 2);
                memoryStreamRead.Seek(-2, SeekOrigin.Current);
                package.ChecksumHeader = binaryReader.ReadUInt16();
                if (ChecksumHeader == package.ChecksumHeader)
                {
                    memoryStreamRead.SetLength(0);
                    memoryStreamRead.Write(data);
                    memoryStreamRead.Seek(20, SeekOrigin.Begin);
                    return package;
                }
                else
                    throw new Exception();
            }
        }
        protected Package ParseBody(Package package)
        {
            lock (memoryStreamRead)
            {
                package.BodySize = binaryReader.ReadUInt16();
                package.Body = binaryReader.ReadBytes(package.BodySize);
                ushort ChecksumBody = CRC16.ComputeChecksum(package.Body);
                package.ChecksumBody = binaryReader.ReadUInt16();
                if (ChecksumBody == package.ChecksumBody)
                    return package;
                else
                    throw new Exception();
            }
        }
    }
}
