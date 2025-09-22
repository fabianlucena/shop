using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class MaxEnabledItemsLimitReachedException()
        : HttpException(400, "The maximum limit of enabled items has been reached.")
    {
    }
}
