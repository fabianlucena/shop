using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class TotalStoresLimitReachedException()
        : HttpException(400, "You can't create more stores because you've reached the limit.")
    {
    }
}
