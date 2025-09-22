using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class ImageIsTooLargeException()
        : HttpException(400, "The image is too large.")
    {
    }
}
