using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, $"Ocorreu uma exeção não tratada: Status Code {context.ModelState}");

            context.Result = new ObjectResult($"Ocorreu um problema ao tratar a sua solicitação: Status Code  {context.ModelState}")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
