using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoNameForBusinessException()
        : HttpException(400, "No name for business provided.")
    {
    }
}
