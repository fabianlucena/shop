using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class StoreDoesNotExistException()
        : HttpException(400, "Store does not exist.")
    {
    }
}
