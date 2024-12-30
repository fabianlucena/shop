using RFHttpExceptions.Exceptions;

namespace backend_shop.Exceptions
{
    public class PlanFeatureAlreadyExistsException()
        : HttpException(400, "Plan feature already exists.")
    {
    }
}
