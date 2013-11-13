using System;
using System.Xml.Serialization;
using Elmah;

namespace ElmahExtensions.ErrorConditions
{
    /// <summary>
    /// returns true fo the error contains a specified substring (case insensitive) 
    /// </summary>
    [Serializable]
    public class ElmahErrorMessageSubstringCondition:ErrorCondition
    {
        [XmlAttribute]
        public string Substring { get; set; }

        public override bool IsTrue(Error error)
        {
            return error.Message.ToLower().Contains(Substring.ToLower());
        }
    }
}