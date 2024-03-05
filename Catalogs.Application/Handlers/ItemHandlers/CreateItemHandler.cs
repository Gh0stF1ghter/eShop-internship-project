﻿using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.Comands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class CreateItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateItemComand, ItemDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemDto> Handle(CreateItemComand command, CancellationToken token)
        {
            var itemExists = await _unitOfWork.Item.IsExistAsync(i => i.Name.Equals(command.ItemDTO.Name, StringComparison.OrdinalIgnoreCase) && i.VendorId.Equals(command.ItemDTO.VendorId), token);

            if (itemExists)
            {
                throw new BadRequestException(ItemMessages.ItemExists);
            }

            await FindReferences(command, token);

            var newItem = _mapper.Map<Item>(command.ItemDTO);

            _unitOfWork.Item.AddItem(command.TypeId, newItem);

            await _unitOfWork.SaveChangesAsync(token);

            var itemToReturn = _mapper.Map<ItemDto>(newItem);

            return itemToReturn;
        }

        private async Task FindReferences(CreateItemComand command, CancellationToken token)
        {
            var itemTypeExists = await _unitOfWork.ItemType.IsExistAsync(t => t.Id.Equals(command.TypeId), token);

            if (!itemTypeExists)
            {
                throw new NotFoundException(ItemTypeMessages.TypeNotFound);
            }

            var brandExists = await _unitOfWork.Brand.IsExistAsync(b => b.Id.Equals(command.ItemDTO.BrandId), token);

            if (!brandExists)
            {
                throw new NotFoundException(BrandMessages.BrandNotFound);
            }

            var vendorExists = await _unitOfWork.Vendor.IsExistAsync(b => b.Id.Equals(command.ItemDTO.VendorId), token);

            if (!vendorExists)
            {
                throw new BadRequestException(VendorMessages.VendorNotFound);
            }
        }
    }
}