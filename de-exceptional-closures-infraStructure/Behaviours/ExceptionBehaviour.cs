using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace de_exceptional_closures_infraStructure.Behaviours
{
    public class ExceptionBehaviour
    {
        public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        {
            private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

            private readonly NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
            {
                _logger = logger;
            }

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                try
                {
                    var response = await next();
                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception at {typeof(TResponse).Name} of {typeof(TRequest).Name} at {DateTime.UtcNow:yyyy-MM-dd hh:mm:ss.fff}");

                    logger.Error(ex, $"Logged by Exception Behaviour - Exception at {typeof(TResponse).Name} of {typeof(TRequest).Name} at {DateTime.UtcNow:yyyy-MM-dd hh:mm:ss.fff}");

                    throw;
                }
            }
        }
    }
}