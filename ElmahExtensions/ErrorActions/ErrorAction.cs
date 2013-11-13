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

        /// <summary>
        /// Pass in a string emplate and an error and the output will be an attempt to fill in the template with properties from the error
        /// </summary>
        /// <param name="input"></param>
        /// <param name="error"></param>
        /// <returns></returns>
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