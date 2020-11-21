using AutoMapper;
using FluentValidation.Results;
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

            CreateMap<ValidationFailure, FieldValidationErrorDto>()
                .ReverseMap();

            CreateMap<User, LoginResponse>()
                .ReverseMap();
        }
    }
}
