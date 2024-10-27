using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.Common.Interfaces
{
    public interface IDatabase
    {
        void Create<T>(T item);
        void Update<T>(T item);
        void Delete(int id);
        void Save();
    }
}
