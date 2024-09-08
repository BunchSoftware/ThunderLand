using GrapeNetwork.Core;
using GrapeNetwork.Packages;
using GrapeNetwork.Protocol.GameProtocol;
using GrapeNetwork.Protocol.GameProtocol.Command.Account.Get;
using GrapeNetwork.Protocol.GameProtocol.Command.Account.Set;
using GrapeNetwork.Server.Core.Protocol;
using System.Collections.Generic;

namespace GrapeNetwork.Protocol.GameProtocol
{
    public class GameProtocol : ApplicationProtocol
    {
        public GameProtocol() 
        {
            commandRegistry = new List<ApplicationCommand>
            {
                new RequestGetDataAccount(2, 1, "AccountService"),
                new ResponseGetDataAccount(2, 2, "AccountService"),
                new ResponseRejectedGetDataAccount(2, 3, "AccountService"),
                new RequestSetDataAccount(2, 4, "AccountService"),
                new ResponseSetDataAccount(2, 5, "AccountService"),
                new ResponseRejectedSetDataAccount(2, 6, "AccountService"),
                new RequestGetWorldState(2, 7, "MapService"),
                new ResponseGetWorldState(2, 8, "MapService"),
                new ResponseRejectedGetWorldState(2, 9, "MapService"),
            };
        }
    }
}
