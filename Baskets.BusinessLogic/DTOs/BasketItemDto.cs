﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.DTOs
{
    public record BasketItemDto(string Id, int Quantity, double sumPrice, ItemDto Item);
}
