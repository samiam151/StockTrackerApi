using StockTracker.Controllers.StockTracker.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracker.Models
{
    public class UserDTO : LoginResult
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<UserWatchlist> Watchlist { get; set; }

    }
}
