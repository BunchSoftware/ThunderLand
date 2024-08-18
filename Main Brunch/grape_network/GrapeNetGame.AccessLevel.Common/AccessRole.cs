

namespace GrapeNetwork.AccessLevel.Common
{
    public class AccessRole
    {
        private string Login;
        private string Password;
        private bool IsAdministrator;

        public string NameUser { get; }
        private string URLAvatar;
        public string Status => IsAdministrator ? "Администратор" : "Пользователь";

        public bool GetIsAdministartor()
        {
            return IsAdministrator;
        }
    }
}
