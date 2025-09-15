using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoLimitNameInPlanException(string name)
        : HttpException(500, "No limit name {0} in plan.", name)
    {
    }
}
