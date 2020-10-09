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
    public abstract class RepositoryBasedController<T, U> : ControllerBase
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

        
        protected ActionResult<List<U>> GetBase() =>
            _mapper.Map<List<U>>(_itemService.Get());
        
        [HttpGet]
        public ActionResult<List<U>> Get()
        {
            return GetBase();
        }
        
        protected ActionResult<U> GetBase(string id)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            var itemDto = _mapper.Map<U>(item);

            return itemDto;
        }

        public abstract ActionResult<U> Get(string id);
        
        protected ActionResult<U> CreateBase(string routeName, U itemDto)
        {
            var item = _mapper.Map<T>(itemDto);
            
            item.CreatedOn = DateTimeOffset.Now;

            var created = _itemService.Create(item);

            var newItemDto = _mapper.Map<U>(created);

            return CreatedAtRoute(routeName, new { id = created.Id }, newItemDto);
        }

        public abstract ActionResult<U> Create(U itemDto);
        
        protected IActionResult UpdateBase(string id, U itemDto)
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

        [HttpPut("{id:length(24)}")]
        public virtual IActionResult Update(string id, U itemDto)
        {
            return UpdateBase(id, itemDto);
        }
        
        protected IActionResult DeleteBase(string id)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            _itemService.Remove(item.Id);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public virtual IActionResult Delete(string id)
        {
            return DeleteBase(id);
        }
    }
}