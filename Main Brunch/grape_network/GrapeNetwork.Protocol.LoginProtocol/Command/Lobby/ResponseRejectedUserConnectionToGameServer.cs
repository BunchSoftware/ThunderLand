using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Lobby
{
    public class ResponseRejectedUserConnectionToGameServer : ApplicationCommand
    {
        public ResponseRejectedUserConnectionToGameServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            Action<string> ErrorInfo = data[0] as Action<string>;

            if (ErrorInfo != null)
            {
                ErrorInfo?.Invoke("Запрос игровым сервером был отклонен, попробуйте еще раз !");
            }
        }
        public static ResponseRejectedUserConnectionToGameServer DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedUserConnectionToGameServer command)
        {
            throw new NotImplementedException();
        }
    }
}
