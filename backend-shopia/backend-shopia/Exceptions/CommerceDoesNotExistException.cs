using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class CommerceDoesNotExistException()
        : HttpException(400, "Commerce does not exist.")
    {
    }
}
