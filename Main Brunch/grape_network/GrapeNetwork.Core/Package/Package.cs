using System;
using System.Net;

namespace GrapeNetwork.Core
{
    [Serializable]
    public class Package 
    {
        // Header
        public static byte HeaderSize = 14;
        public uint IPConnection;
        // Запрос на аунтефикацию и получение RSA ключа
        public bool AuthAndGetRSAKey;
        // Запрос на отключение
        public bool Shutdown;
        // GroupCommand = 0 отсутствие группы у команды. 
        public ushort GroupCommand = 0;
        // Command = 0 отсутствие какой-либо команды. 
        public uint Command = 0;
        public ushort ChecksumHeader;

        //Body
        public ushort BodySize;
        public byte[] Body;
        public ushort ChecksumBody;

        public Package() { }
        public Package(IPAddress IPAdress, ushort GroupCommand, uint Command)
        {
            IPConnection = Package.ConvertFromIpAddressToInteger(IPAdress.ToString());
            this.GroupCommand = GroupCommand;
            this.Command = Command;
        }
        public Package(string IPAddress, ushort GroupCommand, uint Command)
        {
            IPConnection = Package.ConvertFromIpAddressToInteger(IPAddress);
            this.GroupCommand = GroupCommand;
            this.Command = Command;
        }

        public static uint ConvertFromIpAddressToInteger(string IPAddress)
        {
            IPAddress address = System.Net.IPAddress.Parse(IPAddress);
            byte[] bytes = address.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        public static string ConvertFromIntegerToIpAddress(uint IPAddress)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return new IPAddress(bytes).ToString();
        }
    }
}
