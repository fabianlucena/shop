using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class MaxEnabledStoresLimitReachedException()
        : HttpException(400, "The maximum limit of enabled stores has been reached.")
    {
    }
}
