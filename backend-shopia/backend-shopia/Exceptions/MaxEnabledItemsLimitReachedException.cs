using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class MaxEnabledItemsLimitReachedException()
        : HttpException(400, "The maximum limit of enabled items has been reached.")
    {
    }
}
