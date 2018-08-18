using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.LykkeDevelopers.Core.Team
{
    public interface ITeamEntity : IEntity
    {
        string Name { get; set; }
    }
}
