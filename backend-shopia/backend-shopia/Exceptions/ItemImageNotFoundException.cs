using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class ItemImageNotFoundException()
        : HttpException(404, "Item image not found.")
    {
    }
}
