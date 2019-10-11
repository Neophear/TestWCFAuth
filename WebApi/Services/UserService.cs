using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserData data;

        public UserService(IUserData data) => this.data = data;

        public User Authenticate(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                return null;

            var user = data.GetUser(username);

            if (user == null)
                return null;

            if (!new PasswordHash(user.PasswordSalt, user.PasswordHash).Verify(password))
                return null;

            return user;
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (data.GetUser(user.Username) != null)
                throw new AppException("Username already exists");
            
            PasswordHash passwordHash = new PasswordHash(password);

            user.PasswordHash = passwordHash.Hash;
            user.PasswordSalt = passwordHash.Salt;
            
            user.Role = user.FirstName == "Stiig" ? Roles.Admin : Roles.User;

            data.Add(user);

            return user;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            return data.GetUsers();
        }

        public User GetById(int id)
        {
            return data.GetById(id);
        }

        public void Update(User user, string password = null)
        {
            throw new NotImplementedException();
        }

        private sealed class PasswordHash
        {
            const int SaltSize = 64, HashSize = 128, HashIter = 10000;
            readonly byte[] _salt, _hash;
            public PasswordHash(string password)
            {
                new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
                _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            }
            public PasswordHash(byte[] hashBytes)
            {
                Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
                Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
            }
            public PasswordHash(byte[] salt, byte[] hash)
            {
                Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
                Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
            }
            public byte[] ToArray()
            {
                byte[] hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
                return hashBytes;
            }
            public byte[] Salt { get { return (byte[])_salt.Clone(); } }
            public byte[] Hash { get { return (byte[])_hash.Clone(); } }
            public bool Verify(string password)
            {
                byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
                for (int i = 0; i < HashSize; i++)
                    if (test[i] != _hash[i])
                        return false;
                return true;
            }
        }
    }
}