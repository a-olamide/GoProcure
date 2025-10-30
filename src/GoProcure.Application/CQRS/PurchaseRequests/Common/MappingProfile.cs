using AutoMapper;
using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using GoProcure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Common
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PurchaseRequest, PurchaseRequestDto>()
                .ForMember(d => d.Total, opt => opt.MapFrom(s => s.Total.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Total.Currency));
        }
    }
}
