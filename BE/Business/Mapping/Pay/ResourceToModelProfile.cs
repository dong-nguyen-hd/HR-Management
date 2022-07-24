﻿using AutoMapper;
using Business.Resources.Pay;

namespace Business.Mapping.Pay
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreatePayResource, Domain.Models.Pay>();

            CreateMap<UpdatePayResource, Domain.Models.Pay>();
        }
    }
}
