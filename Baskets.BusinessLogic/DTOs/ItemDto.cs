using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.DTOs
{
    public record ItemDto(string Id, string Name, double Price, string ImageUrl);
}
