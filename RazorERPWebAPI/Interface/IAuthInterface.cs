using RazorERPWebAPI.Model;

namespace RazorERPWebAPI.Interface
{
    public interface IAuthInterface
    {
        Task<string> GenerateToken(User user);

    }
}
