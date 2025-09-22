using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoStoreException()
        : HttpException(400, "No store provided.")
    {
    }
}
