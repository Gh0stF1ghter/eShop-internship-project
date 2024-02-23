﻿using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record UpdateItemComand(int TypeId, int Id, ItemManipulateDto Item, bool TrackChanges) : IRequest;
}