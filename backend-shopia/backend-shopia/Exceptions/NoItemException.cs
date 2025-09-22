using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoItemException()
        : HttpException(400, "No item provided.")
    {
    }
}
