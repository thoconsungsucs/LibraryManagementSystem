using LMS.Domain.Exceptions;

namespace LMS.Domain.IService
{
    public interface ITokenService
    {
        string GenerateToken(int id, string username, List<string>? role = null);
        Result ValidateToken(string token);
    }
}
