using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class CategoryDoesNotExistException()
        : HttpException(400, "Categorys does not exist.")
    {
    }
}
