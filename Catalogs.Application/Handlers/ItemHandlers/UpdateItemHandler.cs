using Catalogs.Application.Commands.ItemCommands;
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

            var itemToUpdate = await _unitOfWork.Item.GetItemOfTypeByIdAsync(comand.TypeId, comand.Id, comand.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.ItemNotFound);

            _mapper.Map(comand.Item, itemToUpdate);
            await _unitOfWork.SaveChangesAsync(token);
        }

        private async Task FindReferences(UpdateItemComand comand, CancellationToken token)
        {
            var itemTypeExists = await _unitOfWork.ItemType.Exists(t => t.Id.Equals(comand.TypeId), token);

            if (!itemTypeExists)
            {
                throw new NotFoundException(ErrorMessages.TypeNotFound);
            }

            var brandExists = await _unitOfWork.Brand.Exists(b => b.Id.Equals(comand.Item.BrandId), token);

            if (!brandExists)
            {
                throw new NotFoundException(ErrorMessages.TypeNotFound);
            }

            var vendorExists = await _unitOfWork.Vendor.Exists(b => b.Id.Equals(comand.Item.VendorId), token);

            if (!vendorExists)
            {
                throw new BadRequestException(ErrorMessages.VendorNotFound);
            }
        }
    }
}
