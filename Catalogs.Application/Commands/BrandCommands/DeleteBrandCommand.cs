﻿using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public record DeleteBrandComand(int Id, bool TrackChanges) : IRequest;
}
