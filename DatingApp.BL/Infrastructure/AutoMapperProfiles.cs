using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet.Actions;
using DatingApp.BL.DTO;
using DatingApp.BL.DTO.LikeDTOs;
using DatingApp.BL.DTO.MessagesDTOs;
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
            
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => 
                    opt.MapFrom(src => src.Username.ToLower()));
            
            CreateMap<AppUser, LikeDto>()
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain)!.Url));

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt =>
                    opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain)!.Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt =>
                    opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.IsMain)!.Url));

        }
    }
}
