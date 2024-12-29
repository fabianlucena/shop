using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoPlanException()
        : HttpException(400, "No plan provided")
    {
    }
}
