using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoStoreException()
        : HttpException(400, "No store provided.")
    {
    }
}
