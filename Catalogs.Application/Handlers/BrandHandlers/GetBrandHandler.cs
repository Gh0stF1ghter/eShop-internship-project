using Catalogs.Application.Queries.ItemQueries;
using MediatR;

namespace Catalogs.Application.Handlers.BrandHandlers
{
    public sealed class GetBrandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBrandQuery, BrandDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BrandDto> Handle(GetBrandQuery query, CancellationToken token)
        {
            var brand = await _unitOfWork.Brand.GetBrandByIdAsync(query.Id, query.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.BrandNotFound + query.Id);

            var brandDto = _mapper.Map<BrandDto>(brand);

            return brandDto;
        }
    }
}
