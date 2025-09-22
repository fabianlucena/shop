using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class MaxEnabledStoresLimitReachedException()
        : HttpException(400, "The maximum limit of enabled stores has been reached.")
    {
    }
}
