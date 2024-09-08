using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.Server.Core.Configuration
{
    public class Configurator
    {
        public ConfigServer GetConfigServer()
        {
            return new ConfigServer();
        }
    }
}
