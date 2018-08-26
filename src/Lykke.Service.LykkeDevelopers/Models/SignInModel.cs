using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.LykkeDevelopers.Models
{
    public class SignInModel
    {
        [Required]
        [DisplayName("E-mail")]
        public string Email;

        [Required]
        [DisplayName("Password")]
        public string Password;

    }
}
