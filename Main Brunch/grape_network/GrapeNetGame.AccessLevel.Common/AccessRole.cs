

namespace GrapeNetwork.AccessLevel.Common
{
    public class AccessRole
    {
        private string Login;
        private string Password;
        private bool IsAdministrator = true;

        public string NameUser { get; }
        private string URLAvatar;
        public string Status => IsAdministrator ? "Administrator" : "User";

        public bool GetIsAdministartor()
        {
            return IsAdministrator;
        }
    }
}
