using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class TotalImagesPerItemLimitReachedException()
        : HttpException(400, "You can't add more images to the item because you've reached the limit.")
    {
    }
}
