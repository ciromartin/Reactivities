using AutoMapper;

namespace Application.Mappings
{
    public interface IMapTo<T>
    {   
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
    }
}