namespace LMS.Domain.IService
{
    public interface ITokenService
    {
        string GenerateToken(string id, string username, List<string> role = null);
    }
}
