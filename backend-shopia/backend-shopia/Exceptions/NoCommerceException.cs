using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoCommerceException()
        : HttpException(400, "No commerce provided.")
    {
    }
}
