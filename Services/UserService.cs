using Crud.Context;
using Crud.Interface;
using Crud.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using Hangfire;

using Sieve.Models;

namespace Crud.Services
{
    public class UserService : IUser
    {
        private readonly UserContext _dbContext;
        private IMapper _Map;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IHangFireJobs _hanFireJobs;
        public UserService(UserContext dbContext,IMapper Map,IBackgroundJobClient backgroundJobClient, IHangFireJobs hangFireJobs)
        {
            _dbContext =dbContext;
            _Map = Map;
            _backgroundJobClient = backgroundJobClient;
            _hanFireJobs = hangFireJobs;
            
        }
        public async Task<User> AddUser(UserDto user)
        {;
            var mapData = _Map.Map<User>(user);
            var data = await _dbContext.users.AddAsync(mapData); 
         await  _dbContext.SaveChangesAsync();
            var (subject, emailBody) = GetWelcomeMessage(user.name,"847865");
            _backgroundJobClient.Enqueue(() => _hanFireJobs.SendMail(user.email, subject, emailBody));
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
        public static (string subject, string emailBody) GetWelcomeMessage(string name, string password)
        {
            string subject = "Welcome to My Crud";
            string emailBody = $@"
<div style='font-family: Arial, sans-serif; text-align: center; background-color: #f9f9f9; padding: 20px; border: 1px solid #ddd;'>
    <h2 style='color: #333;'>Welcome to My Crud</h2>
    <p style='font-size: 16px; color: #555;'>Dear {name},</p>
    <p style='font-size: 16px; color: #555;'>Your account has been successfully created. Below are your login details:</p>
    <p style='font-size: 16px; color: #333;'><strong>Password:</strong> {password}</p>
    <p style='font-size: 16px; color: #555;'>Please log in and change your password at your earliest convenience.</p>
    <p style='font-size: 16px; color: #555;'>Thank you,<br/>[Kalash]</p>
</div>";
            return (subject, emailBody);
        }

    }
}
