using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoNameForCommerceException()
        : HttpException(400, "No name for commerce provided.")
    {
    }
}
