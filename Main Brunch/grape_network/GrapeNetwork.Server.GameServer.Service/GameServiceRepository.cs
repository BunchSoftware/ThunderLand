using GrapeNetwork.Configurator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrapeNetwork.Server.Game.Service
{
    public class GameServiceRepository : IRepository<Core.Service>
    {
        private Dictionary<string, Core.Service> keyValuePairs = new Dictionary<string, Core.Service>();

        public GameServiceRepository() 
        {
            keyValuePairs.Add("AccountService", new AccountService("AccountService"));
            //keyValuePairs.Add("AuctionService", new AuctionService("AccountService"));
            //keyValuePairs.Add("BotService", new AccountService("AccountService"));
            //keyValuePairs.Add("ChatService", new AccountService("AccountService"));
            //keyValuePairs.Add("InventoryService", new AccountService("AccountService"));
            keyValuePairs.Add("MapService", new MapService("MapService"));
            //keyValuePairs.Add("WeatherService", new AccountService("AccountService"));
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
