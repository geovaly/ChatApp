namespace ChatApp.Presentation.WebMvc.Services.Impl
{
    public class GlobalUserAuthenticationWithoutPassword : IAuthenticationWithoutPassword
    {
        private static string _username = "";

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(_username);
        }

        public string GetUsername()
        {
            return _username;
        }

        public void SignIn(string username)
        {
            _username = username;
        }

        public void SignOut()
        {
            _username = "";
        }
    }
}