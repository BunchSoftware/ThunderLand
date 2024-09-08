using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.GameProtocol.Command.Account.Set
{
    public class ResponseRejectedSetDataAccount : ApplicationCommand
    {
        public ResponseRejectedSetDataAccount(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
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
        public static ResponseRejectedSetDataAccount DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedSetDataAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
