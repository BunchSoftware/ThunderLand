using GrapeNetwork.Protocol.LoginProtocol.Command.Lobby;
using GrapeNetwork.Protocol.LoginProtocol.Command.Registration;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.LoginServer.Service
{
    public class RegistrationService : BaseService
    {
        public RegistrationService(string nameService) : base(nameService)
        {
        }

        protected override void DistrubuteCommandProcessing(CommandProcessing commandProcessing, ClientState clientState)
        {
            switch (commandProcessing.Command)
            {
                case 4:
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        BinaryReader binaryReader = new BinaryReader(memoryStream);
                        memoryStream.Write(commandProcessing.CommandData);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        string login = binaryReader.ReadString();
                        string password = binaryReader.ReadString();
                        if (login == "Den4o" && password == "win")
                        {
                            server.DebugInfo($"Такой клиент уже зарегистрирован !");
                            ResponseRejectedRegistrationUser commandProcessingResponseRejectedRegistrationUser = new ResponseRejectedRegistrationUser(1, 6, "RegistrationService");
                            commandProcessingResponseRejectedRegistrationUser.Connection = clientState.connection;
                            SendCommandProcessing(commandProcessingResponseRejectedRegistrationUser);
                        }
                        else
                        {
                            server.DebugInfo($"Зарегистрирован новый акаунт !");
                            ResponseUserRegistrationConfirmation commandProcessingResponseUserRegistrationConfirmation = new ResponseUserRegistrationConfirmation(1, 5, "RegistrationService");
                            commandProcessingResponseUserRegistrationConfirmation.Connection = clientState.connection;
                            SendCommandProcessing(commandProcessingResponseUserRegistrationConfirmation);
                        }
                        break;
                    }
            };
        }
    }
}
