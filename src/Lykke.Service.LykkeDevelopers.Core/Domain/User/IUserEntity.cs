namespace Lykke.Service.LykkeDevelopers.Core.Domain.User
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
