using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class EnabledStoresLimitReachedException()
        : HttpException(400, "You can't create more enabled stores because you've reached the limit.")
    {
    }
}
