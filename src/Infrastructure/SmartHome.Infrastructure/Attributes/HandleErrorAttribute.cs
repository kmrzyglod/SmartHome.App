using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SmartHome.Infrastructure.Attributes
{
#pragma warning disable CS0618
    public class HandleErrorAttribute : FunctionExceptionFilterAttribute
    {
        private const string FunctionInstanceId = "FunctionInstanceId";
        private const string FunctionName = "FunctionName";

        public override Task OnExceptionAsync(FunctionExceptionContext exceptionContext,
            CancellationToken cancellationToken)
        {
            var state = new Dictionary<string, object>
            {
                {FunctionInstanceId, exceptionContext.FunctionInstanceId},
                {FunctionName, exceptionContext.FunctionName}
            };

            exceptionContext.Logger.Log(LogLevel.Error, 0, state, exceptionContext.Exception, Formatter);
            return Task.CompletedTask;
        }

        internal static string Formatter<T>(T state, Exception ex)
        {
            string errorMessage = ex != null ? ex.ToString() : string.Empty;
            var stateDictionary = state as Dictionary<string, object> ?? new Dictionary<string, object>();
            stateDictionary.TryGetValue(FunctionInstanceId, out var functionInstanceId);
            stateDictionary.TryGetValue(FunctionName, out var functionName);

            return
                $"ErrorHandler called (FunctionName: {functionName} : FunctionInstanceId: {functionInstanceId} | Exception: {errorMessage}";
        }
    }
}