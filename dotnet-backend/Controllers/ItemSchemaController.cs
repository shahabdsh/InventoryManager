using System.Linq;
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
        private readonly IItemService _itemService;
        private const string GetRouteName = nameof(ItemSchemaController);

        public ItemSchemaController(IItemSchemaService schemaService,
            IMapper mapper,
            IValidator<ItemSchema> validator,
            IItemService itemService) :
            base(schemaService, mapper, validator)
        {
            _itemService = itemService;
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

        public override IActionResult Delete(string id)
        {
            if (_itemService.Queryable().Any(item => item.SchemaId == id))
            {
                return BadRequest(new GenericBadRequestResponseDto
                {
                    Error = "itemsDependentOnSchemaExist",
                    Description = "Can not delete this item schema because existing items depend on it."
                });
            }

            return base.Delete(id);
        }
    }
}
