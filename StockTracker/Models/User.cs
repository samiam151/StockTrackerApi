using StockTracker.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracker.Models
{
    public class User : Entity
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
