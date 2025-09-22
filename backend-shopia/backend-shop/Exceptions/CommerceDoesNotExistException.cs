using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class CommerceDoesNotExistException()
        : HttpException(400, "Commerce does not exist.")
    {
    }
}
