using GrapeNetwork.Server.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.InterpretatorConfiguration
{
    public class Configurator
    {
        public ConfigServer GetConfigServer()
        {
            return new ConfigServer();
        }
    }
}
