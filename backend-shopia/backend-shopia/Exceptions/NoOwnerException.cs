using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoOwnerException()
        : HttpException(400, "No owner provided.")
    {
    }
}
