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

        public void SetIPConnection(IPAddress ipAdress)
        {

        }
    }
}
