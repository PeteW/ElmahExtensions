using System;
using System.Xml.Serialization;
using Elmah;
using ElmahExtensions.Utils;

namespace ElmahExtensions.ErrorConditions
{
    [Serializable]
    public class ExceptionTypeErrorCondition:ErrorCondition
    {
        [XmlAttribute]
        public string TypeName { get; set; }

        public override bool IsTrue(Error error)
        {
            return error.Type == TypeName;
        }
    }
}