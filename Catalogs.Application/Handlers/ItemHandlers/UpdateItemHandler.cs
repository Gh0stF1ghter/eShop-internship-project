using Catalogs.Application.Comands.ItemCommands;
using Catalogs.Domain.Entities.Models;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class UpdateItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateItemComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateItemComand comand, CancellationToken token)
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

        private async Task FindReferences(UpdateItemComand comand, CancellationToken token)
        {
            var itemTypeExists = await _unitOfWork.ItemType.IsExistAsync(t => t.Id.Equals(comand.TypeId), token);

            if (!itemTypeExists)
            {
                throw new NotFoundException(ItemTypeMessages.TypeNotFound);
            }

            var brandExists = await _unitOfWork.Brand.IsExistAsync(b => b.Id.Equals(comand.Item.BrandId), token);

            if (!brandExists)
            {
                throw new NotFoundException(BrandMessages.BrandNotFound);
            }

            var vendorExists = await _unitOfWork.Vendor.IsExistAsync(b => b.Id.Equals(comand.Item.VendorId), token);

            if (!vendorExists)
            {
                throw new BadRequestException(VendorMessages.VendorNotFound);
            }
        }
    }
}
