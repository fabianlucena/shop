using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class ImageIsTooLargeException()
        : HttpException(400, "The image is too large.")
    {
    }
}
