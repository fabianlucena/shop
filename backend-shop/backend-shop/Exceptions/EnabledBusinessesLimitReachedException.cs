using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class EnabledBusinessesLimitReachedException()
        : HttpException(400, "You can't create more enabled businesses because you've reached the limit.")
    {
    }
}
