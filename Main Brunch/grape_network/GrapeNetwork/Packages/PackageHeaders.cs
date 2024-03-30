using NLog;
using NLog.Fluent;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GrapeNetwork.Packages
{
    [Serializable]
    public class PackageHeaders
    {
        public string Language;
        public string Date { get; }
        public string Connection;
        public string Server;
        public string Authorization;

        public PackageHeaders()
        {
            Date = $"{DateTime.UtcNow.DayOfWeek}, {DateTime.UtcNow.Day}/{DateTime.UtcNow.Month}/{DateTime.UtcNow.Year} {DateTime.UtcNow.Hour}:{DateTime.UtcNow.Minute}:{DateTime.UtcNow.Second} UTC";
        }
    }
}
