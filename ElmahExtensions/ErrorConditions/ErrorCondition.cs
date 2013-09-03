using System;
using Elmah;

namespace ElmahExtensions.ErrorConditions
{
    public class ErrorCondition
    {
        public virtual bool IsTrue(Error error){throw new NotImplementedException();}
    }
}