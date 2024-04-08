using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet.Actions;
using DatingApp.BL.DTO;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Extensions;

namespace DatingApp.BL.Infrastructure
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<UploadResult, Photo>()
                .ForMember(dest => dest.Url, opt =>
                    opt.MapFrom(src => src.SecureUrl.AbsoluteUri))
                .ForMember(dest => dest.PublicId, opt =>
                    opt.MapFrom(src => src.PublicId));

        }
    }
}
