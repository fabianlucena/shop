using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class TotalItemsLimitReachedException()
        : HttpException(400, "You can't create more items because you've reached the limit.")
    {
    }
}
