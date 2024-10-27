using Dapper;
using Microsoft.Data.SqlClient;
using RazorERPWebAPI.Interface;
using RazorERPWebAPI.Model;
using System.ComponentModel.Design;
using System.Data;

namespace RazorERPWebAPI.Repository
{
    public class UserRepository : IUserInterface
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<User>> GetUsersByCompany(int companyId, bool includeAdmins)
        {
            string query = includeAdmins
                ? "SELECT * FROM Users WHERE CompanyId = @CompanyId"
                : "SELECT * FROM Users WHERE CompanyId = @CompanyId AND Roles != 'Admin'";
            return await _dbConnection.QueryAsync<User>(query, new { CompanyId = companyId });
        }

        public async Task<User> GetUserByUsername(string username, int companyId)
        {
            var query = "SELECT * FROM Users WHERE Username = @Username AND CompanyId = @CompanyId";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Username = username, CompanyId = companyId });
        }

        public async Task<int> CreateUser(User user)
        {
            var query = @"INSERT INTO Users (Id, Username, PasswordHash, Roles, CompanyId) 
                      VALUES (@Id, @Username, @PasswordHash, @Roles, @CompanyId)";
            return await _dbConnection.ExecuteAsync(query, user);
        }

        public async Task<int> UpdateUser(User user)
        {
            var query = "UPDATE Users SET Username = @Username, Roles = @Roles WHERE Id = @Id AND CompanyId = @CompanyId";
            return await _dbConnection.ExecuteAsync(query, user);
        }

        public async Task<int> DeleteUser(int id, int companyId)
        {
            var query = "DELETE FROM Users WHERE Id = @Id AND CompanyId = @CompanyId";
            return await _dbConnection.ExecuteAsync(query, new { Id = id, CompanyId = companyId });
        }
    }


}
