using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class CategoryDoesNotExistException()
        : HttpException(400, "Category does not exist.")
    {
    }
}
