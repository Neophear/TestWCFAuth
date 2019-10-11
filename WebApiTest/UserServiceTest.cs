using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Services;
using Xunit;

namespace WebApiTest
{
    public class UserServiceTest
    {
        UserService service;

        public UserServiceTest()
        {
            service = new UserService(new UserDataMockup());
        }

        [Fact]
        public void TestAuthenticate()
        {
            service.Create(new User { Username = "user1" }, "pass1");
            Assert.Null(service.Authenticate("user1", "pass"));
            Assert.NotNull(service.Authenticate("user1", "pass1"));
        }
    }

    class UserDataMockup : IUserData
    {
        readonly ICollection<User> users = new List<User>();

        public void Add(User user) => users.Add(user);

        public User GetById(int id) => users.FirstOrDefault(u => u.Id == id);

        public User GetUser(string username) => users.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()));

        public IEnumerable<User> GetUsers() => users;
    }
}
