using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockTracker.Extensions;
using StockTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BCrypt.Net;

namespace StockTracker.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
        User AddUser(string userName, string password);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private DbSet<User> _users;
        private DbContext _context;


        // inject your database here for user validation
        public UserService(ILogger<UserService> logger, DbContext context)
        {
            _logger = logger;
            _context = context;
            _users = context.ResolveDbSet<User>() as DbSet<User>;
        }

        public User AddUser(string username, string password)
        {
            User newUser = new Models.User { email = username, password = HashPassword(password) };
            
            _users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }


        public bool IsValidUserCredentials(string email, string password)
        {
            _logger.LogInformation($"Validating user [{email}]");
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            foreach(var user in _users.ToList())
            {
                if (user.email == email && VerifyPassword(password, user.password))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAnExistingUser(string userName)
        {
            // return _users.ContainsKey(userName);
            return _users.Any(u => u.email == userName);
        }

        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userName == "admin")
            {
                return UserRoles.Admin;
            }

            return UserRoles.BasicUser;
        }

        public string HashPassword(string _password)
        {
            return BCrypt.Net.BCrypt.HashPassword(_password);
        }

        public bool VerifyPassword(string _password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(_password, passwordHash);
        }
    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}
