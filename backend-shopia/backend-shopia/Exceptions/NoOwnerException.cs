using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoOwnerException()
        : HttpException(400, "No owner provided.")
    {
    }
}
