using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.GameProtocol.Command.Account.Get
{
    public class ResponseGetWorldState : ApplicationCommand
    {
        public ResponseGetWorldState(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            Action<string> DebugInfo = data[0] as Action<string>;

            if (DebugInfo != null)
            {
                DebugInfo?.Invoke("Получены данные об аккауенте");
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
