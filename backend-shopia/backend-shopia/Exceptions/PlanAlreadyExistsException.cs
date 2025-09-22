using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class PlanAlreadyExistsException()
        : HttpException(400, "Plan already exists.")
    {
    }
}
