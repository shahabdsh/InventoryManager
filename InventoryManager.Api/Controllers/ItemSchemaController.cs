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
    public class ItemSchemaController : ControllerBase
    {
        private readonly IItemSchemaService _itemSchemaService;
        private readonly IMapper _mapper;

        public ItemSchemaController(IItemSchemaService itemSchemaService, IMapper mapper)
        {
            _itemSchemaService = itemSchemaService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<ItemSchemaDto>> Get() =>
            _mapper.Map<List<ItemSchemaDto>>(_itemSchemaService.Get());

        [HttpGet("{id:length(24)}", Name = "GetItemSchema")]
        public ActionResult<ItemSchemaDto> Get(string id)
        {
            var itemSchema = _itemSchemaService.Get(id);

            if (itemSchema == null)
            {
                return NotFound();
            }

            var itemSchemaDto = _mapper.Map<ItemSchemaDto>(itemSchema);

            return itemSchemaDto;
        }

        [HttpPost]
        public ActionResult<ItemSchemaDto> Create(ItemSchemaDto itemSchemaDto)
        {
            var itemSchema = _mapper.Map<ItemSchema>(itemSchemaDto);
            
            itemSchema.CreatedOn = DateTimeOffset.Now;

            var created = _itemSchemaService.Create(itemSchema);

            var newItemSchemaDto = _mapper.Map<ItemSchemaDto>(created);

            return CreatedAtRoute("GetItemSchema", new { id = created.Id.ToString() }, newItemSchemaDto);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ItemSchemaDto itemSchemaDto)
        {
            var existing = _itemSchemaService.Get(id);

            if (existing == null)
            {
                return NotFound();
            }

            var itemSchema = _mapper.Map<ItemSchema>(itemSchemaDto);
            
            itemSchema.UpdatedOn = DateTimeOffset.Now;

            _itemSchemaService.Update(id, itemSchema);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var itemSchema = _itemSchemaService.Get(id);

            if (itemSchema == null)
            {
                return NotFound();
            }

            _itemSchemaService.Remove(itemSchema.Id);

            return NoContent();
        }
    }
}