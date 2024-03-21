using System;

namespace GrapeNetwork.Packages
{
    [Serializable]
    public class Package : IDisposable
    {
        public StatusCode StatusCode;
        public PackageHeaders Headers;
        public string ContentType;
        public string Content;

        public Package()
        {
            StatusCode = StatusCode.OK;
            Headers = new PackageHeaders();
        }

        public void Dispose()
        {
            StatusCode = StatusCode.OK;
            Headers = new PackageHeaders();
            ContentType = null;
            Content = null;
        }
    }
}
