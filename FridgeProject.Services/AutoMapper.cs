using AutoMapper;

namespace FridgeProject.Services
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Data.Models.User, Abstract.Data.User>().ReverseMap(); 
            CreateMap<Data.Models.Fridge, Abstract.Data.Fridge>().ReverseMap();
            CreateMap<Data.Models.FridgeModel, Abstract.Data.FridgeModel>().ReverseMap();
            CreateMap<Data.Models.Product, Abstract.Data.Product>().ReverseMap();
            CreateMap<Data.Models.FridgeProduct, Abstract.Data.FridgeProduct>().ReverseMap();
        }
    }
}
