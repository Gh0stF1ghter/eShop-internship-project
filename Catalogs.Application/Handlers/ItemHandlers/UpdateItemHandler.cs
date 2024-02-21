using AutoMapper;
using Catalogs.Application.Commands.ItemCommands;
using Catalogs.Domain.Entities.Constants;
using Catalogs.Domain.Entities.Exceptions;
using Catalogs.Domain.Interfaces;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class UpdateItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateItemCommand request, CancellationToken token)
        {
            var itemToUpdate = await _unitOfWork.Item.GetItemByIdAsync(request.Id, request.TrackChanges)
                ?? throw new BadRequestException(ErrorMessages.ItemNotFound);

            _mapper.Map(request.Item, itemToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
