using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Entities.DataTransferObjects
{
    public record ItemDTO(Guid Id, string Name, int Stock, decimal Price, string? ImageUrl);
}
