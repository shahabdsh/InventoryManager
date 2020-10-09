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
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : RepositoryBasedController<Item, ItemDto>
    {
        public ItemController(IItemService itemService, IMapper mapper) : base(itemService, mapper)
        {
        }
    }
}