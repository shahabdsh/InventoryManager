using System;
using System.Collections.Generic;
using AutoMapper;
using InventoryManager.Api.Dtos;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemSchemaController : RepositoryBasedController<ItemSchema, ItemSchemaDto>
    {
        public ItemSchemaController(IItemSchemaService itemSchemaService, IMapper mapper) : base(itemSchemaService, mapper)
        {
        }
    }
}