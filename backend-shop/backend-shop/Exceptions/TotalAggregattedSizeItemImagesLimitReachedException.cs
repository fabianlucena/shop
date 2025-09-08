using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class TotalAggregattedSizeItemImagesLimitReachedException()
        : HttpException(400, "You can't create more items images because you've reached the size limit.")
    {
    }
}
