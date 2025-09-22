using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class MaxEnabledItemsImagesLimitReachedException()
        : HttpException(400, "The maximum limit of enabled item images has been reached.")
    {
    }
}
