using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs
{
    public record CreateItemDTO(string Name, int Stock, decimal Price, string ImageUrl);
}
