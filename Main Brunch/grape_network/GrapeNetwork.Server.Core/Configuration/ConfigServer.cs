using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using GrapeNetwork.Configurator.Interfaces;
using GrapeNetwork.Server.Core.Grpc;
using GrapeNetwork.Server.Core.Protocol;

namespace GrapeNetwork.Server.Core.Configuration
{
    public class ConfigServer : IConfig
    {
        private Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

        private ApplicationProtocol ApplicationProtocol;
        private List<ConfigService> InternalServicesConfig;
        private List<ConfigService> ExternalServicesConfig;
        private List<ConfigCommunicationClient> ConfigCommunicationClients;
        private List<IRepository<Core.Service>> RepositoryServices;
        private string NameServer;
        private IPAddress IPAdressServer;
        private int PortServer;
        private GrpcServer GrpcServer;
        private bool IsLog = true;

        public ConfigServer()
        {
            keyValuePairs.Add("ApplicationProtocol", ApplicationProtocol);
            keyValuePairs.Add("InternalServicesConfig", InternalServicesConfig);
            keyValuePairs.Add("ExternalServicesConfig", ExternalServicesConfig);
            keyValuePairs.Add("RepositoryServices", RepositoryServices);
            keyValuePairs.Add("NameServer", NameServer);
            keyValuePairs.Add("IPAddressServer", IPAdressServer);
            keyValuePairs.Add("PortServer", PortServer);
            keyValuePairs.Add("ConfigCommunicationServices", ConfigCommunicationClients);
            keyValuePairs.Add("GrpcServer", GrpcServer);
            keyValuePairs.Add("IsLog", IsLog);
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
                {
                    keyValuePairs[key] = value;
                    return;
                }
            }
            throw new Exception($"Ключ {key} отсутствует, проверьте правильность ключа");
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
