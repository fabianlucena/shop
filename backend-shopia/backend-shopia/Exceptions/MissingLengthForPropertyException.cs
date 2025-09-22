using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class MissingLengthForPropertyException(string property)
        : HttpException(500, "Missing length for property {0}.", property)
    {
    }
}
