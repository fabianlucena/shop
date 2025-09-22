using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class TotalCommercesImagesLimitReachedException()
        : HttpException(400, "You can't create more commerce images because you've reached the limit.")
    {
    }
}
