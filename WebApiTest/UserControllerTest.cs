using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Controllers;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;
using Xunit;

namespace WebApiTest
{
    public class UserControllerTest
    {
        UserController controller;

        public UserControllerTest()
        {
            IUserService service = new UserServiceMockup();
            AutoMapper.IMapper mapper = new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
            }));

            IOptions<AppSettings> options = new OptionsWrapper<AppSettings>(new AppSettings
            {
                Secret = "XXXX"
            });

            controller = new UserController(service, mapper, options);
        }

        [Fact]
        public void TestAuthenticate()
        {
            var okResult = controller.Authenticate(new UserDTO
            {
                Username = "user1",
                Password = "pass1"
            });

            Assert.IsType<OkObjectResult>(okResult);
        }
    }

    class UserServiceMockup : IUserService
    {
        public User Authenticate(string username, string password)
        {
            if (username == "user1" && password == "pass1")
            {
                return new User
                {
                    Username = username
                };
            }
            else
                return null;
        }

        public User Create(User user, string password)
        {
            throw new NotImplementedException();
        }
        
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(User user, string password = null)
        {
            throw new NotImplementedException();
        }
    }
}
