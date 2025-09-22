using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class MissingLengthForPropertyException(string property)
        : HttpException(500, "Missing length for property {0}.", property)
    {
    }
}
