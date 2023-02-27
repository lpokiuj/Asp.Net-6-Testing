using testt.Models;

namespace testt.Services
{
    public interface IJWTTokenServices
    {
        JWTTokens Authenticate();
    }
}
