using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.Configurator.Interfaces
{
    public interface IConfig
    {
        public object? GetSection(string key);
        public T GetSection<T>(string key);
        public void ChangeValueSection(string key, object value);
    }
}
