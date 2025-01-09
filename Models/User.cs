using LoginForm.Enums;

namespace LoginForm.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
    }
}
