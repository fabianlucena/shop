using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class StoreDoesNotExistException()
        : HttpException(400, "Store does not exist.")
    {
    }
}
