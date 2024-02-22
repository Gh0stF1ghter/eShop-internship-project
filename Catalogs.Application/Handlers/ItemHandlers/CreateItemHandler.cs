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
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(command.TypeId, command.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);
            var brand = await _unitOfWork.Brand.GetBrandByIdAsync(command.ItemDTO.BrandId, command.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.BrandNotFound);
            var vendor = await _unitOfWork.Vendor.GetVendorByIdAsync(command.ItemDTO.VendorId, command.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.VendorNotFound);
        }
    }
}
