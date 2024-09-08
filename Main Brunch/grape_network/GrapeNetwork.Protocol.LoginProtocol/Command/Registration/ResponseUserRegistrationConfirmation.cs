using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Registration
{
    public class ResponseUserRegistrationConfirmation : ApplicationCommand
    {
        public ResponseUserRegistrationConfirmation(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            Action<string> DebugInfo = data[0] as Action<string>;

            if (DebugInfo != null)
            {
                DebugInfo("Регистрация прошла успешна ! Желаем хорошой игры !");
            }
        }
        public static ResponseUserRegistrationConfirmation DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseUserRegistrationConfirmation command)
        {
            throw new NotImplementedException();
        }
    }
}
