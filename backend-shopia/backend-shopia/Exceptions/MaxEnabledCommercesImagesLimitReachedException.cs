using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class MaxEnabledCommercesImagesLimitReachedException()
        : HttpException(400, "The maximum limit of enabled commerce images has been reached.")
    {
    }
}
