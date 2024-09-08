
using GrapeNetwork.Server.Core.Protocol;

namespace GrapeNetwork.Protocol.DatabaseProtocol
{
    public class DatabaseProtocol : ApplicationProtocol
    {
        public DatabaseProtocol() 
        { 
            commandRegistry = new System.Collections.Generic.List<ApplicationCommand>() 
            { 
            
            };      
        }
    }
}
