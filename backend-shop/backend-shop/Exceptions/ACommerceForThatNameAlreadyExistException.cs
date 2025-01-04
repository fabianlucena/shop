using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class ACommerceForThatNameAlreadyExistException()
        : HttpException(400, "You already own a commerce with that name.")
    {
    }
}
