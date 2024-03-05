using Catalogs.Application.Comands.BrandCommands;
using MediatR;

namespace Catalogs.Application.Handlers.BrandHandlers
{
    public sealed class UpdateBrandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateBrandComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateBrandComand request, CancellationToken token)
        {
            var brandToUpdate = await _unitOfWork.Brand.GetBrandByIdAsync(request.Id, request.TrackChanges, token);

            if (brandToUpdate == null)
            {
                throw new BadRequestException(BrandMessages.BrandNotFound);
            }

            _mapper.Map(request.Brand, brandToUpdate);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
