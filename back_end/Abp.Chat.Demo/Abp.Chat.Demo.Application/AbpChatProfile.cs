using Abp.Chat.Demo.Contract.Dto;
using Abp.Chat.Demo.Domain;
using AutoMapper;

namespace Abp.Chat.Demo.Application
{
    internal class AbpChatProfile : Profile
    {
        public AbpChatProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RedisUserDto>()
                .ForMember(dest => dest.Id, options =>
                {
                    options.MapFrom(src => src.Id);
                })
                .ReverseMap();
        }
    }
}
