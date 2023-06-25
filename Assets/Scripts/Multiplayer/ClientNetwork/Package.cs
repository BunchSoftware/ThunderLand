using System;
using UnityEngine;

[Serializable]
public class Package
{
    public StatusCode StatusCode;
    public PackageHeaders Headers;
    public string? ContentType;
    public string? Content;

    public Package()
    {
        StatusCode = StatusCode.OK;
        Headers = new PackageHeaders();
    }
}
