using AutoMapper;
using Lykke.Service.LykkeDevelopers.Client.Models;
using Lykke.Service.LykkeDevelopers.Core.Domain.Developer;
using Lykke.Service.LykkeDevelopers.Core.Domain.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.LykkeDevelopers.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<IDeveloperEntity, DeveloperModel>(MemberList.Source);
            CreateMap<DeveloperModel, IDeveloperEntity>(MemberList.Source);
            CreateMap<ITeamEntity, TeamModel>(MemberList.Source);
        }
    }
}
