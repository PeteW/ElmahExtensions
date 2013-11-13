using System;
using Elmah;

namespace ElmahExtensions.ErrorConditions
{
    /// <summary>
    /// Always returns true
    /// </summary>
    [Serializable]
    public class CatchAllErrorCondition : ErrorCondition
    {
        public override bool IsTrue(Error error)
        {
            return true;
        }
    }
}