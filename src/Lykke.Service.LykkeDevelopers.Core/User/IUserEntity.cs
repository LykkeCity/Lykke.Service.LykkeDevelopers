using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.LykkeDevelopers.Core.User
{
    public interface IUserEntity : IEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Salt { get; set; }
        string PasswordHash { get; set; }
        bool? Active { get; set; }
    }
}
