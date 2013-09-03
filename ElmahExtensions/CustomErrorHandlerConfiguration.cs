using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ElmahExtensions.Configuration;
using ElmahExtensions.ErrorActions;
using ElmahExtensions.ErrorConditions;
using ElmahExtensions.Utils;

namespace ElmahExtensions
{
    [Serializable]
    public class CustomErrorHandlerConfiguration
    {
        [XmlAttribute]
        public string ErrorHandlerLoggingLevel { get; set; }

        public List<ErrorHandlerSection> ErrorHandlers { get; set; }

        private static XmlSerializer _serializer = null;

        [XmlIgnore]
        private static XmlSerializer Serializer
        {
            get
            {
                if (_serializer == null)
                    _serializer = new XmlSerializer(typeof (CustomErrorHandlerConfiguration));
                return _serializer;
            }
        }

        public static CustomErrorHandlerConfiguration ReadFromString(string input)
        {
            CustomErrorHandlerConfiguration result = null;
            using (TextReader reader = new StringReader(input))
            {
                result = (CustomErrorHandlerConfiguration) Serializer.Deserialize(reader);
            }
            return result;
        }

        [XmlIgnore]
        public LoggingLevel LoggingLevel
        {
            get
            {
                try
                {
                    return (LoggingLevel) Enum.Parse(typeof (LoggingLevel), ErrorHandlerLoggingLevel);
                }
                catch
                {
                    return LoggingLevel.Error;
                }
            }
        }
    }

    [Serializable]
    public class ErrorHandlerSection
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlArrayItem("DummyErrorCondition", typeof (DummyErrorCondition))]
        [XmlArrayItem("ElmahErrorMessageSubstringCondition", typeof (ElmahErrorMessageSubstringCondition))]
        [XmlArrayItem("CatchAllErrorCondition", typeof (CatchAllErrorCondition))]
        [XmlArrayItem("ExceptionTypeErrorCondition", typeof (ExceptionTypeErrorCondition))]
        [XmlArray("ErrorConditions")]
        public List<ErrorCondition> ErrorConditions { get; set; }

        [XmlArrayItem("DummyErrorAction", typeof (DummyErrorAction))]
        [XmlArrayItem("WriteToEventLogErrorAction", typeof (WriteToEventLogErrorAction))]
        [XmlArrayItem("SendEmailErrorAction", typeof (SendEmailErrorAction))]
        [XmlArray("ErrorActions")]
        public List<ErrorAction> ErrorActions { get; set; }
    }
}