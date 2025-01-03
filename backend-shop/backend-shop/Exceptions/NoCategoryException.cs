using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoCategoryException()
        : HttpException(400, "No category provided.")
    {
    }
}
