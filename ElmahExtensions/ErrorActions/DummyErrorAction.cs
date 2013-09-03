using System;
using Elmah;

namespace ElmahExtensions.ErrorActions
{
    [Serializable]
    public class DummyErrorAction:ErrorAction
    {
        public override void Run(Error error)
        {
            Console.WriteLine("Derp");
        }
    }
}