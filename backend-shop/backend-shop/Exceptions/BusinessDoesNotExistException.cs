using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class BusinessDoesNotExistException()
        : HttpException(400, "Business does not exist.")
    {
    }
}
