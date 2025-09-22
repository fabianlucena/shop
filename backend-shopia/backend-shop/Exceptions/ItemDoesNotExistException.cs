using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class ItemDoesNotExistException()
        : HttpException(400, "Item does not exist.")
    {
    }
}
