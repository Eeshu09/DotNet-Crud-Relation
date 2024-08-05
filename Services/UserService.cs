using Crud.Context;
using Crud.Interface;
using Crud.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using Sieve.Models;

namespace Crud.Services
{
    public class UserService : IUser
    {
        private readonly UserContext _dbContext;
        private IMapper _Map;
         public UserService(UserContext dbContext,IMapper Map)
        {
            _dbContext =dbContext;
            _Map = Map;
        }
        public async Task<User> AddUser(UserDto user)
        {;
            var mapData = _Map.Map<User>(user);
            var data = await _dbContext.users.AddAsync(mapData); // Assuming the DbSet<User> is named Users
         await  _dbContext.SaveChangesAsync();
            return data.Entity;

        }

        public async Task<bool> DeleteUser(int id)
        {

            var user = await _dbContext.users.FindAsync(id); 
            if (user == null)
            {
                return false;
            }
             _dbContext.users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public  async Task<User> GetUserById(int id)
        {
            try
            {
                var result = await _dbContext.users.FindAsync(id);
                if (result==null)
                {
                    throw new Exception("Geeting EXECPIITN WHILE FETCHING data");
                }
                return result;
            }catch(Exception ex) {
                return BadRequest("Internal Server Erro");
            }
           
        }

        private User BadRequest(string v)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateUser(int id, User user)
        {
            var result = await _dbContext.users.FindAsync(id);

            if (result == null)
            {
                return null;
            }

            // Update the user properties
            result.name = user.name;
            result.email = user.email;
            result.address = user.address;
            result.password = user.password;

            // Save changes to the database
           var res= _dbContext.users.Update(result);
            await _dbContext.SaveChangesAsync();

            return res.Entity;
        }



        public async Task<List<UserDto>> GetAllUser()
        {
            var data = await _dbContext.users.ToListAsync();
            
            if (data==null)
            {
                return new List<UserDto>();
            }
            var mapdata=_Map.Map<List<UserDto>>(data);
            return mapdata;
        }
        
      
        public async Task <IQueryable<User>>GetAllUsersQuery()
        {
            return   _dbContext.users.AsQueryable();
        }

     
    }
}
