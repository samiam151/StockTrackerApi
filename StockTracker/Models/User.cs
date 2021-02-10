using StockTracker.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracker.Models
{
    public class User : Entity
    {
        public User()
        {
            Watchlist = new HashSet<StockSymbol>();
        }

        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }

        public virtual ICollection<StockSymbol> Watchlist { get; set; }
    }
}
