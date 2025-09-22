using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoCommerceException()
        : HttpException(400, "No commerce provided.")
    {
    }
}
