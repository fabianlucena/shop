using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class TotalCommercesImagesAggregatedSizeLimitReachedException()
        : HttpException(400, "You can't create more commerce images because you've reached the size limit.")
    {
    }
}
