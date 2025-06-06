using UserRegister.Models;

namespace UserRegister.Services
{
    public class UserStorageService
    {
        // Список “in-memory” для хранения всех зарегистрированных пользователей.
        private readonly List<UserDto> _users = new();

        // Счётчик ID (если нужен уникальный идентификатор).
        private int _nextId = 1;

        // Возвращает всех пользователей
        public IEnumerable<UserDto> GetAll()
        {
            return _users;
        }

        // Возвращает одного пользователя по id
        public UserDto? Get(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        // Добавляет нового пользователя (назначает ему Id, кладёт в коллекцию)
        public UserDto Add(UserDto user)
        {
            user.Id = _nextId++;
            _users.Add(user);
            return user;
        }

        // Обновляет существующего пользователя (по id)
        public bool Update(int id, UserDto newUser)
        {
            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing is null) return false;

            // Здесь можно обновлять нужные поля:
            existing.Username = newUser.Username;
            existing.Email = newUser.Email;
            existing.Password = newUser.Password;
            return true;
        }

        // Удаляет пользователя по id
        public bool Delete(int id)
        {
            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing is null) return false;
            _users.Remove(existing);
            return true;
        }
    }
}
