using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class NoItemUuidException()
        : HttpException(400, "No item UUID provided.")
    {
    }
}
