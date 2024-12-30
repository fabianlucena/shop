using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class BusinessLimitReachedException()
        : HttpException(400, "You can't create more businesses because you've reached the limit.")
    {
    }
}
