using GrapeNetwork.Configurator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrapeNetwork.Server.Login.Service
{
    public class LoginServiceRepository : IRepository<Core.Service>
    {
        private Dictionary<string, Core.Service> keyValuePairs = new Dictionary<string, Core.Service>();

        public LoginServiceRepository()
        {
            keyValuePairs.Add("AuthenticationService", new AuthenticationService("AuthenticationService"));
            keyValuePairs.Add("LobbyService", new LobbyService("LobbyService"));
            keyValuePairs.Add("RegistrationService", new RegistrationService("RegistrationService"));
        }

        public Core.Service GetObject(string key)
        {
            if (keyValuePairs.ContainsKey(key) == false)
                throw new Exception($"Ключ {key} отсутствует, проверьте правильность ключа");
            return keyValuePairs[key];
        }

        public IEnumerable<Core.Service> GetObjectList()
        {
            return keyValuePairs.Values.ToList();
        }
    }
}