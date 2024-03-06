using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.Queries.BrandQueries;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Catalogs.Application.Handlers.BrandHandlers
{
    public sealed class GetBrandsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetBrandsHandler> logger) : IRequestHandler<GetBrandsQuery, IEnumerable<BrandDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<BrandDto>> Handle(GetBrandsQuery query, CancellationToken token)
        {
            logger.LogInformation("TEST MESSAGE FIOR JIBANA");
            var brand = await _unitOfWork.Brand.GetAllBrandsAsync(query.TrackChanges, token);

            var brandDtos = _mapper.Map<IEnumerable<BrandDto>>(brand);

            return brandDtos;
        }
    }
}