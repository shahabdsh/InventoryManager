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
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        public ItemController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<ItemDto>> Get() =>
            _mapper.Map<List<ItemDto>>(_itemService.Get());

        [HttpGet("{id:length(24)}", Name = "GetItem")]
        public ActionResult<ItemDto> Get(string id)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            var itemDto = _mapper.Map<ItemDto>(item);

            return itemDto;
        }

        [HttpPost]
        public ActionResult<ItemDto> Create(ItemDto itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);

            var created = _itemService.Create(item);

            var newItemDto = _mapper.Map<ItemDto>(created);

            return CreatedAtRoute("GetItem", new { id = created.Id.ToString() }, newItemDto);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ItemDto itemDto)
        {
            var existing = _itemService.Get(id);

            if (existing == null)
            {
                return NotFound();
            }

            var item = _mapper.Map<Item>(itemDto);

            _itemService.Update(id, item);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            _itemService.Remove(item.Id);

            return NoContent();
        }
    }
}