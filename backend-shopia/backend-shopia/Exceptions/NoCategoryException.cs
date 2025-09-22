using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoCategoryException()
        : HttpException(400, "No category provided.")
    {
    }
}
