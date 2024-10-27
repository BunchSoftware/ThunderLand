using GrapeNetwork.Configurator.Interfaces;
using GrapeNetwork.Server.Core.Grpc;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core.Configuration
{
    public class ConfigGrpcServer : IConfig
    {
        private Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

        private IPAddress IPAddressServer;
        private int PortServer;
        private Task RunAsync;
        private Task StopAsync;

        public ConfigGrpcServer()
        {
            keyValuePairs.Add("IPAddressServer", IPAddressServer);
            keyValuePairs.Add("PortServer", PortServer);
            keyValuePairs.Add("RunAsync", RunAsync);
            keyValuePairs.Add("StopAsync", StopAsync);
        }

        public object GetSection(string key)
        {
            if (keyValuePairs.ContainsKey(key) == false)
                throw new Exception($"Ключ {key} отсутствует, проверьте правильность ключа");
            return keyValuePairs[key];
        }

        public void ChangeValueSection(string key, object value)
        {
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                if (keyValuePairs.ContainsKey(key))
                    keyValuePairs[key] = value;
            }
        }

        public T GetSection<T>(string key)
        {
            T result = default(T);
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                if (keyValuePairs.ContainsKey(key))
                {
                    result = (T)keyValuePairs[key];
                }
            }
            return result;
        }
    }
}
