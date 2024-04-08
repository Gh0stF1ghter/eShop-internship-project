using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.BrandCommands.CreateBrand
{
    public sealed class CreateBrandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateBrandCommand, BrandDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BrandDto> Handle(CreateBrandCommand command, CancellationToken token)
        {
            var isBrandExists = await _unitOfWork.Brand.IsExistAsync(b => b.Name.Equals(command.BrandDto.Name), token);

            if (isBrandExists)
            {
                throw new BadRequestException(BrandMessages.BrandExists);
            }

            var brand = _mapper.Map<Brand>(command.BrandDto);

            _unitOfWork.Brand.Add(brand);

            await _unitOfWork.SaveChangesAsync(token);

            var brandToReturn = _mapper.Map<BrandDto>(brand);

            return brandToReturn;
        }
    }
}
