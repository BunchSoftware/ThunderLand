using NLog.Fluent;
using System;
using System.Net;

namespace GrapeNetwork.Packages
{
    [Serializable]
    public class Package 
    {
        // Header
        public static byte HeaderSize = 19;
        public uint IPConnection;
        public uint IDConnection;
        // Запрос на аунтефикацию и получение RSA ключа
        public bool AuthAndGetRSAKey;
        // Запрос на отключение
        public bool Shutdown;
        // Запрос на переподключение к другому сервер
        public bool ReconnectionOtherServer;
        // GroupCommand = 0 отсутствие группы у команды. 
        public ushort GroupCommand = 0;
        // Command = 0 отсветсвие какой-либо команды. 
        public uint Command = 0;
        public ushort ChecksumHeader;

        //Body
        public ushort BodySize;
        public byte[] Body;
        public ushort ChecksumBody;

        public static uint ConvertFromIpAddressToInteger(string ipAddress)
        {
            var address = IPAddress.Parse(ipAddress);
            byte[] bytes = address.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        public static string ConvertFromIntegerToIpAddress(uint ipAddress)
        {
            byte[] bytes = BitConverter.GetBytes(ipAddress);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return new IPAddress(bytes).ToString();
        }
    }
}
