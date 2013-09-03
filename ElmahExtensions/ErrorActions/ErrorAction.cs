using System;
using System.Reflection;
using System.Xml.Serialization;
using Elmah;

namespace ElmahExtensions.ErrorActions
{
    [Serializable]
    public class ErrorAction
    {
        [XmlAttribute]
        public string Name{ get; set; }

        public virtual void Run(Error error){}

        public string FormatString(string input, Error error)
        {
            foreach (PropertyInfo propertyInfo in typeof(Error).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Instance))
            {
                var value = propertyInfo.GetValue(error, null);
                input = input.Replace("${" + propertyInfo.Name + "}", (value ?? "").ToString());
            }
            return input;
        }
    }
}