
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PackageHeaders 
{
    public string? Language;
    public string Data { get; }
    public string? Server;
    public string? Authorization;

    public PackageHeaders()
    {
       Data = $"{DateTime.UtcNow.DayOfWeek}, {DateTime.UtcNow.Day}/{DateTime.UtcNow.Month}/{DateTime.UtcNow.Year} {DateTime.UtcNow.Hour}:{DateTime.UtcNow.Minute}:{DateTime.UtcNow.Second} UTC";
    }
}
