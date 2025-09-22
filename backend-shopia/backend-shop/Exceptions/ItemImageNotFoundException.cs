using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class ItemImageNotFoundException()
        : HttpException(404, "Item image not found.")
    {
    }
}
