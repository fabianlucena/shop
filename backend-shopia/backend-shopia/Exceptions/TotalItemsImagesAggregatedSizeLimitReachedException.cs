using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class TotalItemsImagesAggregatedSizeLimitReachedException()
        : HttpException(400, "You can't create more items images because you've reached the size limit.")
    {
    }
}
