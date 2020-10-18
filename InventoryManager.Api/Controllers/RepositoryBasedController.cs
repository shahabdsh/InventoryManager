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
    /// <typeparam name="U">DTO type</typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class RepositoryBasedController<T, U> : ControllerBase
        where T : EntityBase
        where U : EntityBase
    {
        private readonly IRepositoryService<T> _repository;
        private readonly IMapper _mapper;

        public RepositoryBasedController(IRepositoryService<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected ActionResult<List<U>> GetBase(string query) =>
            string.IsNullOrWhiteSpace(query) ?
            _mapper.Map<List<U>>(_repository.Get()) :
            _mapper.Map<List<U>>(_repository.Get(query));

        [HttpGet]
        public ActionResult<List<U>> Get([FromQuery(Name = "query")] string query)
        {
            return GetBase(query);
        }

        protected ActionResult<List<string>> GetIdsBase(string query) =>
            string.IsNullOrWhiteSpace(query) ?
                _repository.GetIdsOnly():
                _repository.GetIdsOnly(query);

        [HttpGet("ids")]
        public ActionResult<List<string>> GetIds([FromQuery(Name = "query")] string query)
        {
            return GetIdsBase(query);
        }

        protected ActionResult<U> GetOneBase(string id)
        {
            var entity = _repository.GetOne(id);

            if (entity == null)
            {
                return NotFound();
            }

            var entityDto = _mapper.Map<U>(entity);

            return entityDto;
        }

        public abstract ActionResult<U> GetOne(string id);

        protected ActionResult<U> CreateBase(string routeName, U entityDto)
        {
            var entity = _mapper.Map<T>(entityDto);

            var created = _repository.Create(entity);

            var newEntityDto = _mapper.Map<U>(created);

            return CreatedAtRoute(routeName, new { id = created.Id }, newEntityDto);
        }

        public abstract ActionResult<U> Create(U entityDto);

        protected IActionResult UpdateBase(string id, U entityDto)
        {
            var entity = _mapper.Map<T>(entityDto);

            try
            {
                _repository.Update(id, entity);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id:length(24)}")]
        public virtual IActionResult Update(string id, U entityDto)
        {
            return UpdateBase(id, entityDto);
        }

        protected IActionResult DeleteBase(string id)
        {
            var entity = _repository.GetOne(id);

            if (entity == null)
            {
                return NotFound();
            }

            _repository.Remove(entity.Id);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public virtual IActionResult Delete(string id)
        {
            return DeleteBase(id);
        }
    }
}
