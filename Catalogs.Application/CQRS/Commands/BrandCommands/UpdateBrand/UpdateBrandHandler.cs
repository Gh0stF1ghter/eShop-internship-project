using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.BrandCommands.UpdateBrand
{
    public sealed class UpdateBrandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateBrandCommand request, CancellationToken token)
        {
            var brandToUpdate = await _unitOfWork.Brand.GetBrandToUpdateAsync(request.Id, token);

            if (brandToUpdate == null)
            {
                throw new NotFoundException(BrandMessages.BrandNotFound);
            }

            _mapper.Map(request.Brand, brandToUpdate);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}