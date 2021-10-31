using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JimmyCms.Domain.Articles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JimmyCms.Domain.Pipelines
{
    public class ResponseBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
        where TResponse : BasicResponse, new()
    {
        private readonly ILogger<ResponseBehaviour<TRequest, TResponse>> _logger;

        public ResponseBehaviour(ILogger<ResponseBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (ValidationException e)
            {
                return new TResponse
                {
                    Success = false,
                    Value = e.Message,
                    ResponseCode = StatusCodes.Status403Forbidden
                };
            }
            catch (KeyNotFoundException e)
            {
                return new TResponse
                {
                    Success = false,
                    Value = e.Message,
                    ResponseCode = StatusCodes.Status404NotFound
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal exception");
                return new TResponse
                {
                    Success = false,
                    Value = "Internal server error",
                    ResponseCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}