using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class MaxEnabledBusinessesLimitReachedException()
        : HttpException(400, "The maximum limit of enabled businesses has been reached.")
    {
    }
}
