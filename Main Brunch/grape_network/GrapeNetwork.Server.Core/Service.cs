using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Server.Core.Protocol;

namespace GrapeNetwork.Server.Core
{
    public class Service
    {
        public string nameService { get; }
        protected Server server;
        protected Queue<Task<ApplicationCommand>> queueTaskApplicationCommand = new Queue<Task<ApplicationCommand>>();
        public event Action<ApplicationCommand> OnSendApplicationCommand;

        public Service(string nameService) 
        { 
            this.nameService = nameService;
        }

        public virtual void Init(Server server)
        {
            this.server = server;
            this.server.TickServer += Tick;
        }

        private void Tick()
        {
            for (int i = 0; i < queueTaskApplicationCommand.Count; i++)
            {
                Task<ApplicationCommand> taskApplicationCommand = queueTaskApplicationCommand.Dequeue();
                taskApplicationCommand.Start();
            }
        }
        protected void SendApplicationCommand(ApplicationCommand applicationCommand)
        {
            OnSendApplicationCommand?.Invoke(applicationCommand);
        }

        public void AddApplicationCommand(ApplicationCommand applicationCommand, ClientState clientState)
        {
            queueTaskApplicationCommand.Enqueue(
                  new Task<ApplicationCommand>(
                      () => 
                      {
                          DistrubuteApplicationCommand(applicationCommand, clientState);
                          return applicationCommand;                      
                      })
            ); ; 
        }
        protected virtual void DistrubuteApplicationCommand(ApplicationCommand applicationCommand, ClientState clientState)
        {

        }
        public virtual void ReadConfig(ConfigService configService)
        {

        }
    }
}
