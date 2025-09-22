using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoNameException()
        : HttpException(400, "No name provided.")
    {
    }
}
