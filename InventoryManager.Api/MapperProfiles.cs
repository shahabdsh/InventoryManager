using AutoMapper;
using InventoryManager.Api.Dtos;
using InventoryManager.Api.Models;

namespace InventoryManager.Api
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Item, ItemDto>()
                .ReverseMap();

            CreateMap<ItemSchema, ItemSchemaDto>()
                .ReverseMap();
        }
    }
}