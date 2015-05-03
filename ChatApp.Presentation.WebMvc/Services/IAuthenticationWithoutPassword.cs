namespace ChatApp.Presentation.WebMvc.Services
{
    public interface IAuthenticationWithoutPassword
    {
        bool IsAuthenticated();

        string GetUsername();

        void SignIn(string username);

        void SignOut();
    }
}