﻿using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record CreateItemTypeCommand(ItemTypeManipulateDto ItemTypeDto) : IRequest<ItemTypeDto>;
}