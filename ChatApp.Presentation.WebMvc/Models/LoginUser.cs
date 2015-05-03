using System.ComponentModel.DataAnnotations;

namespace ChatApp.Presentation.WebMvc.Models
{
    public class LoginUser
    {
        public LoginUser()
        {
            Username = "";
        }

        [Required]
        public string Username { get; set; }
    }
}