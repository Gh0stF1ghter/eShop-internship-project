using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.BrandQueries.GetBrands
{
    public sealed class GetBrandsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBrandsQuery, IEnumerable<BrandDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<BrandDto>> Handle(GetBrandsQuery query, CancellationToken token)
        {
            var brand = await _unitOfWork.Brand.GetAllBrandsAsync(query.TrackChanges, token);

            var brandDtos = _mapper.Map<IEnumerable<BrandDto>>(brand);

            return brandDtos;
        }
    }
}