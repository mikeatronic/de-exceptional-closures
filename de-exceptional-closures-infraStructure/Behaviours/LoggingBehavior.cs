﻿using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace de_exceptional_closures_infraStructure.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        private readonly NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name} at {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff")}");

            var response = await next();

            _logger.LogInformation($"Handled {typeof(TResponse).Name} of {typeof(TRequest).Name} at {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff")}");

            logger.Info($"Logged by LoggingBehaviour - Handled {typeof(TResponse).Name} of {typeof(TRequest).Name} at {DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff")}");

            return response;
        }
    }
}