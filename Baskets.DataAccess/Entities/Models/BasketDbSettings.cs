using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.DataAccess.Entities.Models
{
    public class BasketDatabase
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string BasketsCollectionName { get; set; } = null!;
        public string ItemsCollectionName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
    }
}
