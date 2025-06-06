// Services/UserStorageService.cs
using UserRegister.Models;

namespace UserRegister.Services
{
    public class UserStorageService
    {
        private readonly List<UserDto> _users = new();
        private int _nextId = 1;

        public List<UserDto> GetAll() => _users;

        public UserDto? Get(int id) => _users.FirstOrDefault(u => u.Id == id);

        public UserDto Add(UserDto user)
        {
            user.Id = _nextId++;
            _users.Add(user);
            return user;
        }

        public bool Update(int id, UserDto updated)
        {
            var user = Get(id);
            if (user == null) return false;

            user.Username = updated.Username;
            user.Email = updated.Email;
            user.Password = updated.Password;
            return true;
        }

        public bool Delete(int id) => _users.RemoveAll(u => u.Id == id) > 0;
    }
}
