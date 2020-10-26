using AutoMapper;
using FluentValidation;
using InventoryManager.Api.Dtos;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Api.Controllers
{
    public class ItemSchemaController : RepositoryBasedController<ItemSchema, ItemSchemaDto>
    {
        private const string GetRouteName = nameof(ItemSchemaController);

        public ItemSchemaController(IItemSchemaService schemaRepository, IMapper mapper, IValidator<ItemSchema> validator) :
            base(schemaRepository, mapper, validator)
        {
        }

        [HttpGet("{id:length(24)}", Name = GetRouteName)]
        public override ActionResult<ItemSchemaDto> GetOne(string id)
        {
            return GetOneBase(id);
        }

        public override ActionResult<ItemSchemaDto> Create(ItemSchemaDto itemDto)
        {
            return CreateBase(GetRouteName, itemDto);
        }
    }
}
