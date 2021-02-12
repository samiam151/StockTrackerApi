using StockTracker.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StockTracker.Models
{
    public class User : Entity
    {
        public User()
        {
            Watchlist = new HashSet<UserWatchlist>();
        }

        [Required]
        public string email { get; set; }

        [Required]
        [JsonIgnore]
        public string password { get; set; }

        public virtual ICollection<UserWatchlist> Watchlist { get; set; }
    }
}
