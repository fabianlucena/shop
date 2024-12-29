using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoNameForCompanyException()
        : HttpException(400, "No name for company provided")
    {
    }
}
