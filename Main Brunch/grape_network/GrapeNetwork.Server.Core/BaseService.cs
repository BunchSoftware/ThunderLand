using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core
{
    public class BaseService
    {
        public string nameService { get; }
        protected BaseServer server;
        protected Queue<Task<CommandProcessing>> queueTaskCommandProcessing = new Queue<Task<CommandProcessing>>();
        public event Action<CommandProcessing> OnSendCommandProcessing;
        protected event Action<Exception> OnExceptionInfo;
        protected event Action<string> OnDebugInfo;

        public BaseService(string nameService) 
        { 
            this.nameService = nameService;
        }

        public virtual void Init(BaseServer server)
        {
            this.server = server;
            this.server.TickServer += Tick;
        }

        private void Tick()
        {
            for (int i = 0; i < queueTaskCommandProcessing.Count; i++)
            {
                Task<CommandProcessing> taskCommandProcessing = queueTaskCommandProcessing.Dequeue();
                taskCommandProcessing.Start();
            }
        }
        protected void SendCommandProcessing(CommandProcessing commandProcessing)
        {
            OnSendCommandProcessing?.Invoke(commandProcessing);
        }

        public void AddCommandProcessing(CommandProcessing commandProcessing, ClientState clientState)
        {
            queueTaskCommandProcessing.Enqueue(
                  new Task<CommandProcessing>(
                      () => 
                      {
                          DistrubuteCommandProcessing(commandProcessing, clientState);
                          return commandProcessing;                      
                      })
            ); ; 
        }
        protected virtual void DistrubuteCommandProcessing(CommandProcessing commandProcessing, ClientState clientState)
        {

        }
        public virtual void ReadConfig()
        {
        }
    }
}
