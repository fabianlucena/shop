using RFHttpExceptions.Exceptions;

namespace backend_shopia.Exceptions
{
    public class PlanFeatureAlreadyExistsException()
        : HttpException(400, "Plan feature already exists.")
    {
    }
}
