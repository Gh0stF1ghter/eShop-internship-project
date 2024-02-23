using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class CreateItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateItemComand, ItemDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemDto> Handle(CreateItemComand command, CancellationToken token)
        {
            if (command.ItemDTO is null)
            {
                throw new BadRequestException(ErrorMessages.ItemIsNull);
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
            var itemTypeExists = await _unitOfWork.ItemType.Exists(t => t.Id.Equals(command.TypeId), token);

            if (!itemTypeExists)
            {
                throw new NotFoundException(ErrorMessages.TypeNotFound);
            }

            var brandExists = await _unitOfWork.Brand.Exists(b => b.Id.Equals(command.ItemDTO.BrandId), token);

            if (!brandExists)
            {
                throw new NotFoundException(ErrorMessages.TypeNotFound);
            }

            var vendorExists = await _unitOfWork.Vendor.Exists(b => b.Id.Equals(command.ItemDTO.VendorId), token);

            if (!vendorExists)
            {
                throw new BadRequestException(ErrorMessages.VendorNotFound);
            }
        }
    }
}
