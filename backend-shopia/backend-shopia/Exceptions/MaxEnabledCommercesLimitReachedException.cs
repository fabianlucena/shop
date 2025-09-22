using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class MaxEnabledCommercesLimitReachedException()
        : HttpException(400, "The maximum limit of enabled commerces has been reached.")
    {
    }
}
