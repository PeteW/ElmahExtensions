using System;
using System.Diagnostics;
using System.Linq;
using Elmah;
using ElmahExtensions.Configuration;

namespace ElmahExtensions
{
    public class CustomErrorHandler
    {
        public CustomErrorHandlerConfiguration Configuration { get; set; }

        public void HandleError(Error error)
        {
            foreach (var errorHandlerSection in Configuration.ErrorHandlers)
            {
                if (errorHandlerSection.ErrorConditions.All(x => x.IsTrue(error)))
                {
                    if (Configuration.LoggingLevel == LoggingLevel.Debug)
                        Trace.WriteLine(string.Format("Debug: Running section [{0}]", errorHandlerSection.Name));
                    foreach (var errorAction in errorHandlerSection.ErrorActions)
                    {
                        try
                        {
                            if (Configuration.LoggingLevel == LoggingLevel.Debug)
                                Trace.WriteLine(string.Format("Debug: Running section [{0}] action [{1}]", errorHandlerSection.Name, errorAction.Name));
                            errorAction.Run(error);
                        }
                        catch (Exception exp)
                        {
                            var message = string.Format("Error occurred when executing section [{0}] action [{2}]: {1}", errorHandlerSection.Name, exp, errorAction.Name);
                            Trace.WriteLine(message);
                            throw new CustomErrorHandlerException(message);
                        }
                    }
                }
            }
        }
    }
}