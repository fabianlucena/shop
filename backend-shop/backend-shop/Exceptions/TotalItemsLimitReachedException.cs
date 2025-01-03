using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class TotalItemsLimitReachedException()
        : HttpException(400, "You can't create more items because you've reached the limit.")
    {
    }
}
