using Catalogs.Application.Comands.ItemCommands;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemCommands.UpdateItem
{
    public sealed class UpdateItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateItemCommand comand, CancellationToken token)
        {
            await FindReferences(comand, token);

            var itemToUpdate = await _unitOfWork.Item.GetItemOfTypeByIdAsync(comand.TypeId, comand.Id, comand.TrackChanges, token);

            if (itemToUpdate == null)
            {
                throw new BadRequestException(ItemMessages.ItemNotFound);
            }

            _mapper.Map(comand.Item, itemToUpdate);
            await _unitOfWork.SaveChangesAsync(token);
        }

        private async Task FindReferences(UpdateItemCommand comand, CancellationToken token)
        {
            var itemTypeExists = await _unitOfWork.ItemType.IsExistAsync(t => t.Id.Equals(comand.TypeId), token);

            if (!itemTypeExists)
            {
                throw new NotFoundException(ItemTypeMessages.ItemTypeNotFound);
            }

            var brandExists = await _unitOfWork.Brand.IsExistAsync(b => b.Id.Equals(comand.Item.BrandId), token);

            if (!brandExists)
            {
                throw new NotFoundException(BrandMessages.BrandNotFound);
            }

            var vendorExists = await _unitOfWork.Vendor.IsExistAsync(b => b.Id.Equals(comand.Item.VendorId), token);

            if (!vendorExists)
            {
                throw new NotFoundException(VendorMessages.VendorNotFound);
            }
        }
    }
}
