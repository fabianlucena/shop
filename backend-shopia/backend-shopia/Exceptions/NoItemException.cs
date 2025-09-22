using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoItemException()
        : HttpException(400, "No item provided.")
    {
    }
}
