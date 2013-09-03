using System;
using Elmah;

namespace ElmahExtensions.ErrorConditions
{
    [Serializable]
    public class DummyErrorCondition : ErrorCondition
    {
        public override bool IsTrue(Error error)
        {
            return true;
        }
    }
}