using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class MaxEnabledItemsImagesAggregatedSizeLimitReachedException()
        : HttpException(400, "The maximum limit of enabled item images size has been reached.")
    {
    }
}
