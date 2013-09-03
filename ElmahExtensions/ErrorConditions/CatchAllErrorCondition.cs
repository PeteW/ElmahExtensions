using Elmah;

namespace ElmahExtensions.ErrorConditions
{
    public class CatchAllErrorCondition:ErrorCondition
    {
        public override bool IsTrue(Error error)
        {
            return true;
        }
    }
}