using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class CategoryDoesNotExistException()
        : HttpException(400, "Category does not exist.")
    {
    }
}
