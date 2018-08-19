namespace Lykke.Service.LykkeDevelopers.Core.Domain.Developer
{
    public interface  IDeveloperEntity: IEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string TelegramAcc { get; set; }
        string GithubAcc { get; set; }
        string Team { get; set; }
    }
}
