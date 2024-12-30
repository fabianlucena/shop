using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class ABusinessForThatNameAlreadyExistException()
        : HttpException(400, "You already own a business with that name.")
    {
    }
}
