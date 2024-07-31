using AutoMapper;
using POS_ITS.MODEL.DTOs.ProductDTOs;
using POS_ITS.MODEL.DTOs.UserDTOs;
using POS_ITS.MODEL.Entities;

namespace POS_ITS.API.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<UserDTO, User>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
