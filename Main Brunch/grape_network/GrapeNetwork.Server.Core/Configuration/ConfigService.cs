using GrapeNetwork.Configurator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.Server.Core.Configuration
{
    public class ConfigService : IConfig
    {
        private Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        private string NameService;


        public ConfigService() 
        {
            keyValuePairs.Add("NameService", NameService);
        }

        public void ChangeValueSection(string key, object value)
        {
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                if (keyValuePairs.ContainsKey(key))
                    keyValuePairs[key] = value;
            }
        }

        public object GetSection(string key)
        {
            if (keyValuePairs.ContainsKey(key) == false)
                throw new Exception($"Ключ {key} отсутствует, проверьте правильность ключа");
            return keyValuePairs[key];
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
