using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTracker.Extensions;
using StockTracker.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracker.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EntityController<T> : ControllerBase where T : Entity
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<T> dbSet;

        public EntityController(DbContext context)
        {
            dbContext = context;
            dbSet = dbContext.ResolveDbSet<T>() as DbSet<T>;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllEntities()
        {
            var query = dbSet.AsQueryable();
            var result = query.IncludeAll<T>(typeof(T));
            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEntity(int id)
        {
            var query = dbSet.AsQueryable();
            T result = query.FirstOrDefault(e => e.Id == id);
            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddEntity(T newEntity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                dbSet.Add(newEntity);
                var result = await dbContext.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = newEntity?.Id }, newEntity);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateEntity(T entity)
        {
            try
            {
                var e = await dbSet.FindAsync(entity.Id);
                if (e == null)
                {
                    return NotFound();
                }
                dbContext.UpdateEntity<T>(entity);
                await dbContext.SaveChangesAsync();
                return Ok(entity);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteEntity(int id)
        {
            try
            {
                var e = await dbSet.FindAsync(id);
                if (e == null)
                {
                    return NotFound();
                }
                dbSet.Remove(e);
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}
