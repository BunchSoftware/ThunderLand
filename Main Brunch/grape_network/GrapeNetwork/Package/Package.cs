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
        // ������ �� ������������ � ��������� RSA �����
        public bool AuthAndGetRSAKey;
        // ������ �� ����������
        public bool Shutdown;
        // ������ �� ��������������� � ������� ������
        public bool ReconnectionOtherServer;
        // GroupCommand = 0 ���������� ������ � �������. 
        public ushort GroupCommand = 0;
        // Command = 0 ���������� �����-���� �������. 
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
