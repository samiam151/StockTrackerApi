using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTracker.Controllers.Base;
using StockTracker.Extensions;
using StockTracker.Models;
using StockTracker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StockTracker.Controllers
{
    public class R
    {
        [Required]
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [Required]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }

    public class WatchlistsController : EntityController<UserWatchlist>
    {
        public WatchlistsController(DbContext context) : base(context)
        {
        }

        [HttpPost]
        [Route("add-watchlist-symbol")]
        public async Task<IActionResult> AddToWatchlist([FromBody] R req)
        {
            try
            {
                var symbols = dbContext.ResolveDbSet<StockSymbol>() as DbSet<StockSymbol>;
                var matchingSymbol = symbols.Where(s => s.Symbol == req.Symbol).FirstOrDefault();
                var newWatchlist = new UserWatchlist { SymbolId = matchingSymbol.Id, UserId = Convert.ToInt32(req.UserId) };
                return await AddEntity(newWatchlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}
