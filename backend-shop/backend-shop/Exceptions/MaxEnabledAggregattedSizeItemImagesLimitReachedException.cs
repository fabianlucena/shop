using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class MaxEnabledAggregattedSizeItemImagesLimitReachedException()
        : HttpException(400, "The maximum limit of enabled items images has been reached.")
    {
    }
}
