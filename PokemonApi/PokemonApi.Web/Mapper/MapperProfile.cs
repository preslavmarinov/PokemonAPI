using AutoMapper;
using PokemonApi.Data.Models;
using PokemonApi.Data.Models.Identity;
using PokemonApi.Web.Models.Location;
using PokemonApi.Web.Models.Pokemon;
using PokemonApi.Web.Models.Type;
using PokemonApi.Web.Models.User;

namespace PokemonApi.Web.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<PokemonEntity, PokemonViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.HP, opt => opt.MapFrom(src => src.HP.ToString()))
                .ForMember(dest => dest.Attack, opt => opt.MapFrom(src => src.Attack.ToString()))
                .ForMember(dest => dest.Defence, opt => opt.MapFrom(src => src.Defence.ToString()))
                .ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.Speed.ToString()))
                .ForMember(dest => dest.Generation, opt => opt.MapFrom(src => src.Generation.ToString()))
                .ForMember(dest => dest.IsLegendary, opt => opt.MapFrom(src => src.IsLegendary.ToString()))
                .ForMember(dest => dest.Types, opt => opt.MapFrom(src => src.Types.Select(y => new TypeViewModel {Id = y.Type.Id, Name = y.Type.Name }).ToArray()))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationViewModel {Id = src.Location.Id, Name = src.Location.Name }))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.Email : null));

            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<TypeEntity, TypeViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<LocationEntity, LocationViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
