using GrapeNetwork.Core;
using GrapeNetwork.Packages;
using GrapeNetwork.Server.Core.Protocol;
using System.Collections.Generic;
using System.Net.Security;

namespace GrapeNetwork.Protocol.LoginProtocol
{
    public class LoginProtocol : ApplicationProtocol
    {
        public LoginProtocol()
        {
            commandRegistry = new List<ApplicationCommand>
            {
                new Command.Authentication.RequestConnectToLoginServer(1, 1, "AuthenticationService"),
                new Command.Registration.RequestRegistrationUser(1, 4, "RegistrationService"),
                new Command.Lobby.RequestToGameServerForUserConnection(1, 7, "LobbyService"),
                new Command.Authentication.ResponseSendClientToLobby(1, 2, "AuthenticationService"),
                new Command.Authentication.ResponseRejectedLobbyConnection(1, 3, "AuthenticationService"),
                new Command.Registration.ResponseUserRegistrationConfirmation(1, 5, "RegistrationService"),
                new Command.Registration.ResponseRejectedRegistrationUser(1, 6, "RegistrationService"),
                new Command.Lobby.ResponseRejectedUserConnectionToGameServer(1, 8, "LobbyService"),
                new Command.Lobby.ResponseConnectingUserToGameServer(1, 9, "LobbyService"),
            };
        }
    }
}
