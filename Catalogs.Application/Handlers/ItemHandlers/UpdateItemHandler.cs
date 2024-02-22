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

        private async Task FindReferences(UpdateItemComand command, CancellationToken token)
        {
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(command.TypeId, command.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);
            var brand = await _unitOfWork.Brand.GetBrandByIdAsync(command.Item.BrandId, command.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.BrandNotFound);
            var vendor = await _unitOfWork.Vendor.GetVendorByIdAsync(command.Item.VendorId, command.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.VendorNotFound);
        }

    }
}
