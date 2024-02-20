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
    public sealed class CreateItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateItemCommand, ItemDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemDTO> Handle(CreateItemCommand command, CancellationToken token)
        {
            if (command.ItemDTO is null)
            {
                throw new BadRequestException(ErrorMessages.ItemIsNull);
            }

            var newItem = _mapper.Map<Item>(command.ItemDTO);

            await _unitOfWork.Items.AddItemAsync(command.BrandId, command.TypeId, command.VendorId, newItem);
            _unitOfWork.SaveChanges();

            var itemToReturn = _mapper.Map<ItemDTO>(newItem);

            return itemToReturn;
        }
    }
}
