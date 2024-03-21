namespace GrapeNetwork.Packages
{
    public class PackageProcessingCondition
    {
        private StatusCode StatusCode;
        private PackageHeaders Headers;
        private string ContentType;

        public PackageProcessingCondition(StatusCode StatusCode, PackageHeaders Headers, string ContentType)
        {
            this.StatusCode = StatusCode;
            this.Headers = Headers;
            this.ContentType = ContentType;
        }

        public bool CheckCondition(Package package)
        {
            if (package.StatusCode == StatusCode
                && package.Headers.Connection == Headers.Connection
                && package.Headers.Server == Headers.Server
                && package.Headers.Authorization == Headers.Authorization
                && package.ContentType == ContentType)
                return true;

            return false;
        }
    }
}
