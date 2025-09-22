using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class AStoreForThatNameAlreadyExistException()
        : HttpException(400, "You already own a store with that name.")
    {
    }
}
