using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class UpdateItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateItemTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateItemTypeCommand request, CancellationToken token)
        {
            var typeToUpdate = await _unitOfWork.ItemType.GetItemTypeByIdAsync(request.Id, request.TrackChanges)
                ?? throw new BadRequestException(ErrorMessages.TypeNotFound);

            _mapper.Map(request.Type, typeToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
