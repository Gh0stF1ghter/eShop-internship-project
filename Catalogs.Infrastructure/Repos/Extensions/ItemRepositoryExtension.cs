using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Infrastructure.Repos.Extensions
{
    public static class ItemRepositoryExtension
    {
        public static IQueryable<Item> FilterItems(this IQueryable<Item> items, ItemParameters itemParameters) =>
            items.Where(i => i.Price >= itemParameters.MinPrice
                && i.Price <= itemParameters.MaxPrice
                && i.Stock >= itemParameters.Stock);

        public static IQueryable<Item> Search(this IQueryable<Item> items, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return items;
            }

            string normalizedSearchTerm = searchTerm.Trim().ToLower();

            return items.Where(i => i.Name.ToLower().Contains(normalizedSearchTerm));
        }
    }
}
