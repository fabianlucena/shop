using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class AStoreForThatNameAlreadyExistException()
        : HttpException(400, "You already own a store with that name.")
    {
    }
}
