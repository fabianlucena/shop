using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoPlanException()
        : HttpException(400, "No plan provided.")
    {
    }
}
