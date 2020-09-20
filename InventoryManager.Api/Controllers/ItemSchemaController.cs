using System.Collections.Generic;
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

        public ItemSchemaController(IItemSchemaService itemSchemaService)
        {
            _itemSchemaService = itemSchemaService;
        }

        [HttpGet]
        public ActionResult<List<ItemSchema>> Get() =>
            _itemSchemaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetItemSchema")]
        public ActionResult<ItemSchema> Get(string id)
        {
            var itemSchema = _itemSchemaService.Get(id);

            if (itemSchema == null)
            {
                return NotFound();
            }

            return itemSchema;
        }

        [HttpPost]
        public ActionResult<ItemSchema> Create(ItemSchema itemSchema)
        {
            var created = _itemSchemaService.Create(itemSchema);

            return CreatedAtRoute("GetItemSchema", new { id = created.Id.ToString() }, itemSchema);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ItemSchema itemSchemaIn)
        {
            var itemSchema = _itemSchemaService.Get(id);

            if (itemSchema == null)
            {
                return NotFound();
            }

            _itemSchemaService.Update(id, itemSchemaIn);

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