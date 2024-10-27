using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.Configurator.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetObjectList();
        T GetObject(string key);
    }
}
