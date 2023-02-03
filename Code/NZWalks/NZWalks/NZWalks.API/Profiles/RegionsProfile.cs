using AutoMapper;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile:Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
            .ReverseMap();
        }
    }
}
