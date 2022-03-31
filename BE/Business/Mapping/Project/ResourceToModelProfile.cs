﻿using AutoMapper;
using Business.Extensions;
using Business.Resources.Project;

namespace Business.Mapping.Project
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateProjectResource, Domain.Models.Project>()
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.OrderIndex, opt => opt.MapFrom(src => 1))
                .ForMember(x => x.Responsibilities, opt => opt.MapFrom(src => src.Responsibilities.RemoveSpaceCharacter()));

            CreateMap<UpdateProjectResource, Domain.Models.Project>()
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.Responsibilities, opt => opt.MapFrom(src => src.Responsibilities.RemoveSpaceCharacter()));
        }
    }
}
