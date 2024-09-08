using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Authentication
{
    public class ResponseRejectedLobbyConnection : ApplicationCommand
    {
        public ResponseRejectedLobbyConnection(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            Action<string> ErrorInfo = data[0] as Action<string>;

            if (ErrorInfo != null)
            {
                ErrorInfo?.Invoke("Логин или пароль были не верны, проверьте правильность вводимых данных");
            }
        }
        public static ResponseRejectedLobbyConnection DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedLobbyConnection command)
        {
            throw new NotImplementedException();
        }
    }
}
