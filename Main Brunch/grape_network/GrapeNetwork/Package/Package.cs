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
        public int IPConnection;
        public int IDConnection;
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

        public void SetIPConnection(IPAddress ipAdress)
        {

        }
    }
}
