using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class TotalImagesPerCommerceLimitReachedException()
        : HttpException(400, "You can't add more images to the commerce because you've reached the limit.")
    {
    }
}
