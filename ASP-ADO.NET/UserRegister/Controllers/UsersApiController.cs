using AdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersApiController : ControllerBase
    {
        private static int _idCounter = 1;
        private readonly UserManager<IdentityUser> _userManager;

        public class UserDto
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
        }

        private class InternalUser : User
        {
            public int Id { get; set; }
        }

        public UsersApiController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        private static List<InternalUser> _db = new List<InternalUser>();
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _db.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("Пользователь не найден.");
            return Ok(new UserDto { Id = user.Id, Username = user.Username, Email = user.Email });
        }

        [HttpPost]
        public IActionResult Create([FromBody] User model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newUser = new InternalUser
            {
                Id = _idCounter++,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };

            _db.Add(newUser);
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User model)
        {
            var existing = _db.FirstOrDefault(u => u.Id == id);
            if (existing == null) return NotFound("Пользователь не найден.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            existing.Username = model.Username;
            existing.Email = model.Email;
            existing.Password = model.Password;
            existing.ConfirmPassword = model.ConfirmPassword;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _db.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("Пользователь не найден.");
            _db.Remove(user);
            return NoContent();
        }
    }
}
