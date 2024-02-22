using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.BrandHandlers
{
    public sealed class CreateBrandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateBrandComand, BrandDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BrandDto> Handle(CreateBrandComand command, CancellationToken token)
        {
            if (command.BrandDto is null)
            {
                throw new BadRequestException(ErrorMessages.BrandIsNull);
            }

            var brand = _mapper.Map<Brand>(command.BrandDto);

            _unitOfWork.Brand.Add(brand);
            await _unitOfWork.SaveChangesAsync(token);

            var brandToReturn = _mapper.Map<BrandDto>(brand);

            return brandToReturn;
        }
    }
}
