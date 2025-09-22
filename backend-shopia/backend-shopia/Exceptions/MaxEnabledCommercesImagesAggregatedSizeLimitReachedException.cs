using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class MaxEnabledCommercesImagesAggregatedSizeLimitReachedException()
        : HttpException(400, "The maximum limit of enabled commerce images size has been reached.")
    {
    }
}
