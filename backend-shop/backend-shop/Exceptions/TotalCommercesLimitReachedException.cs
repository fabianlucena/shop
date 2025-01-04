using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class TotalCommercesLimitReachedException()
        : HttpException(400, "You can't create more commerces because you've reached the limit.")
    {
    }
}
