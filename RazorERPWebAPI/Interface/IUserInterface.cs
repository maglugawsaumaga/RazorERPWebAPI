using RazorERPWebAPI.Model;

namespace RazorERPWebAPI.Interface
{
    public interface IUserInterface
    {
        Task<IEnumerable<User>> GetUsersByCompany(int companyId, bool includeAdmins);
        Task<User> GetUserByUsername(string username, int companyId);
        Task<int> CreateUser(User user);
        Task<int> UpdateUser(User user);
        Task<int> DeleteUser(int id, int companyId);
    }
}
