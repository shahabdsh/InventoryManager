using System;
using System.Collections.Generic;
using AutoMapper;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Data layer entity type</typeparam>
    /// <typeparam name="U">Dto type</typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryBasedController<T, U> : ControllerBase 
        where T : EntityBase 
        where U : EntityBase
    {
        private readonly IGenericRepositoryService<T> _itemService;
        private readonly IMapper _mapper;

        public RepositoryBasedController(IGenericRepositoryService<T> itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<U>> Get() =>
            _mapper.Map<List<U>>(_itemService.Get());

        [HttpGet("{id:length(24)}")]
        public ActionResult<U> Get(string id)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            var itemDto = _mapper.Map<U>(item);

            return itemDto;
        }

        [HttpPost]
        public ActionResult<U> Create(U itemDto)
        {
            var item = _mapper.Map<T>(itemDto);
            
            item.CreatedOn = DateTimeOffset.Now;

            var created = _itemService.Create(item);

            var newItemDto = _mapper.Map<U>(created);

            return CreatedAtRoute("GetItem", new { id = created.Id.ToString() }, newItemDto);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, U itemDto)
        {
            var existing = _itemService.Get(id);

            if (existing == null)
            {
                return NotFound();
            }

            var item = _mapper.Map<T>(itemDto);
            
            item.UpdatedOn = DateTimeOffset.Now;

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