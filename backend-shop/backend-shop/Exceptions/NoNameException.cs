using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoNameException()
        : HttpException(400, "No name provided.")
    {
    }
}
