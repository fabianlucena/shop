using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoBusinessException()
        : HttpException(400, "No business provided.")
    {
    }
}
