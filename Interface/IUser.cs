using Crud.Model;
using Sieve.Models;

namespace Crud.Interface
{
    public interface IUser
    {
     Task<List<UserDto>> GetAllUser();
        Task<User> GetUserById(int id);
        Task<User> AddUser(UserDto user);
        Task<User> UpdateUser(int id,User user);
        Task<bool> DeleteUser(int id);
       Task< IQueryable<User>> GetAllUsersQuery();


    }
}
