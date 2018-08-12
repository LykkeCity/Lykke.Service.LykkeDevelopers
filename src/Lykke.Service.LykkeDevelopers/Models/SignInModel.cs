using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
