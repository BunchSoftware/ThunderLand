using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.GameProtocol.Command.Account.Set
{
    public class ResponseSetDataAccount : ApplicationCommand
    {
        public ResponseSetDataAccount(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            Action<string> DebugInfo = data[0] as Action<string>;

            if (DebugInfo != null)
            {
                DebugInfo?.Invoke("Авторизация на сервере прошла успешна, просим напрвиться в лобби");
                DebugInfo?.Invoke("Вход в лобби");
                DebugInfo?.Invoke("Вход в лобби произошел успешно !");
            }
        }
        public static ResponseSetDataAccount DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseSetDataAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
