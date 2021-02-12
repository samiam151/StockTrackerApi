using StockTracker.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracker.Models
{
    public class UserWatchlist : Entity
    {
        public User User { get; set; }
        public int? UserId { get; set; }
        public StockSymbol Symbol { get; set; }
        public int? SymbolId { get; set; }
    }
}
