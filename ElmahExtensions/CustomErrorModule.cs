using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using Elmah;
using ElmahExtensions.Configuration;
using ElmahExtensions.Utils;

namespace ElmahExtensions
{
    public class CustomErrorModule : HttpModuleBase, IExceptionFiltering
    {
        public event ExceptionFilterEventHandler Filtering;
        public event ErrorLoggedEventHandler Logged;


        /// <summary>
        /// Initializes the module and prepares it to handle requests.
        /// </summary>
        protected override void OnInit(HttpApplication application)
        {
            application.AssertNotNull("application");
            application.Error += new EventHandler(OnError);
            ErrorSignal.Get(application).Raised += new ErrorSignalEventHandler(OnErrorSignaled);
        }

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnError(object sender, EventArgs args)
        {
            var application = (HttpApplication) sender;
            if (application == null)
            {
                Trace.Write("Sender was not an HttpApplication.");
                return;
            }
            LogException(application.Server.GetLastError(), application.Context);
        }

        protected virtual void OnErrorSignaled(object sender, ErrorSignalEventArgs args)
        {
            LogException(args.Exception, args.Context);
        }

        delegate void ErrorHandlerDelegate(Error error);

        protected virtual void LogException(Exception e, HttpContext context)
        {
            e.AssertNotNull("e");
            // Fire an event to check if listeners want to filter out
            // logging of the uncaught exception.
            var args = new ExceptionFilterEventArgs(e, context);
            OnFiltering(args);

            if (args.Dismissed)
                return;

            //Run the module
            ErrorLogEntry entry = null;
            try
            {
                var error = new Error(e, context);
                new ErrorHandlerDelegate(err => new CustomErrorHandler() {Configuration = SettingsManager.Config}.HandleError(err))(error);
            }
            catch (Exception localException)
            {
                //
                // IMPORTANT! We swallow any exception raised during the 
                // logging and send them out to the trace . The idea 
                // here is that logging of exceptions by itself should not 
                // be  critical to the overall operation of the application.
                // The bad thing is that we catch ANY kind of exception, 
                // even system ones and potentially let them slip by.
                Trace.WriteLine(localException);
            }

            if (entry != null)
                OnLogged(new ErrorLoggedEventArgs(entry));
        }

        protected virtual void OnLogged(ErrorLoggedEventArgs args)
        {
            if (Logged != null)
                Logged(this, args);
        }

        protected virtual void OnFiltering(ExceptionFilterEventArgs args)
        {
            ExceptionFilterEventHandler handler = Filtering;

            if (Filtering != null)
                Filtering(this, args);
        }

        /// <summary>
        /// Determines whether the module will be registered for discovery
        /// in partial trust environments or not.
        /// </summary>
        protected override bool SupportDiscoverability
        {
            get { return true; }
        }
    }
}