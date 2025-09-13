using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class MaxEnabledCommercesImagesAggregatedSizeLimitReachedException()
        : HttpException(400, "The maximum limit of enabled commerce images size has been reached.")
    {
    }
}
