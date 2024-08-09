namespace LMS.Domain.IService
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
