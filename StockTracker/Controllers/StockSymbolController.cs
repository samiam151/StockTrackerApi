using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTracker.Controllers.Base;
using StockTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracker.Controllers
{
    public class StockSymbolController : EntityController<StockSymbol>
    {
        public StockSymbolController(DbContext context) : base(context)
        {
        }
        
        [HttpGet]
        [Route("search")]
        public ActionResult SearchSymbols(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            return Ok(dbSet.Where(sym => sym.Name.Contains(term.ToUpper()) || sym.Symbol.Contains(term.ToUpper())));
        }
    }
}
