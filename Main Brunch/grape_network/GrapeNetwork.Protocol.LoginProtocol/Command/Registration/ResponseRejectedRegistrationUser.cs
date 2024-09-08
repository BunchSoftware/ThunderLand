using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Registration
{
    public class ResponseRejectedRegistrationUser : ApplicationCommand
    {
        public ResponseRejectedRegistrationUser(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            Action<string> ErrorInfo = data[0] as Action<string>;

            if (ErrorInfo != null)
            {
                ErrorInfo?.Invoke("Такой акаунт уже зарегистрирован");
            }
        }
        public static ResponseRejectedRegistrationUser DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedRegistrationUser command)
        {
            throw new NotImplementedException();
        }
    }
}
