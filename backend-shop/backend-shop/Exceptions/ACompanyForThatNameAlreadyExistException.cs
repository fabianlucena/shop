using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class ACompanyForThatNameAlreadyExistException()
        : HttpException(400, "You already own a company with that name")
    {
    }
}
