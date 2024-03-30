using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server
{
    public interface ICommandServer
    {
        public bool Start();
        public void Stop();
    }
}
