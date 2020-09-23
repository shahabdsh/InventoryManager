using System.Collections.Generic;
using System.Linq;
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

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult<List<Item>> Get() =>
            _itemService.Get();

        [HttpGet("{id:length(24)}", Name = "GetItem")]
        public ActionResult<Item> Get(string id)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public ActionResult<Item> Create(ItemDto itemDto)
        {
            var item = new Item
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Type = itemDto.Type,
                Quantity = itemDto.Quantity,
                Properties = (Dictionary<string, object>)itemDto.Properties
            };
            
            var created = _itemService.Create(item);

            return CreatedAtRoute("GetItem", new { id = created.Id.ToString() }, item);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ItemDto itemDto)
        {
            var existing = _itemService.Get(id);

            if (existing == null)
            {
                return NotFound();
            }
            
            var item = new Item
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Type = itemDto.Type,
                Quantity = itemDto.Quantity,
                Properties = (Dictionary<string, object>)itemDto.Properties
            };

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

        private Dictionary<string, object> ConvertDict(Dictionary<string, string> input)
        {
            
        }
    }
}