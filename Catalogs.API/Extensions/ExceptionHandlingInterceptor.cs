using Catalogs.Domain.Entities.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Catalogs.Domain.Entities.Constants.StatusCodes;

namespace Catalogs.API.Extensions
{
    public class ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger) : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await base.UnaryServerHandler(request, context, continuation);
            }
            catch (NotFoundException e)
            {
                var responseVm = new GrpcExceptionResponse(GrpcStatusCodes.NotFound, e.Message);

                return MapResponse<TRequest, TResponse>(responseVm);
            }
            catch (Exception e)
            {
                logger.LogError("Error {error} occured with message {message}", e, e.Message);

                var responseVm = new GrpcExceptionResponse(GrpcStatusCodes.InternalError, "Server error");

                return MapResponse<TRequest, TResponse>(responseVm);
            }
        }

        private static TResponse MapResponse<TRequest, TResponse>(GrpcExceptionResponse responseViewModel)
        {
            var concreteResponse = Activator.CreateInstance<TResponse>();

            concreteResponse?.GetType().GetProperty(nameof(responseViewModel.StatusCode))?.SetValue(concreteResponse, responseViewModel.StatusCode);

            concreteResponse?.GetType().GetProperty(nameof(responseViewModel.Message))?.SetValue(concreteResponse, responseViewModel.Message);

            return concreteResponse;
        }
    }
}