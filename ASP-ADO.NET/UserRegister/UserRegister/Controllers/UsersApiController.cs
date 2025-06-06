using Microsoft.AspNetCore.Mvc;
using UserRegister.Models;
using UserRegister.Services;


[ApiController]
[Route("api/[controller]")]
public class UsersApiController : ControllerBase
{
    private readonly UserStorageService _storage;

    public UsersApiController(UserStorageService storage)
    {
        _storage = storage;
    }


    [HttpGet]
    public IActionResult GetAll() => Ok(_storage.GetAll());

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = _storage.Get(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public IActionResult Post(UserDto user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var added = _storage.Add(user);
        return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, UserDto user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = _storage.Update(id, user);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return _storage.Delete(id) ? NoContent() : NotFound();
    }
}
