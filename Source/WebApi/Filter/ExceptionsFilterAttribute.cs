using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Filter
{
    public class ExceptionsFilterAttribute : ExceptionFilterAttribute, IFilterMetadata
    {
        private readonly ILogger _logger;

        public ExceptionsFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
        }
    }
}
