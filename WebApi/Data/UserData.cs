using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Data
{
    public interface IUserData
    {
        User GetUser(string username);
        IEnumerable<User> GetUsers();
        User GetById(int id);
        void Add(User user);
    }

    public class UserData : IUserData
    {
        private readonly ICollection<User> users = new List<User>();

        public User GetById(int id) => users.FirstOrDefault(u => u.Id == id);

        public User GetUser(string username) => users.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()));

        public IEnumerable<User> GetUsers() => users;

        public void Add(User user) => users.Add(user);
    }
}
