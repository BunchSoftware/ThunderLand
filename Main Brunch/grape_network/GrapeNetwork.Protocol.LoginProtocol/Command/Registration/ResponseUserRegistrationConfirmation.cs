﻿using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Registration
{
    public class ResponseUserRegistrationConfirmation : CommandProcessing
    {
        public ResponseUserRegistrationConfirmation(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static ResponseUserRegistrationConfirmation DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseUserRegistrationConfirmation command)
        {
            throw new NotImplementedException();
        }
    }
}