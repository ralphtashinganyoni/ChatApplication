using Microsoft.Extensions.Logging;
using MediatR;

namespace ChatApplication.Server.MediatR
{
    public class ErrorHandlerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public ErrorHandlerPipelineBehavior(ILogger<TRequest> logger)
        {
            this._logger = logger;
        }

        public async Task<TResponse> Handle(TRequest req, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception e)
            {
                var errMsg = e.GetBaseException().Message;
                var errorLog = new LoggingMeta<TRequest>(req, 0)
                {
                    MessageSucceeded = false,
                    MessageFailures = errMsg
                };
                _logger.LogError("{@error}", errorLog);
                throw new UnhandledEventingPipelineException(e, Guid.NewGuid().ToString());
            }
        }
    }

    public class UnhandledEventingPipelineException : Exception
    {
        public UnhandledEventingPipelineException(Exception ex, string correlationId)
            : base($"An unhandled error occurred. Please check the logs for error ID: {correlationId}", ex)
        {
            CorrelationId = correlationId;
        }

        public string CorrelationId { get; set; }
    }
}
