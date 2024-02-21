using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class UpdateItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateItemCommand comand, CancellationToken token)
        {
            await FindReferences(comand, token);

            var itemToUpdate = await _unitOfWork.Item.GetItemOfTypeByIdAsync(comand.TypeId, comand.Id, comand.TrackChanges)
                ?? throw new BadRequestException(ErrorMessages.ItemNotFound);

            _mapper.Map(comand.Item, itemToUpdate);
            await _unitOfWork.SaveChangesAsync(token);
        }

        private async Task FindReferences(UpdateItemCommand command, CancellationToken token)
        {
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(command.TypeId, command.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);
            var brand = await _unitOfWork.Brand.GetBrandByIdAsync(command.Item.BrandId, command.TrackChanges)
                ?? throw new BadRequestException(ErrorMessages.BrandNotFound);
            var vendor = await _unitOfWork.Vendor.GetVendorByIdAsync(command.Item.VendorId, command.TrackChanges)
                ?? throw new BadRequestException(ErrorMessages.VendorNotFound);
        }

    }
}
