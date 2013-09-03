#region

using System;
using System.IO;
using System.Reflection;
using ElmahExtensions.ErrorActions;

#endregion

namespace ElmahExtensions.Utils
{
    public class CommonCode
    {
        public static string GetStringFromEmbeddedResource(string resourceName)
        {
            return GetStringFromEmbeddedResource(resourceName, typeof (ErrorAction).Assembly);
        }

        public static string GetStringFromEmbeddedResource(string resourceName, Assembly assembly)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                stream.AssertNotNull(string.Format("The resource path [{0}] returned nothing", resourceName));
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }

    public static class Extensions
    {
        public static void AssertNotNull(this object o, string error = "A not null assertion failed")
        {
            if (o == null)
                throw new ArgumentNullException(error);
        }

        public static void AssertNotNullOrEmpty(this string o, string error = "A not null assertion failed")
        {
            if (string.IsNullOrEmpty(o))
                throw new ArgumentNullException(error);
        }
    }
}