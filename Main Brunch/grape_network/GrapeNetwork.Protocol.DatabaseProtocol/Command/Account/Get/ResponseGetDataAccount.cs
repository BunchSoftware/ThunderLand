using GrapeNetwork.Core;
using GrapeNetwork.Core.Client;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.IO;

namespace GrapeNetwork.Protocol.DatabaseProtocol.Command.Account.Get
{
    public class ResponseGetDataAccount : ApplicationCommand
    {
        public ResponseGetDataAccount(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            Action<string> DebugInfo = data[0] as Action<string>;
            Action<Package> SendPackage = data[1] as Action<Package>;
            string IPAdressClient = data[2] as string;

            if (DebugInfo != null)
            {

            }
        }
        public static ResponseGetDataAccount DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseGetDataAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
