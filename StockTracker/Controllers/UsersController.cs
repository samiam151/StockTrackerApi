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
    public class UsersController : EntityController<User>
    {
        public UsersController(DbContext context) : base(context)
        {
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search(string email)
        {
            return Ok(dbSet.Where(u => u.email == email));
        }
    }
}
