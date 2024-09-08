using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.IO;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Registration
{
    public class RequestRegistrationUser : ApplicationCommand
    {
        public RequestRegistrationUser(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            ClientState clientState = data[0] as ClientState;
            Server.Core.Server server = data[1] as Server.Core.Server;
            Action<ApplicationCommand> action = data[2] as Action<ApplicationCommand>;

            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Write(CommandData);
            memoryStream.Seek(0, SeekOrigin.Begin);
            string login = binaryReader.ReadString();
            string password = binaryReader.ReadString();

            if (login == "Den4o" && password == "win")
            {
                server.DebugInfo($"Такой клиент уже зарегистрирован !");
                ResponseRejectedRegistrationUser commandProcessingResponseRejectedRegistrationUser = new ResponseRejectedRegistrationUser(1, 6, "RegistrationService");
                commandProcessingResponseRejectedRegistrationUser.Connection = clientState.connection;
                action.Invoke(commandProcessingResponseRejectedRegistrationUser);
            }
            else
            {
                server.DebugInfo($"Зарегистрирован новый акаунт !");
                ResponseUserRegistrationConfirmation commandProcessingResponseUserRegistrationConfirmation = new ResponseUserRegistrationConfirmation(1, 5, "RegistrationService");
                commandProcessingResponseUserRegistrationConfirmation.Connection = clientState.connection;
                action.Invoke(commandProcessingResponseUserRegistrationConfirmation);
            }
        }
        public static RequestRegistrationUser DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(RequestRegistrationUser command)
        {
            throw new NotImplementedException();
        }
    }
}
