using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using InventoryManager.Api.Dtos;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Api.Controllers
{
    public class ItemController : RepositoryBasedController<Item, ItemDto>
    {
        private const string GetRouteName = nameof(ItemController);

        public ItemController(IItemService repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [HttpGet("{id:length(24)}", Name = GetRouteName)]
        public override ActionResult<ItemDto> Get(string id)
        {
            return GetBase(id);
        }

        [HttpPost]
        public override ActionResult<ItemDto> Create(ItemDto itemDto)
        {
            return CreateBase(GetRouteName, itemDto);
        }
    }
}