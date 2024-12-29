using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class PlanAlreadyExistsException()
        : HttpException(400, "Plan already exists")
    {
    }
}
