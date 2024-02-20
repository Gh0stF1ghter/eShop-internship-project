using AutoMapper;
using Catalogs.Application.Commands.ItemCommands;
using Catalogs.Domain.Entities.Constants;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.Exceptions;
using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class CreateItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateItemCommand, ItemDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemDto> Handle(CreateItemCommand command, CancellationToken token)
        {
            if (command.ItemDTO is null)
            {
                throw new BadRequestException(ErrorMessages.ItemIsNull);
            }

            var newItem = _mapper.Map<Item>(command.ItemDTO);

            _unitOfWork.Item.AddItem(command.BrandId, command.TypeId, command.VendorId, newItem);
            await _unitOfWork.SaveChangesAsync();

            var itemToReturn = _mapper.Map<ItemDto>(newItem);

            return itemToReturn;
        }
    }
}
