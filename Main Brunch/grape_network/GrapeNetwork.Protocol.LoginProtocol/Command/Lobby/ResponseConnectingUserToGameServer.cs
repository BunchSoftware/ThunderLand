using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Lobby
{
    public class ResponseConnectingUserToGameServer : ApplicationCommand
    {
        public ResponseConnectingUserToGameServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            Action<string> DebugInfo = data[0] as Action<string>;

            if (DebugInfo != null)
            {
                DebugInfo?.Invoke("Игровой сервер принял вызов !");
                DebugInfo?.Invoke("Вход в игровой мир");
                DebugInfo?.Invoke("Вход в игровой мир произошел успешно !");
            }
        }
        public static ResponseConnectingUserToGameServer DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseConnectingUserToGameServer command)
        {
            throw new NotImplementedException();
        }
    }
}
