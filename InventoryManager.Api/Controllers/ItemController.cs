using AutoMapper;
using FluentValidation;
using InventoryManager.Api.Dtos;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Api.Controllers
{
    public class ItemController : RepositoryBasedController<Item, ItemDto>
    {
        private const string GetRouteName = nameof(ItemController);

        public ItemController(IItemService repository, IMapper mapper, IValidator<Item> validator) :
            base(repository, mapper, validator)
        {
        }

        [HttpGet("{id:length(24)}", Name = GetRouteName)]
        public override ActionResult<ItemDto> GetOne(string id)
        {
            return GetOneBase(id);
        }

        [HttpPost]
        public override ActionResult<ItemDto> Create(ItemDto itemDto)
        {
            return CreateBase(GetRouteName, itemDto);
        }
    }
}
