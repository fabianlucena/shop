using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class NoItemUuidException()
        : HttpException(400, "No item UUID provided.")
    {
    }
}
