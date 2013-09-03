using System;
using System.Diagnostics;
using System.Xml.Serialization;
using Elmah;

namespace ElmahExtensions.ErrorActions
{
    [Serializable]
    public class WriteToEventLogErrorAction : ErrorAction
    {
        /*
          public enum EventLogEntryType
          {
            Error = 1,
            Warning = 2,
            Information = 4,
            SuccessAudit = 8,
            FailureAudit = 16,
          }
         */

        [XmlAttribute]
        public string EventLogEntryType { get; set; }

        [XmlAttribute]
        public string SourceName { get; set; }

        [XmlAttribute]
        public string StringFormat { get; set; }

        public override void Run(Error error)
        {
            var errorMessage = FormatString(StringFormat, error);
            Trace.WriteLine("Formatted error: "+errorMessage);
            var eventLogEntryType = (EventLogEntryType) Enum.Parse(typeof (EventLogEntryType), EventLogEntryType);
            string source = SourceName;
            if (EventLog.SourceExists(source) == false)
                EventLog.CreateEventSource(source, "Application");
            EventLog.WriteEntry(source, errorMessage, eventLogEntryType);

        }
    }
}