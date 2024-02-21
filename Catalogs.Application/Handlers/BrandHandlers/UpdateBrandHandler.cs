using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.BrandHandlers
{
    public sealed class UpdateBrandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateBrandCommand request, CancellationToken token)
        {
            var brandToUpdate = await _unitOfWork.Brand.GetBrandByIdAsync(request.Id, request.TrackChanges)
                ?? throw new BadRequestException(ErrorMessages.BrandNotFound);

            _mapper.Map(request.Brand, brandToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
