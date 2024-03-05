﻿using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Comands.ItemTypeCommands
{
    public sealed record CreateItemTypeCommand(ItemTypeManipulateDto ItemTypeDto) : IRequest<ItemTypeDto>;
}