# Структура проекта: WebApplication2

```
WebApplication2/
├── WebApplication2/
│   ├── Controllers/
│   │   ├── BookingsController.cs
│   │   ├── StudentsController.cs
│   │   ├── TasksController.cs
│   │   └── WeatherForecastController.cs
│   ├── Models/
│   │   ├── Booking.cs
│   │   ├── Student.cs
│   │   └── TaskItem.cs
│   ├── Program.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Services/
│   │   ├── BookingService.cs
│   │   ├── StudentService.cs
│   │   └── TaskService.cs
│   ├── WeatherForecast.cs
│   ├── WebApplication2.csproj
│   ├── WebApplication2.csproj.user
│   ├── WebApplication2.http
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── bin/
│   │   └── Debug/
│   │       └── net8.0/
│   │           ├── WebApplication2.deps.json
│   │           ├── WebApplication2.pdb
│   │           ├── WebApplication2.runtimeconfig.json
│   │           ├── appsettings.Development.json
│   │           └── appsettings.json
│   └── obj/
│       ├── Debug/
│       │   └── net8.0/
│       │       ├── WebApplication2.AssemblyInfo.cs
│       │       ├── WebApplication2.AssemblyInfoInputs.cache
│       │       ├── WebApplication2.GeneratedMSBuildEditorConfig.editorconfig
│       │       ├── WebApplication2.GlobalUsings.g.cs
│       │       ├── WebApplication2.MvcApplicationPartsAssemblyInfo.cache
│       │       ├── WebApplication2.MvcApplicationPartsAssemblyInfo.cs
│       │       ├── WebApplication2.assets.cache
│       │       ├── WebApplication2.csproj.AssemblyReference.cache
│       │       ├── WebApplication2.csproj.CopyComplete
│       │       ├── WebApplication2.csproj.CoreCompileInputs.cache
│       │       ├── WebApplication2.csproj.FileListAbsolute.txt
│       │       ├── WebApplication2.genruntimeconfig.cache
│       │       ├── WebApplication2.pdb
│       │       ├── WebApplication2.sourcelink.json
│       │       ├── ref/

│       │       ├── refint/

│       │       ├── staticwebassets/
│       │       │   ├── msbuild.build.WebApplication2.props
│       │       │   ├── msbuild.buildMultiTargeting.WebApplication2.props
│       │       │   └── msbuild.buildTransitive.WebApplication2.props
│       │       └── staticwebassets.build.json
│       ├── WebApplication2.csproj.nuget.dgspec.json
│       ├── WebApplication2.csproj.nuget.g.props
│       ├── WebApplication2.csproj.nuget.g.targets
│       ├── project.assets.json
│       └── project.nuget.cache
└── WebApplication2.sln
```

# Содержимое файлов

## WebApplication2/Controllers/BookingsController.cs

```csharp
﻿using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingsController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("available-resources")]
    public ActionResult<List<Resource>> GetAvailableResources(
        [FromQuery] DateTime startTime,
        [FromQuery] DateTime endTime,
        [FromQuery] string? resourceType = null)
    {
        var resources = _bookingService.GetAvailableResources(startTime, endTime, resourceType);
        return Ok(resources);
    }

    [HttpPost]
    public ActionResult<Booking> CreateBooking(Booking booking)
    {
        var created = _bookingService.CreateBooking(booking);
        if (created == null) return BadRequest("Resource not available or doesn't exist");
        return CreatedAtAction(nameof(GetBookings), new { id = created.Id }, created);
    }

    [HttpGet]
    public ActionResult<List<Booking>> GetBookings()
    {
        return Ok(_bookingService.GetAllBookings());
    }

    [HttpDelete("{id}")]
    public IActionResult CancelBooking(int id)
    {
        if (!_bookingService.CancelBooking(id))
            return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
    {
        if (!_bookingService.UpdateBooking(id, request.StartTime, request.EndTime))
            return BadRequest("Booking not found or time slot not available");
        return NoContent();
    }
}

public class UpdateBookingRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
```

---


## WebApplication2/Controllers/StudentsController.cs

```csharp
﻿using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentsController(StudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public ActionResult<List<Student>> GetAll()
    {
        return Ok(_studentService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetById(int id)
    {
        var student = _studentService.GetById(id);
        if (student == null) return NotFound();
        return Ok(student);
    }

    [HttpPost]
    public ActionResult<Student> Create(Student student)
    {
        var created = _studentService.Add(student);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Student student)
    {
        if (!_studentService.Update(id, student))
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_studentService.Delete(id))
            return NotFound();
        return NoContent();
    }
}
```

---


## WebApplication2/Controllers/TasksController.cs

```csharp
﻿using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingsController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("available-resources")]
    public ActionResult<List<Resource>> GetAvailableResources(
        [FromQuery] DateTime startTime,
        [FromQuery] DateTime endTime,
        [FromQuery] string? resourceType = null)
    {
        var resources = _bookingService.GetAvailableResources(startTime, endTime, resourceType);
        return Ok(resources);
    }

    [HttpPost]
    public ActionResult<Booking> CreateBooking(Booking booking)
    {
        var created = _bookingService.CreateBooking(booking);
        if (created == null) return BadRequest("Resource not available or doesn't exist");
        return CreatedAtAction(nameof(GetBookings), new { id = created.Id }, created);
    }

    [HttpGet]
    public ActionResult<List<Booking>> GetBookings()
    {
        return Ok(_bookingService.GetAllBookings());
    }

    [HttpDelete("{id}")]
    public IActionResult CancelBooking(int id)
    {
        if (!_bookingService.CancelBooking(id))
            return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
    {
        if (!_bookingService.UpdateBooking(id, request.StartTime, request.EndTime))
            return BadRequest("Booking not found or time slot not available");
        return NoContent();
    }
}

public class UpdateBookingRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
```

---


## WebApplication2/Controllers/WeatherForecastController.cs

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

```

---


## WebApplication2/Models/Booking.cs

```csharp
﻿namespace WebApplication2.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string ResourceType { get; set; } = string.Empty;
        public string ResourceId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }

    public class Resource
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}

```

---


## WebApplication2/Models/Student.cs

```csharp
﻿namespace WebApplication2.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}

```

---


## WebApplication2/Models/TaskItem.cs

```csharp
﻿namespace WebApplication2.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.New;
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Completed
    }
}

```

---


## WebApplication2/Program.cs

```csharp
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
```

---


## WebApplication2/Properties/launchSettings.json

```json
﻿{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:2098",
      "sslPort": 44362
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5139",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7046;http://localhost:5139",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

```

---


## WebApplication2/Services/BookingService.cs

```csharp
﻿using WebApplication2.Models;

public class BookingService
{
    private readonly List<Booking> _bookings = new();
    private readonly List<Resource> _resources = new()
    {
        new Resource { Id = "hotel1", Type = "Hotel", Name = "Hotel A" },
        new Resource { Id = "hotel2", Type = "Hotel", Name = "Hotel B" },
        new Resource { Id = "table1", Type = "Table", Name = "Table 1" },
        new Resource { Id = "table2", Type = "Table", Name = "Table 2" }
    };
    private int _nextId = 1;

    public List<Resource> GetAvailableResources(DateTime startTime, DateTime endTime, string? resourceType = null)
    {
        var bookedResourceIds = _bookings
            .Where(b => b.StartTime < endTime && b.EndTime > startTime)
            .Select(b => b.ResourceId)
            .ToHashSet();

        var availableResources = _resources
            .Where(r => !bookedResourceIds.Contains(r.Id));

        if (!string.IsNullOrEmpty(resourceType))
            availableResources = availableResources.Where(r => r.Type.Equals(resourceType, StringComparison.OrdinalIgnoreCase));

        return availableResources.ToList();
    }

    public Booking? CreateBooking(Booking booking)
    {
        var resource = _resources.FirstOrDefault(r => r.Id == booking.ResourceId);
        if (resource == null) return null;

        var isAvailable = !_bookings.Any(b =>
            b.ResourceId == booking.ResourceId &&
            b.StartTime < booking.EndTime &&
            b.EndTime > booking.StartTime);

        if (!isAvailable) return null;

        booking.Id = _nextId++;
        booking.ResourceType = resource.Type;
        _bookings.Add(booking);
        return booking;
    }

    public bool CancelBooking(int id)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        if (booking == null) return false;

        _bookings.Remove(booking);
        return true;
    }

    public bool UpdateBooking(int id, DateTime startTime, DateTime endTime)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        if (booking == null) return false;

        var isAvailable = !_bookings.Any(b =>
            b.Id != id &&
            b.ResourceId == booking.ResourceId &&
            b.StartTime < endTime &&
            b.EndTime > startTime);

        if (!isAvailable) return false;

        booking.StartTime = startTime;
        booking.EndTime = endTime;
        return true;
    }

    public List<Booking> GetAllBookings() => _bookings;
}
```

---


## WebApplication2/Services/StudentService.cs

```csharp
﻿using System.Xml.Linq;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class StudentService
    {
        private readonly List<Student> _students = new();
        private int _nextId = 1;

        public List<Student> GetAll() => _students;

        public Student? GetById(int id) => _students.FirstOrDefault(s => s.Id == id);

        public Student Add(Student student)
        {
            student.Id = _nextId++;
            _students.Add(student);
            return student;
        }

        public bool Update(int id, Student student)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            existing.Name = student.Name;
            existing.Email = student.Email;
            existing.Age = student.Age;
            return true;
        }

        public bool Delete(int id)
        {
            var student = GetById(id);
            if (student == null) return false;

            _students.Remove(student);
            return true;
        }
    }
}

```

---


## WebApplication2/Services/TaskService.cs

```csharp
﻿using System.Xml.Linq;
using WebApplication2.Models;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public List<TaskItem> GetAll(TaskStatus? status = null)
    {
        if (status.HasValue)
            return _tasks.Where(t => t.Status == status.Value).ToList();
        return _tasks;
    }

    public TaskItem? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

    public TaskItem Add(TaskItem task)
    {
        task.Id = _nextId++;
        _tasks.Add(task);
        return task;
    }

    public bool UpdateStatus(int id, TaskStatus status)
    {
        var task = GetById(id);
        if (task == null) return false;

        task.Status = status;
        return true;
    }

    public bool Delete(int id)
    {
        var task = GetById(id);
        if (task == null) return false;

        _tasks.Remove(task);
        return true;
    }
}
```

---


## WebApplication2/WeatherForecast.cs

```csharp
namespace WebApplication2
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}

```

---


## WebApplication2/WebApplication2.csproj

```text
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
  </ItemGroup>

  <ItemGroup>
    <Folder Include="Models\" />
  </ItemGroup>

</Project>

```

---


## WebApplication2/WebApplication2.csproj.user

```text
﻿<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="Current" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <ActiveDebugProfile>https</ActiveDebugProfile>
  </PropertyGroup>
</Project>
```

---


## WebApplication2/WebApplication2.http

```text
@WebApplication2_HostAddress = http://localhost:5139

GET {{WebApplication2_HostAddress}}/weatherforecast/
Accept: application/json

###

```

---


## WebApplication2/appsettings.Development.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

```

---


## WebApplication2/appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

---


## WebApplication2/bin/Debug/net8.0/WebApplication2.deps.json

```json
{
  "runtimeTarget": {
    "name": ".NETCoreApp,Version=v8.0",
    "signature": ""
  },
  "compilationOptions": {},
  "targets": {
    ".NETCoreApp,Version=v8.0": {
      "WebApplication2/1.0.0": {
        "dependencies": {
          "Swashbuckle.AspNetCore": "6.6.2"
        },
        "runtime": {
          "WebApplication2.dll": {}
        }
      },
      "Microsoft.Extensions.ApiDescription.Server/6.0.5": {},
      "Microsoft.OpenApi/1.6.14": {
        "runtime": {
          "lib/netstandard2.0/Microsoft.OpenApi.dll": {
            "assemblyVersion": "1.6.14.0",
            "fileVersion": "1.6.14.0"
          }
        }
      },
      "Swashbuckle.AspNetCore/6.6.2": {
        "dependencies": {
          "Microsoft.Extensions.ApiDescription.Server": "6.0.5",
          "Swashbuckle.AspNetCore.Swagger": "6.6.2",
          "Swashbuckle.AspNetCore.SwaggerGen": "6.6.2",
          "Swashbuckle.AspNetCore.SwaggerUI": "6.6.2"
        }
      },
      "Swashbuckle.AspNetCore.Swagger/6.6.2": {
        "dependencies": {
          "Microsoft.OpenApi": "1.6.14"
        },
        "runtime": {
          "lib/net8.0/Swashbuckle.AspNetCore.Swagger.dll": {
            "assemblyVersion": "6.6.2.0",
            "fileVersion": "6.6.2.401"
          }
        }
      },
      "Swashbuckle.AspNetCore.SwaggerGen/6.6.2": {
        "dependencies": {
          "Swashbuckle.AspNetCore.Swagger": "6.6.2"
        },
        "runtime": {
          "lib/net8.0/Swashbuckle.AspNetCore.SwaggerGen.dll": {
            "assemblyVersion": "6.6.2.0",
            "fileVersion": "6.6.2.401"
          }
        }
      },
      "Swashbuckle.AspNetCore.SwaggerUI/6.6.2": {
        "runtime": {
          "lib/net8.0/Swashbuckle.AspNetCore.SwaggerUI.dll": {
            "assemblyVersion": "6.6.2.0",
            "fileVersion": "6.6.2.401"
          }
        }
      }
    }
  },
  "libraries": {
    "WebApplication2/1.0.0": {
      "type": "project",
      "serviceable": false,
      "sha512": ""
    },
    "Microsoft.Extensions.ApiDescription.Server/6.0.5": {
      "type": "package",
      "serviceable": true,
      "sha512": "sha512-Ckb5EDBUNJdFWyajfXzUIMRkhf52fHZOQuuZg/oiu8y7zDCVwD0iHhew6MnThjHmevanpxL3f5ci2TtHQEN6bw==",
      "path": "microsoft.extensions.apidescription.server/6.0.5",
      "hashPath": "microsoft.extensions.apidescription.server.6.0.5.nupkg.sha512"
    },
    "Microsoft.OpenApi/1.6.14": {
      "type": "package",
      "serviceable": true,
      "sha512": "sha512-tTaBT8qjk3xINfESyOPE2rIellPvB7qpVqiWiyA/lACVvz+xOGiXhFUfohcx82NLbi5avzLW0lx+s6oAqQijfw==",
      "path": "microsoft.openapi/1.6.14",
      "hashPath": "microsoft.openapi.1.6.14.nupkg.sha512"
    },
    "Swashbuckle.AspNetCore/6.6.2": {
      "type": "package",
      "serviceable": true,
      "sha512": "sha512-+NB4UYVYN6AhDSjW0IJAd1AGD8V33gemFNLPaxKTtPkHB+HaKAKf9MGAEUPivEWvqeQfcKIw8lJaHq6LHljRuw==",
      "path": "swashbuckle.aspnetcore/6.6.2",
      "hashPath": "swashbuckle.aspnetcore.6.6.2.nupkg.sha512"
    },
    "Swashbuckle.AspNetCore.Swagger/6.6.2": {
      "type": "package",
      "serviceable": true,
      "sha512": "sha512-ovgPTSYX83UrQUWiS5vzDcJ8TEX1MAxBgDFMK45rC24MorHEPQlZAHlaXj/yth4Zf6xcktpUgTEBvffRQVwDKA==",
      "path": "swashbuckle.aspnetcore.swagger/6.6.2",
      "hashPath": "swashbuckle.aspnetcore.swagger.6.6.2.nupkg.sha512"
    },
    "Swashbuckle.AspNetCore.SwaggerGen/6.6.2": {
      "type": "package",
      "serviceable": true,
      "sha512": "sha512-zv4ikn4AT1VYuOsDCpktLq4QDq08e7Utzbir86M5/ZkRaLXbCPF11E1/vTmOiDzRTl0zTZINQU2qLKwTcHgfrA==",
      "path": "swashbuckle.aspnetcore.swaggergen/6.6.2",
      "hashPath": "swashbuckle.aspnetcore.swaggergen.6.6.2.nupkg.sha512"
    },
    "Swashbuckle.AspNetCore.SwaggerUI/6.6.2": {
      "type": "package",
      "serviceable": true,
      "sha512": "sha512-mBBb+/8Hm2Q3Wygag+hu2jj69tZW5psuv0vMRXY07Wy+Rrj40vRP8ZTbKBhs91r45/HXT4aY4z0iSBYx1h6JvA==",
      "path": "swashbuckle.aspnetcore.swaggerui/6.6.2",
      "hashPath": "swashbuckle.aspnetcore.swaggerui.6.6.2.nupkg.sha512"
    }
  }
}
```

---


## WebApplication2/bin/Debug/net8.0/WebApplication2.pdb

```text
BSJB         PDB v1.0       |   d   #Pdb    à   Ä  #~  ¤     #Strings    °     #US ´     #GUID   4  V  #Blob   ô5 j@ÍÅ3èg¾½(è¨  W¢		
     (               $   $                     	                     ¯                       
   x    °  ¼  ð  ü  P b    ° ï   ÈS     ÌU ÓU ÚU áU èU @V GV     NV dV ~V         V        -                                                                          	                               6   
        ^           T             sU ïU            ÉU'  Â'  '  
  ¶  ²Ö  Ñö  " uV xV   builder        Ð)¸Bw¬øbQ?ÆÓS ÀO£¡W&inF´­°FõþVÌ 8Mì%«5jìþµÐJÚFb»KØGM~n	\L®ÚËºjt
¨R_uÅ¾E´¸ qåR½L C¦@oI§0ÖOIyÞ D:Study3_Course
2_SemesterCollege-projects_3.2ASP-ADO.NETWebApplication2ControllersWeatherForecastController.cs\
3??O[ Ï0Â¦®s2Ýóæ1Nßc,Eñµ@`,<p
Program.cs\
3??¥ ÈÆ³F ¹HqÚÏqÑj?6R¢Rñ&Á/YÉ'$WeatherForecast.cs\
3??Ý ¤ðQp*¤ 41J,¢Qó"Y\ø±1$objDebugnet8.0!WebApplication2.GlobalUsings.g.cs\
3??!'. Ái7Â%ô±þ§{¾ð@¢|Î*·í("o^5Ë  Û1ï|y;÷*eU<úu&v«5ôí­(ì"8c/C¾¤çYO¡¶¯Ê¼ßú2vÚe-ìQt:[xbÚpµ5Öà»)ÎX"æTIÂÂgC¾/ÈÕpoM1Ä2ÉCX×) Hö?ä3'cjÍNõÝøbçë²ú¹ÿ [4ÓU0·xn@âjú©¿óÉn=è²1?Nm59Ò7..NETCoreApp,Version=v8.0.AssemblyAttributes.cs\
3??!'P ÷¥Ùx6%á9ÿá:P80ö¸Zöß¬v´Ê    // <autogenerated />
using System;
using System.Reflection;
[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
WebApplication2.AssemblyInfo.cs\
3??!'~ ùV6ùL^Ð@,ºîãÉqìüÕÇÂËk·íi:é½  ­TIkÛ@¾òZJ´yS
nJ BhJr=¥±Ðb¤ÑÁ7;.í¡)¾öÚPp¼P¥ÞþÂÔ7cºàgÄ¼yß2O1ÍÃý=Ó$§4ÉaÀbRÁü:¨ü·°·~ÁÆDvñóÆ09¥ìàrïÌp.àÁØ@¿aòcSË>Lp û0Ü*dÿ¸lXFÉ*ÙGFÙ±,KaáQh¸¹~
%ò«23"?¢èOVÉ²©,Z£ú&`©Mqo*ï0VÝ«ã)Ì½=¯yVatÚGû_´t±=ùAKuÂ¡.RgM£iE/?¡b±®VÕ95ÿRî]ÿÕý½<ãq@.Û`ÑÉÓ¥ñ5Cæ	Ä'*õfaûøÏ£¾Þ;K¢Ûu!RÞÈ{vpÍõV+äUÎÁó[SÅMä©Æ="|Íy°=Í9ÙK³§$66ÎíiÞÄÍ$´þïEUìkÙÔõm»æ4çU*eÙÕJµêZ¾ë9%×/m/{&~î]Ôó=!ÛÑÿ«©»¾o® X¨ÖbwõÍ0#×)ì,ñÙyJÅ¼½|óÐ7ü72WebApplication2.MvcApplicationPartsAssemblyInfo.cs\
3??!'¼ [`BæðWäü·Ây¸"ömP çÃ0#  ­RËNÂ@ÝðVº`Z@LDW#ÆE)Cm,-i§v<.Ô°uëðE^¿pç¼3Æ%Ó´Í¹ç1wFÓ;ñ¦r/i1ùgcÊ¢
Ñ&ð
ÑÂß sKÑÄé¿=á³	ÝPß±xØæ¢c\ï.E"ÑÍ¤©NSzêà¦u]Ü->Í7Wo¤¸D¼É@0#âM'(>&Êy(:2¦)F(¥
9Àµ©xE£õåö¤ÖöEFkI»ÿEY·ÛÏHoRÊTkY®BKyñÑº{Ð[u'§ýÓî]j<¶kR#à¬ý=¥¬ê0Û¥×F°ZÙidHÑ6}/ðªæú9ãÏg´xoÒ|½îØ¦!9Ï¿@sß.í%JFp[Í;m« lYÌ?cnbÿFÚb/àcs¡`!5Å®µÔ=+ßæ¬àUØ©oX5ærR,¶S¡þ¼{"documents":{"D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\*":"https://raw.githubusercontent.com/Mason-Zaccaro/College-projects_3.2/7a9517801a8d1172fecc5542e1656680d8c238d3/*"}}version 2 compiler-version 4.8.0-7.23572.1+7b75981cf3bd520b86ec4ed00ec156c8bc48e4eb language C# source-file-count 7 output-kind ConsoleApplication optimization debug-plus platform AnyCpu runtime-version 4.8.9310.0 language-version 12.0 nullable Enable define TRACE,DEBUG,NET,NET8_0,NETCOREAPP,NET5_0_OR_GREATER,NET6_0_OR_GREATER,NET7_0_OR_GREATER,NET8_0_OR_GREATER,NETCOREAPP1_0_OR_GREATER,NETCOREAPP1_1_OR_GREATER,NETCOREAPP2_0_OR_GREATER,NETCOREAPP2_1_OR_GREATER,NETCOREAPP2_2_OR_GREATER,NETCOREAPP3_0_OR_GREATER,NETCOREAPP3_1_OR_GREATER À I&Microsoft.AspNetCore.Antiforgery.dll  v© à  ô
àkÆ MÃ¬dé@Microsoft.AspNetCore.Authentication.Abstractions.dll  Ã¨² À  qË]?gA©éÅ@§ÆMicrosoft.AspNetCore.Authentication.BearerToken.dll  ®z-Ò À  ºow©«ÊD©`^3¢CMicrosoft.AspNetCore.Authentication.Cookies.dll  Dó¨ À  Î£@ »½AJºuq¯ïMicrosoft.AspNetCore.Authentication.Core.dll  õÆ{á À  )ñ¾p¥ûLÌ@ ¿IêMicrosoft.AspNetCore.Authentication.dll  ÕÌ   çajG®#@¿¬úÉî3¯Microsoft.AspNetCore.Authentication.OAuth.dll  Ö×Ê  À  8RåzB­ø<ü®Microsoft.AspNetCore.Authorization.dll  ;~Û à  ìÀ~·K¸§è´@"Â¯Microsoft.AspNetCore.Authorization.Policy.dll  AÓ×­ À  +²Dx Eðp¤¨Microsoft.AspNetCore.Components.Authorization.dll  a«ÞÆ À  søÀ8ªðFª8:<ÝòMicrosoft.AspNetCore.Components.dll  Ïh¡³ ` £ÂRí³@z½qûæ©Microsoft.AspNetCore.Components.Endpoints.dll  z¥Õ à µú3ª°¤0J¥]6¡Q*Microsoft.AspNetCore.Components.Forms.dll  ·*ú À  ^1;L
xG½á°-41Microsoft.AspNetCore.Components.Server.dll  8NU   Û!ºx~÷qH¹É{øÓMicrosoft.AspNetCore.Components.Web.dll  Ypo¥   ¥·	ÙQJÅG´ ø°ØMicrosoft.AspNetCore.Connections.Abstractions.dll  ýý³ à  ïF-¼â¼BªÓÃÁ/æMicrosoft.AspNetCore.CookiePolicy.dll  ÷o À  oñkKÖhOÄÚ|÷ëMicrosoft.AspNetCore.Cors.dll  vøi À  æzxAÄA1µÐklMicrosoft.AspNetCore.Cryptography.Internal.dll  ´UL   1Â±Ý/¼E¿?]½Q>Microsoft.AspNetCore.Cryptography.KeyDerivation.dll  a    ;5Í
eY ECÃû©z Microsoft.AspNetCore.DataProtection.Abstractions.dll  <    E J¶O¥ËáùÔ×ÂMicrosoft.AspNetCore.DataProtection.dll  Ý   ÌV©sçÑ Kýfõ2|qMicrosoft.AspNetCore.DataProtection.Extensions.dll  ùk]ê    »Ëé ÷¶L8¤ÙXMicrosoft.AspNetCore.Diagnostics.Abstractions.dll  ]Ùa    Voé?þ£ J"F·-A/Microsoft.AspNetCore.Diagnostics.dll  e8±ý   [:jUwÅD§sN|¼3ÈMicrosoft.AspNetCore.Diagnostics.HealthChecks.dll  æ~
á    § g@­	Cºók£Õ^ßMicrosoft.AspNetCore.dll  ²® à  Bµèhó@Ì
3¢¿§Microsoft.AspNetCore.HostFiltering.dll  «L¾    «»·Îr\F¦Ñ-4j{PMicrosoft.AspNetCore.Hosting.Abstractions.dll  dÕ^ø À  0ë4ô*LÕs÷P~Microsoft.AspNetCore.Hosting.dll  KYìÉ  =Ú3_ ¿B¡\iS¼Microsoft.AspNetCore.Hosting.Server.Abstractions.dll  ©<iÛ    æ×mH»j>JTMicrosoft.AspNetCore.Html.Abstractions.dll  R¹æ    ±·o*ÃãKèqäüªcÇMicrosoft.AspNetCore.Http.Abstractions.dll  Ô2ÿ®   Ð×Ç¿GÒ­OI$Z¥ÅLÂMicrosoft.AspNetCore.Http.Connections.Common.dll  û_¶Û    ØEZàC§N#¢© BMicrosoft.AspNetCore.Http.Connections.dll  K @ ?ªtZ),Iýçf7aªMicrosoft.AspNetCore.Http.dll  Ï¡Jõ ` ëDWîÿ[L·á*&{Microsoft.AspNetCore.Http.Extensions.dll  î¨¿   Ý­/[öDo5<;(²¨Microsoft.AspNetCore.Http.Features.dll  ¿zf à  ót\éLH2Cî7ÔyMicrosoft.AspNetCore.Http.Results.dll  L`ñ± @ êz®Iª¾]H»öÂR(üMicrosoft.AspNetCore.HttpLogging.dll  }»[ý   ÷ 3I¨©ù2Microsoft.AspNetCore.HttpOverrides.dll  Sd@ À  ©8ó
 CÄ2äzËåQMicrosoft.AspNetCore.HttpsPolicy.dll  þlÑª    _9Üö3B®/Îó¥HMicrosoft.AspNetCore.Identity.dll  d²ù À JöÝ³iU´H©Ôm¹ÀÕMicrosoft.AspNetCore.Localization.dll  *y,Â À  BÉ8ÜáqI¥HÙ±µMicrosoft.AspNetCore.Localization.Routing.dll  rü    
êxh£áCW/mâMicrosoft.AspNetCore.Metadata.dll  ©³ú    YÊä
MqD_È6þÿMicrosoft.AspNetCore.Mvc.Abstractions.dll  /_û   §ÏN9G¦4KõU[Microsoft.AspNetCore.Mvc.ApiExplorer.dll  y)Á À  ¨1ÅTBô»Ã]1²IMicrosoft.AspNetCore.Mvc.Core.dll  åg÷ú   ¨æ5=ÀH´Ó[b&ÛAMicrosoft.AspNetCore.Mvc.Cors.dll  ¶øpé    ææ¼>CKC´ZÔcã{tMicrosoft.AspNetCore.Mvc.DataAnnotations.dll  ¶h~Ë À  ã¼¦ gÑîD(ì µMicrosoft.AspNetCore.Mvc.dll  Ð    IgàGYiH¥å6È3­þMicrosoft.AspNetCore.Mvc.Formatters.Json.dll  íaÊ    6bãíª@4G/ºr1¹YMicrosoft.AspNetCore.Mvc.Formatters.Xml.dll  Tnì À  bµíÐeI¡Fç|½BºMicrosoft.AspNetCore.Mvc.Localization.dll  ò aã À  O`91ÃM½µ)>QKMicrosoft.AspNetCore.Mvc.Razor.dll  Ï.Î @ íEQCã®wR½Microsoft.AspNetCore.Mvc.RazorPages.dll  Ï¼'¯ À NtÿªGëä¶ÆFúMicrosoft.AspNetCore.Mvc.TagHelpers.dll  TÒ @ à¯>0Fk@¶ 7¿Ý£,Microsoft.AspNetCore.Mvc.ViewFeatures.dll  g¦ @ E¹É¢	ôÝB³@Ùæ°Microsoft.AspNetCore.OutputCaching.dll  \ÊÄ   6V>z«L³iCPûMicrosoft.AspNetCore.RateLimiting.dll  Üm À  ì
¯½FKE¦þdTÏ7Microsoft.AspNetCore.Razor.dll  wQ}Ð À  Ñ½ÃrHRõdeù-ÃMicrosoft.AspNetCore.Razor.Runtime.dll  ¿ª:Ù À  á9÷Ç*B-øXU¬åMicrosoft.AspNetCore.RequestDecompression.dll  ±BÕ×    [J£NA¬Âÿ/ZHMicrosoft.AspNetCore.ResponseCaching.Abstractions.dll  Ç¹    S9×K»Ú-ÀªÅÍMicrosoft.AspNetCore.ResponseCaching.dll  0#ì à  {êtÀ³M0ó3ÏyMicrosoft.AspNetCore.ResponseCompression.dll  ôá À  +Tê+®ÐôIvæðOÎMicrosoft.AspNetCore.Rewrite.dll  Bô   Õ	Ï;BWFî¡7¢Microsoft.AspNetCore.Routing.Abstractions.dll  [¬ À  \ÐÌÆFD$?ÕåU¿Microsoft.AspNetCore.Routing.dll  Ùvò­ @ ¥+ñhØHO¯7;8~ôïMicrosoft.AspNetCore.Server.HttpSys.dll  c =¨ @ âW+QØ¢O¿P{¦µMicrosoft.AspNetCore.Server.IIS.dll  ö6â @ ÕÚwøHÅ¿5»`ÓMicrosoft.AspNetCore.Server.IISIntegration.dll  O¼Û À  ²SÓaKÕI>Microsoft.AspNetCore.Server.Kestrel.Core.dll  Êøæ  Ü¦¾E¸Eìò,uMicrosoft.AspNetCore.Server.Kestrel.dll  #'«    5kðä¥¹@Å©·û[0ºMicrosoft.AspNetCore.Server.Kestrel.Transport.NamedPipes.dll  <|ÄÈ à  iô~ÚHIQ;(Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.dll  Û×}ê   ª¦ðkg ì@¤
±PËQMLMicrosoft.AspNetCore.Server.Kestrel.Transport.Sockets.dll  n=¹Õ   ~æ0>Ó Bhò¹ó¶Z¯Microsoft.AspNetCore.Session.dll  8º À  £m7ú°ôD¹nd2FfMicrosoft.AspNetCore.SignalR.Common.dll  2^½ À  Îb¦õL)8P{òé)Microsoft.AspNetCore.SignalR.Core.dll  OÝËñ   làbw¯ F®°íºw^¥!Microsoft.AspNetCore.SignalR.dll  r¹¼Î    1ü]íÂÜæF¢ ÿèXMicrosoft.AspNetCore.SignalR.Protocols.Json.dll  ÏÅ£    "ùïJÆ"Ë9[Microsoft.AspNetCore.StaticFiles.dll  Ùi¿ à  LöÔF¤KÁ÷IøëMicrosoft.AspNetCore.WebSockets.dll  ^Yjç À  ¥m_ü/Ö®Kªm£d0îMicrosoft.AspNetCore.WebUtilities.dll  ?­   {ò¤!CðÎÓtMicrosoft.CSharp.dll  5¹÷Ó   ²¶oÂD°eÂvª¨ý!Microsoft.Extensions.Caching.Abstractions.dll  ¸rå»    N*[¬B³f3*½Microsoft.Extensions.Caching.Memory.dll  ?ñó°   ý÷ôêºJ´ÒS´´üMicrosoft.Extensions.Configuration.Abstractions.dll  e­í   Å8ÜcÉO÷N¼:Microsoft.Extensions.Configuration.Binder.dll  .q   7ÂÎWÑ?J(iÛW,Microsoft.Extensions.Configuration.CommandLine.dll  W%l   Ë21LO^,ïôçsMicrosoft.Extensions.Configuration.dll  fKÄ³    "o¤`D´£×ùöMicrosoft.Extensions.Configuration.EnvironmentVariables.dll  )¤ï   ÜmâÛÌkJôI E¢Microsoft.Extensions.Configuration.FileExtensions.dll  Í|Ë   §jÒ¥É½BmdØð!Microsoft.Extensions.Configuration.Ini.dll  .°~Ü   À®!BöG^7áüsWMicrosoft.Extensions.Configuration.Json.dll  0Zx¤   xüù(Q]éB¸WöUçÜMicrosoft.Extensions.Configuration.KeyPerFile.dll  [£ö    ªýö,B4?¬
,Microsoft.Extensions.Configuration.UserSecrets.dll  _©à   æß\,5c|Cª ^±æMicrosoft.Extensions.Configuration.Xml.dll  Ö»=Ú   J ÈêIUH¹ýâtè¹sMicrosoft.Extensions.DependencyInjection.Abstractions.dll  zô    è³ÿùt)E¡Õ*~²üMicrosoft.Extensions.DependencyInjection.dll  Sf¶   ¥ê±p«)J³ëqMæîQMicrosoft.Extensions.Diagnostics.Abstractions.dll  HØù   pÆÑ
MzÕ/?Â&Microsoft.Extensions.Diagnostics.dll  ÌÙ   ^G{^<ÿJ¤ã@öY0`ÝMicrosoft.Extensions.Diagnostics.HealthChecks.Abstractions.dll  ,kÙ    SX¬Þ¶µ@­#/ÀáªMicrosoft.Extensions.Diagnostics.HealthChecks.dll  ãE×ÿ à  
Hë¯ÆñA¨á[øYMicrosoft.Extensions.Features.dll  k>Ô     h`DÁé[Ø)Microsoft.Extensions.FileProviders.Abstractions.dll  	|   mhJ0ÝN|\­âMË»Microsoft.Extensions.FileProviders.Composite.dll  h8ÆÐ    .5ªA§áfÃHÛåMicrosoft.Extensions.FileProviders.Embedded.dll  	²» À  TÕw³3H¾¸8ì8ÂMicrosoft.Extensions.FileProviders.Physical.dll  ·r|ð   ëòÐYF®PMicrosoft.Extensions.FileSystemGlobbing.dll  ã!    U3W¿hõG¼4I2_¢Microsoft.Extensions.Hosting.Abstractions.dll  ê¤¬î    ÔÞ´§xCÛ£Ç"lMicrosoft.Extensions.Hosting.dll  Ý{~    Çr$¨ºF¯hîD?ÛMicrosoft.Extensions.Http.dll  T~Ú    4t²î*¨kMÃ³ûrMicrosoft.Extensions.Identity.Core.dll  nD¥ã  hl¬K)8µB¶ËGö.XMicrosoft.Extensions.Identity.Stores.dll  Ò¹°ã à  gS¯C¤èJí©wY-ÐMicrosoft.Extensions.Localization.Abstractions.dll  Ú
    [g¾Ð}O¬«þº<ÅªMicrosoft.Extensions.Localization.dll  Ë À  H}²PÉÓEÊK%¤TMicrosoft.Extensions.Logging.Abstractions.dll  ´ô¥    føä!ö<¾M!øÕ+Ý©Microsoft.Extensions.Logging.Configuration.dll  ýE»Ü   Ëå¾Ù®F»akâèØMicrosoft.Extensions.Logging.Console.dll  {°Ó    TäÊ¡wGÊ|:øMicrosoft.Extensions.Logging.Debug.dll  V'u£   
Æ_´+kMºÙýý»Î Microsoft.Extensions.Logging.dll  _Iê   2Üì<b@½¦/äMicrosoft.Extensions.Logging.EventLog.dll   åÆü   /öÞ`4N
i¥'ÄFkMicrosoft.Extensions.Logging.EventSource.dll  Å¼   ¨$²foFêZ>Microsoft.Extensions.Logging.TraceSource.dll  ½úWÌ   ^ ´÷®^8M¹Es§ïú
Microsoft.Extensions.ObjectPool.dll  è¥    q
<£XbNß[¯]PMicrosoft.Extensions.Options.ConfigurationExtensions.dll  )é   <bU£1I¾°òMicrosoft.Extensions.Options.DataAnnotations.dll  Çpâ½   ~Ã Vè@µ%î­>Microsoft.Extensions.Options.dll  h5 À  tã-WJ¯Æ¯Ô7èHMicrosoft.Extensions.Primitives.dll  µ&z    ï2¶!{E:â4"ÆMicrosoft.Extensions.WebEncoders.dll  wæöô    íB&*C©`o¡KÄMicrosoft.JSInterop.dll  ÒcF à  þÜ¾8ËBH¿äÕYÖyMicrosoft.Net.Http.Headers.dll  õ¢ à  +'"6O¤ø4µÛªÇMicrosoft.OpenApi.dll  gÜÌ À `Í«x¤ÃO®f´¡Microsoft.VisualBasic.Core.dll  ¼\¥Ö   <áÙVJ¾TÆ_Microsoft.VisualBasic.dll  jG¯ò   ¨\?uçFKpºymMicrosoft.Win32.Primitives.dll  »|   xÐ¬ËTK«.qÐï?¾CMicrosoft.Win32.Registry.dll  :Õ]©    |(M'4BÅ®{ñ\v'mscorlib.dll  O8´   .S	bC=\Ë¯netstandard.dll  úªôã À 7´©ÃþPBAoÔ¸+eïSwashbuckle.AspNetCore.Swagger.dll  ÿ¨    f4-QO×ÜD	iaSwashbuckle.AspNetCore.SwaggerGen.dll  bwÚÌ @ ±ÞàÆ"hI§	!éKùSwashbuckle.AspNetCore.SwaggerUI.dll  Â¨<È  # <@ëÁ09;D(ø(1Ïõ)System.AppContext.dll  ü£u    ×ý 1$éJ¯Ek	System.Buffers.dll  ¥Ã6¼   +«2LR¼ÝSystem.Collections.Concurrent.dll  H_©î    ê­|XÖ}ÈOQ«¶è¤ôSystem.Collections.dll   7ü©   ÂJäcáI÷4ÁC cQSystem.Collections.Immutable.dll   d¤ ` ªô0ÿ­b¼H¿9iw°CSystem.Collections.NonGeneric.dll  3*º    Ð)g§\qN¦®Æ3<RSystem.Collections.Specialized.dll  ©ÝÂ    !£ÏL÷WC^v«]ÌSystem.ComponentModel.Annotations.dll  ,ZE¬ À  è>>I¸ÎtIô%System.ComponentModel.DataAnnotations.dll  '¿þæ   §ëùbÅSI¶NtIÒ[qSystem.ComponentModel.dll  ×d¢   ò@uªY@L¢A4jÒkè System.ComponentModel.EventBasedAsync.dll  àÍß   ²"+>òL¨JâÈ?ëDìSystem.ComponentModel.Primitives.dll  Ç}9    · rNÂAµÐDYsùSystem.ComponentModel.TypeConverter.dll  ×UÌ à ïåçFL©.íñF}
ïSystem.Configuration.dll  4S    ûÚ44N´9Ko^<System.Console.dll  §äÅ    µNùÎ3üL­&=ÒSystem.Core.dll  íAÕ    hÛ%®W^¡N4Ñ	Úh/System.Data.Common.dll  °Kéò   ¡ôü	RpC¤ Ðs¦½(System.Data.DataSetExtensions.dll  ×Ã
÷   ý[§·7ÎAÝwÏ>)System.Data.dll  ¿	ªã    ¢\úOO~)¢Ò^åSystem.Diagnostics.Contracts.dll  	Ã   %9$cæF&MÁ®{System.Diagnostics.Debug.dll  èÔù   üðBfþö@ºü¯{VÎ;System.Diagnostics.DiagnosticSource.dll  bì- à  +ârÔêD{@B
íSystem.Diagnostics.EventLog.dll  ïñ5Ý À  «0²ÏøíKëjå» {System.Diagnostics.FileVersionInfo.dll  »
¯   +è'7M.mM³Ïûá_þSystem.Diagnostics.Process.dll  \Üå À  rh7=C£SK{yEª`System.Diagnostics.StackTrace.dll  8]tà    B¬¥Ãà"D¬UaëàjSystem.Diagnostics.TextWriterTraceListener.dll  »mHí   ·Á¶>áö
@{Ñ$·<õSystem.Diagnostics.Tools.dll  t¯¥   ÿ1¹l¾GµN©#2^System.Diagnostics.TraceSource.dll  ¤
@ì    ¢±"ñQ%Jªq¤æU´"System.Diagnostics.Tracing.dll  1á¢    Ñ~¥îKH©K¹ÞÇ¹;qõ8System.dll  lÓ   T=ìÛ)H¿<Æ4¶3ÀSystem.Drawing.dll  ²°ê   sÉÂPH¢)UVàÙSystem.Drawing.Primitives.dll  zX¡ À  åÔyT%xIF§BnÒ>System.Dynamic.Runtime.dll  ­À   À§K@5êDÌØæ&íSystem.Formats.Asn1.dll  ÔÀ    ¡Éûµ7Ñ;E©ú 4¥System.Formats.Tar.dll  ,ªÒê   ëõ¤4ìLr9ËzSystem.Globalization.Calendars.dll  Qò   @ãKÔÝJ£1Wý>ÆïSystem.Globalization.dll  °¯ú   jD3ßoñB 9i´BEîSystem.Globalization.Extensions.dll  béª   ëZ ôpqGùF3(wÆSystem.IO.Compression.Brotli.dll  äÛ   UØ²ó-[Oß³#Üfô4System.IO.Compression.dll  
Rù   »ðá°'¿F¸?
~Â-System.IO.Compression.FileSystem.dll  Â   ÷Ç½oÁúNºRæír7System.IO.Compression.ZipFile.dll  
s   ?M ¦æEÝ\©ËåzaSystem.IO.dll  ìæ¢   Y÷¬H¼¿·¡PþSystem.IO.FileSystem.AccessControl.dll  
 ÷¡   zm¡Ù×©¤@¿^XÐSystem.IO.FileSystem.dll  È¸¡   ©q¦Mÿ«ù^Á
TSystem.IO.FileSystem.DriveInfo.dll  T»ñÇ   ÈV¿MX;Fº`6 NÙovSystem.IO.FileSystem.Primitives.dll  þÿåÝ   zYQ	E·
F¾8±System.IO.FileSystem.Watcher.dll  ÃNh   ¿hÚÌ8FA¥;øý#wìÆSystem.IO.IsolatedStorage.dll  ý@°    ;j\«¯Sv@¯of2»ðSystem.IO.MemoryMappedFiles.dll  ÷zï   Û&o*ÂM²¹<¶ãJSystem.IO.Pipelines.dll  >`   |òÈÂÈ»I¶U.ùc¸¬System.IO.Pipes.AccessControl.dll  §ÚE¡   ¢óÒÆG ­M¤6çGhI~9System.IO.Pipes.dll  Íw§    WÐ\ôkG¿ßoöÈ ®System.IO.UnmanagedMemoryStream.dll  ÁKSî   ¨qpªeFÜÃÆzvwSystem.Linq.dll  Çs À  EÉ89jI«eáÕù7System.Linq.Expressions.dll  Dýz @ 
-FºäI{¹5¸¤ÓþSystem.Linq.Parallel.dll  "´Ø­ À  ®wzìÑ½A;Ò¾System.Linq.Queryable.dll  ñG¡ À  ë~»[cÎFH|¡K§(System.Memory.dll  i«@Ô   ò?Øjî@¸@¢æq=System.Net.dll   ùKÛ   c¤2·§K¨#Ú[Á|õxSystem.Net.Http.dll  Z!w   ì¢|è2M¸Nãòågë6System.Net.Http.Json.dll  ¦ÈÁ    àÍ'´«Ù=Eçsá8aSystem.Net.HttpListener.dll  pê    Ï×kòÿ·ºDrE[0½System.Net.Mail.dll  $%« À  ÚáþûrF¦M3_ÕÐBErSystem.Net.NameResolution.dll  qbº¤   ø#ûÕJµ6USystem.Net.NetworkInformation.dll  HYüÏ À  |uµ,kFæÃX¨ÇÂSystem.Net.Ping.dll  %¾Ä   ÆTf×êlE+HÛªRvcSystem.Net.Primitives.dll  y¨ÿ À  `KÑC5OYWs*·îSystem.Net.Quic.dll  Ulb    IÛÀÖV|HMzY&ô+System.Net.Requests.dll  çë§ À  @u.ØG¥òxhN*
 System.Net.Security.dll   Êý±   <6*K¤HN>:v(System.Net.ServicePoint.dll  n#ß   ÀúÂðñI_ô-äÊSystem.Net.Sockets.dll  L   !<³òóL]¯&<QûSystem.Net.WebClient.dll  CWÌ    wµºTÙÑF`fÏ?(iSystem.Net.WebHeaderCollection.dll  x:   îî{i H¨¡¹!ü¨System.Net.WebProxy.dll  & »   7~æED¶µziL|bSystem.Net.WebSockets.Client.dll  Y¥úÿ   |7)L&7cRþ³System.Net.WebSockets.dll  6B,    l¢ q`/£G$¶÷QgZSystem.Numerics.dll  Ì·ÔÜ   'æ£T|A¯,x##}ÿSystem.Numerics.Vectors.dll  fÿ à  Ï´p,QQH¶4ì*ë#èSystem.ObjectModel.dll  )¡ëË    µ_nÄY$EÖúM-
ÈSystem.Reflection.DispatchProxy.dll  a¥   ¾7
·OKK»õêoÀcVSystem.Reflection.dll  ,)¢   ×íÈ*¥eF®-ålK	System.Reflection.Emit.dll  ÅH0¬ à  è2eíáLL¾ìõ o[System.Reflection.Emit.ILGeneration.dll  Ýÿ´   S­M+ÐK;­â±-System.Reflection.Emit.Lightweight.dll  Eã   Lçwsbä©Cµµã
rM,System.Reflection.Extensions.dll  ÓÀê   £Ø\°QëyM³3õ LÐÒSystem.Reflection.Metadata.dll  qÿ   >Ò"¾:±LBîÁSystem.Reflection.Primitives.dll  ð(×í    GZÕÃÂkJk~GTSystem.Reflection.TypeExtensions.dll  éÆ   ­?éX>~$G·uÂd"ÚSystem.Resources.Reader.dll  HÐ¤   ¿Df¦¢H·[ëeÚSystem.Resources.ResourceManager.dll  Õs   iîÇ"BÛ©æ54System.Resources.Writer.dll  !wì   ÆÝ^÷pZIÙmvM÷eSystem.Runtime.CompilerServices.Unsafe.dll  Õf
Ñ   rC(óE.5ªÅìSystem.Runtime.CompilerServices.VisualC.dll  kz/Û   {4ç1 IyðSystem.Runtime.dll  JÙ¿ à è®?øJ¯+
|EQêSystem.Runtime.Extensions.dll  »£©   ¶bäìLº23CâSystem.Runtime.Handles.dll  Ôì+    ^ò¤Z0á @·&ªûl^System.Runtime.InteropServices.dll  ó5÷Û À 	K·ÌH-CdbHºÑ0System.Runtime.InteropServices.JavaScript.dll  ù[î    jJÄ¨_N+G¦n{0CSystem.Runtime.InteropServices.RuntimeInformation.dll  |£   g÷lL½Eq¤³System.Runtime.Intrinsics.dll  ìÿãá  bí/ÉÂè@ÓðÏ­â9System.Runtime.Loader.dll  Àë   ²âà<ö§@¤CÂÈ=É´System.Runtime.Numerics.dll  ]zÅ§ À  »¿H°EMï}nÉO3²System.Runtime.Serialization.dll  'Èê   (0°A¦]dáÈSystem.Runtime.Serialization.Formatters.dll  @ï    ¦Ð´¼ÒK¬*ÜØ1ëÌSystem.Runtime.Serialization.Json.dll  kÝÃ   ×¢ÂãÝAã: Ë5GSystem.Runtime.Serialization.Primitives.dll  Ü]ï   °áúO³ÝµCÂÍSystem.Runtime.Serialization.Xml.dll  2õ à  ÏuÚ7HgÒ%Y3System.Security.AccessControl.dll  q&Ì À  ·¤û å)¶K®,·whÆçSystem.Security.Claims.dll  tÔ À  ÑÉÖ¤òK[ÙM)JgÅSystem.Security.Cryptography.Algorithms.dll  ÞRÎÔ   ã177ßLÜG¥5	óñBSystem.Security.Cryptography.Cng.dll  B÷Úà   ó,"nW-CSVËâtSystem.Security.Cryptography.Csp.dll  _hÛ   `¥!kÿ FV²êtçåSystem.Security.Cryptography.dll  ]© @ pjl¹O©ºyºSystem.Security.Cryptography.Encoding.dll  ÕZÒ   ½Ë9¡BG²Õ¸½)¡±¦System.Security.Cryptography.OpenSsl.dll  ¸hÖã   y©þò¯ËD½}ëJUàµfSystem.Security.Cryptography.Primitives.dll  ·ÞÛ   v óë<EªäuLú¨System.Security.Cryptography.X509Certificates.dll  ÒrPâ   ñd¹ D¦p·N³âSystem.Security.Cryptography.Xml.dll  ©õ¯ à  y®+|'|B ·ÌlN¸áüSystem.Security.dll  êÒ   ðx.³ùEªÕ
ëÒSystem.Security.Principal.dll  «p°   a¶¬B«´¯Ä wêSystem.Security.Principal.Windows.dll  Cú
á    uBwy^Gè³
À'VSystem.Security.SecureString.dll  VÓ   £uÄÚôRF]N)à¶System.ServiceModel.Web.dll  î5Ì   2ÕÖp¦¶O¨[´±	XSystem.ServiceProcess.dll  ¿Ë5è   ¿N¼ÇËG¯T >ßSystem.Text.Encoding.CodePages.dll  L¢Í   J\¿¥:	æO<T1»RSystem.Text.Encoding.dll  ª   ¾y×ä6KöìÒ¦^72System.Text.Encoding.Extensions.dll  bp
ð   à3oO­Ã`{ÂµSystem.Text.Encodings.Web.dll  },    4×
% 'H§i|ESystem.Text.Json.dll  Æªç ` ö^xøÀAHF]9 s#System.Text.RegularExpressions.dll  ëcÐ À  Ò²_ê¬J¼ïqÎÜP1ôSystem.Threading.Channels.dll  ØàÉ   ü :áq±zJ6°Ð\óSystem.Threading.dll  a<ÚÊ À  ¤Â9àäB£OVú?System.Threading.Overlapped.dll  :W   ²6EVÞD?Kµ./s@System.Threading.RateLimiting.dll  ca§Ñ    °h
«OH¯GZæi¾5System.Threading.Tasks.Dataflow.dll  àÇ¤ À  éÏ~©ðGê@Î¿U/¶System.Threading.Tasks.dll  c   C.	UA]«àQ÷System.Threading.Tasks.Extensions.dll  YCN   °ªòTG¦¼¾\%u0System.Threading.Tasks.Parallel.dll  ]"Þ   J¹µJ¾äïE%=System.Threading.Thread.dll  ãnÞ    ø×f×¸áOÀ7ªSystem.Threading.ThreadPool.dll  =   k`Ä »ñFKß%ß2ÿSystem.Threading.Timer.dll  øA<   §ÍË&K>C´I_6à»RSystem.Transactions.dll  S{Tã   VÙ´ùþØN¯ó®ýipxJSystem.Transactions.Local.dll  ]ÙÅ    vRòW¦¾G³«\;ñÛSystem.ValueTuple.dll  ô¸§   Ö(ª²¨ÒHwkAÛîSystem.Web.dll  ^Æ¦   ×·OÛêÈHwfeáßSystem.Web.HttpUtility.dll  9¡   5þÌI«E¤æ¤ÊSystem.Windows.dll  ó{Ì   )gk²LCþ°{+ÐSystem.Xml.dll  ²YÉ±    ¬yìûÿ³BÀùõës§System.Xml.Linq.dll  ÙsM   ÐhP-þâuFx ä¯²System.Xml.ReaderWriter.dll  &×í   :ac$4OàÚ­L©System.Xml.Serialization.dll  EÌ   tI2þÚb1N¯u9]Sá1System.Xml.XDocument.dll  Gõ³ À  §ÿAì¡û@ÙT¸?í]System.Xml.XmlDocument.dll  ^â´   U@]ë¶KHz[]System.Xml.XmlSerializer.dll  0.ÿ   "×Yí5E¬geúSystem.Xml.XPath.dll  %`3   1¢'³%ÿ¥F]<äàæWSystem.Xml.XPath.XDocument.dll  Nâ®   ¨Èt½|:I¡¨p;Ñ:WindowsBase.dll  V`óÉ   Xñ VN¨]\ñË3  1 "  +  ! Microsoft.AspNetCore.BuilderMicrosoft.AspNetCore.HostingMicrosoft.AspNetCore.HttpMicrosoft.AspNetCore.Routing"Microsoft.Extensions.Configuration(Microsoft.Extensions.DependencyInjectionMicrosoft.Extensions.HostingMicrosoft.Extensions.LoggingSystemSystem.Collections.Generic	System.IOSystem.LinqSystem.Net.HttpSystem.Net.Http.JsonSystem.ThreadingSystem.Threading.TasksMicrosoft.AspNetCore.MvcUÀ SÞÀ SûÀ TÀ T2À TOÀ TrÀ TÀ T¸À TÕÀ TÜÀ T÷À UÀ U
À UÀ U2À UCÀ UZ(       %   #   (   !	$PÀ SÞÀ SûÀ TÀ T2À TOÀ TrÀ TÀ T¸À TÕÀ TÜÀ T÷À UÀ U
À UÀ U2À UC   "   '   K	    y  	3 y >  		 ';
```

---


## WebApplication2/bin/Debug/net8.0/WebApplication2.runtimeconfig.json

```json
{
  "runtimeOptions": {
    "tfm": "net8.0",
    "frameworks": [
      {
        "name": "Microsoft.NETCore.App",
        "version": "8.0.0"
      },
      {
        "name": "Microsoft.AspNetCore.App",
        "version": "8.0.0"
      }
    ],
    "configProperties": {
      "System.GC.Server": true,
      "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization": false
    }
  }
}
```

---


## WebApplication2/bin/Debug/net8.0/appsettings.Development.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

```

---


## WebApplication2/bin/Debug/net8.0/appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.AssemblyInfo.cs

```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Reflection;

[assembly: System.Reflection.AssemblyCompanyAttribute("WebApplication2")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0+7a9517801a8d1172fecc5542e1656680d8c238d3")]
[assembly: System.Reflection.AssemblyProductAttribute("WebApplication2")]
[assembly: System.Reflection.AssemblyTitleAttribute("WebApplication2")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]

// Создано классом WriteCodeFragment MSBuild.


```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.AssemblyInfoInputs.cache

```text
9e7b7ba6174c44142d63bf08b4018a22b16ea2568433acb7bb6f65b0980df97a

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.GeneratedMSBuildEditorConfig.editorconfig

```text
is_global = true
build_property.TargetFramework = net8.0
build_property.TargetPlatformMinVersion = 
build_property.UsingMicrosoftNETSdkWeb = true
build_property.ProjectTypeGuids = 
build_property.InvariantGlobalization = 
build_property.PlatformNeutralAssembly = 
build_property.EnforceExtendedAnalyzerRules = 
build_property._SupportedPlatformList = Linux,macOS,Windows
build_property.RootNamespace = WebApplication2
build_property.RootNamespace = WebApplication2
build_property.ProjectDir = D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\
build_property.EnableComHosting = 
build_property.EnableGeneratedComInterfaceComImportInterop = 
build_property.RazorLangVersion = 8.0
build_property.SupportLocalizedComponentNames = 
build_property.GenerateRazorMetadataSourceChecksumAttributes = 
build_property.MSBuildProjectDirectory = D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2
build_property._RazorSourceGeneratorDebug = 

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.GlobalUsings.g.cs

```csharp
// <auto-generated/>
global using global::Microsoft.AspNetCore.Builder;
global using global::Microsoft.AspNetCore.Hosting;
global using global::Microsoft.AspNetCore.Http;
global using global::Microsoft.AspNetCore.Routing;
global using global::Microsoft.Extensions.Configuration;
global using global::Microsoft.Extensions.DependencyInjection;
global using global::Microsoft.Extensions.Hosting;
global using global::Microsoft.Extensions.Logging;
global using global::System;
global using global::System.Collections.Generic;
global using global::System.IO;
global using global::System.Linq;
global using global::System.Net.Http;
global using global::System.Net.Http.Json;
global using global::System.Threading;
global using global::System.Threading.Tasks;

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.MvcApplicationPartsAssemblyInfo.cache

```text

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.MvcApplicationPartsAssemblyInfo.cs

```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Reflection;

[assembly: Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartAttribute("Swashbuckle.AspNetCore.SwaggerGen")]

// Создано классом WriteCodeFragment MSBuild.


```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.assets.cache

```text
PKGA   ¨G0eè1h(pÂ¶0åqlçÂ0»ÊBsÈ¬
             `C:\Users\zapev\.nuget\packages\microsoft.openapi\1.6.14\lib\netstandard2.0\Microsoft.OpenApi.dll                      qC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swagger\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.Swagger.dll                      wC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggergen\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerGen.dll       	            
   uC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggerui\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerUI.dll                             `C:\Users\zapev\.nuget\packages\microsoft.openapi\1.6.14\lib\netstandard2.0\Microsoft.OpenApi.pdb                qC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swagger\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.Swagger.pdb                wC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggergen\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerGen.pdb       	         uC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggerui\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerUI.pdb                       Microsoft.AspNetCore.App           *Microsoft.Extensions.ApiDescription.Server    Microsoft.OpenApi    Swashbuckle.AspNetCore    Swashbuckle.AspNetCore.Swagger    !Swashbuckle.AspNetCore.SwaggerGen     Swashbuckle.AspNetCore.SwaggerUI       Swashbuckle.AspNetCore/6.6.2   
                                 C:\Users\zapev\.nuget\packages\       `C:\Users\zapev\.nuget\packages\microsoft.openapi\1.6.14\lib\netstandard2.0\Microsoft.OpenApi.xml                qC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swagger\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.Swagger.xml                wC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggergen\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerGen.xml       	         uC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggerui\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerUI.xml                       `C:\Users\zapev\.nuget\packages\microsoft.openapi\1.6.14\lib\netstandard2.0\Microsoft.OpenApi.dll                                        qC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swagger\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.Swagger.dll                                        wC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggergen\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerGen.dll       	            
                     uC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggerui\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerUI.dll                                                       NuGetPackageIdMicrosoft.OpenApiNuGetPackageVersion1.6.14
PathInPackage(lib/netstandard2.0/Microsoft.OpenApi.dllSwashbuckle.AspNetCore.Swagger6.6.2-lib/net8.0/Swashbuckle.AspNetCore.Swagger.dll!Swashbuckle.AspNetCore.SwaggerGen0lib/net8.0/Swashbuckle.AspNetCore.SwaggerGen.dll Swashbuckle.AspNetCore.SwaggerUI/lib/net8.0/Swashbuckle.AspNetCore.SwaggerUI.dllNameSwashbuckle.AspNetCoreVersionIsImplicitlyDefinedFalseResolvedTruePath;C:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore\6.6.2	AssetTyperuntime	CopyLocaltrueDestinationSubPathMicrosoft.OpenApi.dll"Swashbuckle.AspNetCore.Swagger.dll%Swashbuckle.AspNetCore.SwaggerGen.dll$Swashbuckle.AspNetCore.SwaggerUI.dll
```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.csproj.AssemblyReference.cache

```text
   `C:\Users\zapev\.nuget\packages\microsoft.openapi\1.6.14\lib\netstandard2.0\Microsoft.OpenApi.dll    ìö=ÜMicrosoft.OpenApi                           hfile:///C:/Users/zapev/.nuget/packages/microsoft.openapi/1.6.14/lib/netstandard2.0/Microsoft.OpenApi.dll     $          $  RSA1     |´²¥õO\ãUñ&Ó*9
²|ô7¯Æ¼bu©¸¢¿¶uÔã=ìµZ@1Ü²§gâwØÎJ
¿Á³¾»ßâñÿÀ¨_Ödú¯ú£ªÒåE-¿x~?Ó+V¬©]ñ£Äç]ìJ?Le=ÿÃ³Ä   ?WCcvðBUMicrosoft.OpenApi, Version=1.6.14.0, Culture=neutral, PublicKeyToken=3f5743946376f042      
v4.0.30319 qC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swagger\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.Swagger.dll    á\
µyÜSwashbuckle.AspNetCore.Swagger                           yfile:///C:/Users/zapev/.nuget/packages/swashbuckle.aspnetcore.swagger/6.6.2/lib/net8.0/Swashbuckle.AspNetCore.Swagger.dll     $          $  RSA1     ÐðCzÄö×}ü)ÎTß@öctËq"ì]Ú³zmþ`ÿÔ¦sãøÞM§Þ§>ïüKì?FÃnxõBÄ¾u^ýýlÊ¹ðÛ4U¸4!fÐàAÃÄìí2Ö_ÌP]Å7×ÐÌ*W-$øEFÅ   be}ttuaSwashbuckle.AspNetCore.Swagger, Version=6.6.2.0, Culture=neutral, PublicKeyToken=62657d7474907593      
v4.0.30319 wC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggergen\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerGen.dll    !µyÜ!Swashbuckle.AspNetCore.SwaggerGen                           file:///C:/Users/zapev/.nuget/packages/swashbuckle.aspnetcore.swaggergen/6.6.2/lib/net8.0/Swashbuckle.AspNetCore.SwaggerGen.dll     $          $  RSA1     ¸C¶\Öy£ñoÍ¹+ÿY\Ì=ÀçÑ)dÃ³Ú5lØ¢
eåÎK¦/Öþï@Bo[é8ÞÌ}DªÇUCÏÙ©:í*æ*³YKÉÕ²óWÏ¼ð¥Ú
pRZ£©qì|ä
ÑTrõýèùæ   ØMû5S
dSwashbuckle.AspNetCore.SwaggerGen, Version=6.6.2.0, Culture=neutral, PublicKeyToken=d84d99fb0135530a      
v4.0.30319 uC:\Users\zapev\.nuget\packages\swashbuckle.aspnetcore.swaggerui\6.6.2\lib\net8.0\Swashbuckle.AspNetCore.SwaggerUI.dll    á\
µyÜ Swashbuckle.AspNetCore.SwaggerUI                           }file:///C:/Users/zapev/.nuget/packages/swashbuckle.aspnetcore.swaggerui/6.6.2/lib/net8.0/Swashbuckle.AspNetCore.SwaggerUI.dll     $          $  RSA1     éíÖ8ÒB °x÷q$,b?ÄÌ6pôSÒ¶lXdv·lj1öÑÞl³ú'¿FÞá[|¤Ì |ëÀtR¼>ãûûªÞN(§êìúà	Å+W%d:!ÁUBÝ¤sAkkêë2h
=Ô-1%,
BÍîM5Ð   B2É'³ÂTcSwashbuckle.AspNetCore.SwaggerUI, Version=6.6.2.0, Culture=neutral, PublicKeyToken=4232c99127b3c254      
v4.0.30319 
```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.csproj.CopyComplete

```text

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.csproj.CoreCompileInputs.cache

```text
9b138a3cab9b9861d5114b1b83f200de2fdfb4c8d983a060e7b560fd9c58abc2

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.csproj.FileListAbsolute.txt

```text
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\appsettings.Development.json
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\appsettings.json
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\WebApplication2.exe
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\WebApplication2.deps.json
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\WebApplication2.runtimeconfig.json
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\WebApplication2.dll
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\WebApplication2.pdb
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\Microsoft.OpenApi.dll
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\Swashbuckle.AspNetCore.Swagger.dll
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\Swashbuckle.AspNetCore.SwaggerGen.dll
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\bin\Debug\net8.0\Swashbuckle.AspNetCore.SwaggerUI.dll
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.csproj.AssemblyReference.cache
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.GeneratedMSBuildEditorConfig.editorconfig
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.AssemblyInfoInputs.cache
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.AssemblyInfo.cs
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.csproj.CoreCompileInputs.cache
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.MvcApplicationPartsAssemblyInfo.cs
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.MvcApplicationPartsAssemblyInfo.cache
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.sourcelink.json
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\staticwebassets.build.json
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\staticwebassets.development.json
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\staticwebassets\msbuild.WebApplication2.Microsoft.AspNetCore.StaticWebAssets.props
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\staticwebassets\msbuild.build.WebApplication2.props
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\staticwebassets\msbuild.buildMultiTargeting.WebApplication2.props
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\staticwebassets\msbuild.buildTransitive.WebApplication2.props
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\staticwebassets.pack.json
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\scopedcss\bundle\WebApplication2.styles.css
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.csproj.CopyComplete
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.dll
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\refint\WebApplication2.dll
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.pdb
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\WebApplication2.genruntimeconfig.cache
D:\Study\3_Course\2_Semester\College-projects_3.2\ASP-ADO.NET\WebApplication2\WebApplication2\obj\Debug\net8.0\ref\WebApplication2.dll

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.genruntimeconfig.cache

```text
82423863819517090cb15ce30b731f51027ad0b032d2622841385a5be2723157

```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.pdb

```text
BSJB         PDB v1.0       |   d   #Pdb    à   Ä  #~  ¤     #Strings    °     #US ´     #GUID   4  V  #Blob   ô5 j@ÍÅ3èg¾½(è¨  W¢		
     (               $   $                     	                     ¯                       
   x    °  ¼  ð  ü  P b    ° ï   ÈS     ÌU ÓU ÚU áU èU @V GV     NV dV ~V         V        -                                                                          	                               6   
        ^           T             sU ïU            ÉU'  Â'  '  
  ¶  ²Ö  Ñö  " uV xV   builder        Ð)¸Bw¬øbQ?ÆÓS ÀO£¡W&inF´­°FõþVÌ 8Mì%«5jìþµÐJÚFb»KØGM~n	\L®ÚËºjt
¨R_uÅ¾E´¸ qåR½L C¦@oI§0ÖOIyÞ D:Study3_Course
2_SemesterCollege-projects_3.2ASP-ADO.NETWebApplication2ControllersWeatherForecastController.cs\
3??O[ Ï0Â¦®s2Ýóæ1Nßc,Eñµ@`,<p
Program.cs\
3??¥ ÈÆ³F ¹HqÚÏqÑj?6R¢Rñ&Á/YÉ'$WeatherForecast.cs\
3??Ý ¤ðQp*¤ 41J,¢Qó"Y\ø±1$objDebugnet8.0!WebApplication2.GlobalUsings.g.cs\
3??!'. Ái7Â%ô±þ§{¾ð@¢|Î*·í("o^5Ë  Û1ï|y;÷*eU<úu&v«5ôí­(ì"8c/C¾¤çYO¡¶¯Ê¼ßú2vÚe-ìQt:[xbÚpµ5Öà»)ÎX"æTIÂÂgC¾/ÈÕpoM1Ä2ÉCX×) Hö?ä3'cjÍNõÝøbçë²ú¹ÿ [4ÓU0·xn@âjú©¿óÉn=è²1?Nm59Ò7..NETCoreApp,Version=v8.0.AssemblyAttributes.cs\
3??!'P ÷¥Ùx6%á9ÿá:P80ö¸Zöß¬v´Ê    // <autogenerated />
using System;
using System.Reflection;
[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
WebApplication2.AssemblyInfo.cs\
3??!'~ ùV6ùL^Ð@,ºîãÉqìüÕÇÂËk·íi:é½  ­TIkÛ@¾òZJ´yS
nJ BhJr=¥±Ðb¤ÑÁ7;.í¡)¾öÚPp¼P¥ÞþÂÔ7cºàgÄ¼yß2O1ÍÃý=Ó$§4ÉaÀbRÁü:¨ü·°·~ÁÆDvñóÆ09¥ìàrïÌp.àÁØ@¿aòcSË>Lp û0Ü*dÿ¸lXFÉ*ÙGFÙ±,KaáQh¸¹~
%ò«23"?¢èOVÉ²©,Z£ú&`©Mqo*ï0VÝ«ã)Ì½=¯yVatÚGû_´t±=ùAKuÂ¡.RgM£iE/?¡b±®VÕ95ÿRî]ÿÕý½<ãq@.Û`ÑÉÓ¥ñ5Cæ	Ä'*õfaûøÏ£¾Þ;K¢Ûu!RÞÈ{vpÍõV+äUÎÁó[SÅMä©Æ="|Íy°=Í9ÙK³§$66ÎíiÞÄÍ$´þïEUìkÙÔõm»æ4çU*eÙÕJµêZ¾ë9%×/m/{&~î]Ôó=!ÛÑÿ«©»¾o® X¨ÖbwõÍ0#×)ì,ñÙyJÅ¼½|óÐ7ü72WebApplication2.MvcApplicationPartsAssemblyInfo.cs\
3??!'¼ [`BæðWäü·Ây¸"ömP çÃ0#  ­RËNÂ@ÝðVº`Z@LDW#ÆE)Cm,-i§v<.Ô°uëðE^¿pç¼3Æ%Ó´Í¹ç1wFÓ;ñ¦r/i1ùgcÊ¢
Ñ&ð
ÑÂß sKÑÄé¿=á³	ÝPß±xØæ¢c\ï.E"ÑÍ¤©NSzêà¦u]Ü->Í7Wo¤¸D¼É@0#âM'(>&Êy(:2¦)F(¥
9Àµ©xE£õåö¤ÖöEFkI»ÿEY·ÛÏHoRÊTkY®BKyñÑº{Ð[u'§ýÓî]j<¶kR#à¬ý=¥¬ê0Û¥×F°ZÙidHÑ6}/ðªæú9ãÏg´xoÒ|½îØ¦!9Ï¿@sß.í%JFp[Í;m« lYÌ?cnbÿFÚb/àcs¡`!5Å®µÔ=+ßæ¬àUØ©oX5ærR,¶S¡þ¼{"documents":{"D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\*":"https://raw.githubusercontent.com/Mason-Zaccaro/College-projects_3.2/7a9517801a8d1172fecc5542e1656680d8c238d3/*"}}version 2 compiler-version 4.8.0-7.23572.1+7b75981cf3bd520b86ec4ed00ec156c8bc48e4eb language C# source-file-count 7 output-kind ConsoleApplication optimization debug-plus platform AnyCpu runtime-version 4.8.9310.0 language-version 12.0 nullable Enable define TRACE,DEBUG,NET,NET8_0,NETCOREAPP,NET5_0_OR_GREATER,NET6_0_OR_GREATER,NET7_0_OR_GREATER,NET8_0_OR_GREATER,NETCOREAPP1_0_OR_GREATER,NETCOREAPP1_1_OR_GREATER,NETCOREAPP2_0_OR_GREATER,NETCOREAPP2_1_OR_GREATER,NETCOREAPP2_2_OR_GREATER,NETCOREAPP3_0_OR_GREATER,NETCOREAPP3_1_OR_GREATER À I&Microsoft.AspNetCore.Antiforgery.dll  v© à  ô
àkÆ MÃ¬dé@Microsoft.AspNetCore.Authentication.Abstractions.dll  Ã¨² À  qË]?gA©éÅ@§ÆMicrosoft.AspNetCore.Authentication.BearerToken.dll  ®z-Ò À  ºow©«ÊD©`^3¢CMicrosoft.AspNetCore.Authentication.Cookies.dll  Dó¨ À  Î£@ »½AJºuq¯ïMicrosoft.AspNetCore.Authentication.Core.dll  õÆ{á À  )ñ¾p¥ûLÌ@ ¿IêMicrosoft.AspNetCore.Authentication.dll  ÕÌ   çajG®#@¿¬úÉî3¯Microsoft.AspNetCore.Authentication.OAuth.dll  Ö×Ê  À  8RåzB­ø<ü®Microsoft.AspNetCore.Authorization.dll  ;~Û à  ìÀ~·K¸§è´@"Â¯Microsoft.AspNetCore.Authorization.Policy.dll  AÓ×­ À  +²Dx Eðp¤¨Microsoft.AspNetCore.Components.Authorization.dll  a«ÞÆ À  søÀ8ªðFª8:<ÝòMicrosoft.AspNetCore.Components.dll  Ïh¡³ ` £ÂRí³@z½qûæ©Microsoft.AspNetCore.Components.Endpoints.dll  z¥Õ à µú3ª°¤0J¥]6¡Q*Microsoft.AspNetCore.Components.Forms.dll  ·*ú À  ^1;L
xG½á°-41Microsoft.AspNetCore.Components.Server.dll  8NU   Û!ºx~÷qH¹É{øÓMicrosoft.AspNetCore.Components.Web.dll  Ypo¥   ¥·	ÙQJÅG´ ø°ØMicrosoft.AspNetCore.Connections.Abstractions.dll  ýý³ à  ïF-¼â¼BªÓÃÁ/æMicrosoft.AspNetCore.CookiePolicy.dll  ÷o À  oñkKÖhOÄÚ|÷ëMicrosoft.AspNetCore.Cors.dll  vøi À  æzxAÄA1µÐklMicrosoft.AspNetCore.Cryptography.Internal.dll  ´UL   1Â±Ý/¼E¿?]½Q>Microsoft.AspNetCore.Cryptography.KeyDerivation.dll  a    ;5Í
eY ECÃû©z Microsoft.AspNetCore.DataProtection.Abstractions.dll  <    E J¶O¥ËáùÔ×ÂMicrosoft.AspNetCore.DataProtection.dll  Ý   ÌV©sçÑ Kýfõ2|qMicrosoft.AspNetCore.DataProtection.Extensions.dll  ùk]ê    »Ëé ÷¶L8¤ÙXMicrosoft.AspNetCore.Diagnostics.Abstractions.dll  ]Ùa    Voé?þ£ J"F·-A/Microsoft.AspNetCore.Diagnostics.dll  e8±ý   [:jUwÅD§sN|¼3ÈMicrosoft.AspNetCore.Diagnostics.HealthChecks.dll  æ~
á    § g@­	Cºók£Õ^ßMicrosoft.AspNetCore.dll  ²® à  Bµèhó@Ì
3¢¿§Microsoft.AspNetCore.HostFiltering.dll  «L¾    «»·Îr\F¦Ñ-4j{PMicrosoft.AspNetCore.Hosting.Abstractions.dll  dÕ^ø À  0ë4ô*LÕs÷P~Microsoft.AspNetCore.Hosting.dll  KYìÉ  =Ú3_ ¿B¡\iS¼Microsoft.AspNetCore.Hosting.Server.Abstractions.dll  ©<iÛ    æ×mH»j>JTMicrosoft.AspNetCore.Html.Abstractions.dll  R¹æ    ±·o*ÃãKèqäüªcÇMicrosoft.AspNetCore.Http.Abstractions.dll  Ô2ÿ®   Ð×Ç¿GÒ­OI$Z¥ÅLÂMicrosoft.AspNetCore.Http.Connections.Common.dll  û_¶Û    ØEZàC§N#¢© BMicrosoft.AspNetCore.Http.Connections.dll  K @ ?ªtZ),Iýçf7aªMicrosoft.AspNetCore.Http.dll  Ï¡Jõ ` ëDWîÿ[L·á*&{Microsoft.AspNetCore.Http.Extensions.dll  î¨¿   Ý­/[öDo5<;(²¨Microsoft.AspNetCore.Http.Features.dll  ¿zf à  ót\éLH2Cî7ÔyMicrosoft.AspNetCore.Http.Results.dll  L`ñ± @ êz®Iª¾]H»öÂR(üMicrosoft.AspNetCore.HttpLogging.dll  }»[ý   ÷ 3I¨©ù2Microsoft.AspNetCore.HttpOverrides.dll  Sd@ À  ©8ó
 CÄ2äzËåQMicrosoft.AspNetCore.HttpsPolicy.dll  þlÑª    _9Üö3B®/Îó¥HMicrosoft.AspNetCore.Identity.dll  d²ù À JöÝ³iU´H©Ôm¹ÀÕMicrosoft.AspNetCore.Localization.dll  *y,Â À  BÉ8ÜáqI¥HÙ±µMicrosoft.AspNetCore.Localization.Routing.dll  rü    
êxh£áCW/mâMicrosoft.AspNetCore.Metadata.dll  ©³ú    YÊä
MqD_È6þÿMicrosoft.AspNetCore.Mvc.Abstractions.dll  /_û   §ÏN9G¦4KõU[Microsoft.AspNetCore.Mvc.ApiExplorer.dll  y)Á À  ¨1ÅTBô»Ã]1²IMicrosoft.AspNetCore.Mvc.Core.dll  åg÷ú   ¨æ5=ÀH´Ó[b&ÛAMicrosoft.AspNetCore.Mvc.Cors.dll  ¶øpé    ææ¼>CKC´ZÔcã{tMicrosoft.AspNetCore.Mvc.DataAnnotations.dll  ¶h~Ë À  ã¼¦ gÑîD(ì µMicrosoft.AspNetCore.Mvc.dll  Ð    IgàGYiH¥å6È3­þMicrosoft.AspNetCore.Mvc.Formatters.Json.dll  íaÊ    6bãíª@4G/ºr1¹YMicrosoft.AspNetCore.Mvc.Formatters.Xml.dll  Tnì À  bµíÐeI¡Fç|½BºMicrosoft.AspNetCore.Mvc.Localization.dll  ò aã À  O`91ÃM½µ)>QKMicrosoft.AspNetCore.Mvc.Razor.dll  Ï.Î @ íEQCã®wR½Microsoft.AspNetCore.Mvc.RazorPages.dll  Ï¼'¯ À NtÿªGëä¶ÆFúMicrosoft.AspNetCore.Mvc.TagHelpers.dll  TÒ @ à¯>0Fk@¶ 7¿Ý£,Microsoft.AspNetCore.Mvc.ViewFeatures.dll  g¦ @ E¹É¢	ôÝB³@Ùæ°Microsoft.AspNetCore.OutputCaching.dll  \ÊÄ   6V>z«L³iCPûMicrosoft.AspNetCore.RateLimiting.dll  Üm À  ì
¯½FKE¦þdTÏ7Microsoft.AspNetCore.Razor.dll  wQ}Ð À  Ñ½ÃrHRõdeù-ÃMicrosoft.AspNetCore.Razor.Runtime.dll  ¿ª:Ù À  á9÷Ç*B-øXU¬åMicrosoft.AspNetCore.RequestDecompression.dll  ±BÕ×    [J£NA¬Âÿ/ZHMicrosoft.AspNetCore.ResponseCaching.Abstractions.dll  Ç¹    S9×K»Ú-ÀªÅÍMicrosoft.AspNetCore.ResponseCaching.dll  0#ì à  {êtÀ³M0ó3ÏyMicrosoft.AspNetCore.ResponseCompression.dll  ôá À  +Tê+®ÐôIvæðOÎMicrosoft.AspNetCore.Rewrite.dll  Bô   Õ	Ï;BWFî¡7¢Microsoft.AspNetCore.Routing.Abstractions.dll  [¬ À  \ÐÌÆFD$?ÕåU¿Microsoft.AspNetCore.Routing.dll  Ùvò­ @ ¥+ñhØHO¯7;8~ôïMicrosoft.AspNetCore.Server.HttpSys.dll  c =¨ @ âW+QØ¢O¿P{¦µMicrosoft.AspNetCore.Server.IIS.dll  ö6â @ ÕÚwøHÅ¿5»`ÓMicrosoft.AspNetCore.Server.IISIntegration.dll  O¼Û À  ²SÓaKÕI>Microsoft.AspNetCore.Server.Kestrel.Core.dll  Êøæ  Ü¦¾E¸Eìò,uMicrosoft.AspNetCore.Server.Kestrel.dll  #'«    5kðä¥¹@Å©·û[0ºMicrosoft.AspNetCore.Server.Kestrel.Transport.NamedPipes.dll  <|ÄÈ à  iô~ÚHIQ;(Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.dll  Û×}ê   ª¦ðkg ì@¤
±PËQMLMicrosoft.AspNetCore.Server.Kestrel.Transport.Sockets.dll  n=¹Õ   ~æ0>Ó Bhò¹ó¶Z¯Microsoft.AspNetCore.Session.dll  8º À  £m7ú°ôD¹nd2FfMicrosoft.AspNetCore.SignalR.Common.dll  2^½ À  Îb¦õL)8P{òé)Microsoft.AspNetCore.SignalR.Core.dll  OÝËñ   làbw¯ F®°íºw^¥!Microsoft.AspNetCore.SignalR.dll  r¹¼Î    1ü]íÂÜæF¢ ÿèXMicrosoft.AspNetCore.SignalR.Protocols.Json.dll  ÏÅ£    "ùïJÆ"Ë9[Microsoft.AspNetCore.StaticFiles.dll  Ùi¿ à  LöÔF¤KÁ÷IøëMicrosoft.AspNetCore.WebSockets.dll  ^Yjç À  ¥m_ü/Ö®Kªm£d0îMicrosoft.AspNetCore.WebUtilities.dll  ?­   {ò¤!CðÎÓtMicrosoft.CSharp.dll  5¹÷Ó   ²¶oÂD°eÂvª¨ý!Microsoft.Extensions.Caching.Abstractions.dll  ¸rå»    N*[¬B³f3*½Microsoft.Extensions.Caching.Memory.dll  ?ñó°   ý÷ôêºJ´ÒS´´üMicrosoft.Extensions.Configuration.Abstractions.dll  e­í   Å8ÜcÉO÷N¼:Microsoft.Extensions.Configuration.Binder.dll  .q   7ÂÎWÑ?J(iÛW,Microsoft.Extensions.Configuration.CommandLine.dll  W%l   Ë21LO^,ïôçsMicrosoft.Extensions.Configuration.dll  fKÄ³    "o¤`D´£×ùöMicrosoft.Extensions.Configuration.EnvironmentVariables.dll  )¤ï   ÜmâÛÌkJôI E¢Microsoft.Extensions.Configuration.FileExtensions.dll  Í|Ë   §jÒ¥É½BmdØð!Microsoft.Extensions.Configuration.Ini.dll  .°~Ü   À®!BöG^7áüsWMicrosoft.Extensions.Configuration.Json.dll  0Zx¤   xüù(Q]éB¸WöUçÜMicrosoft.Extensions.Configuration.KeyPerFile.dll  [£ö    ªýö,B4?¬
,Microsoft.Extensions.Configuration.UserSecrets.dll  _©à   æß\,5c|Cª ^±æMicrosoft.Extensions.Configuration.Xml.dll  Ö»=Ú   J ÈêIUH¹ýâtè¹sMicrosoft.Extensions.DependencyInjection.Abstractions.dll  zô    è³ÿùt)E¡Õ*~²üMicrosoft.Extensions.DependencyInjection.dll  Sf¶   ¥ê±p«)J³ëqMæîQMicrosoft.Extensions.Diagnostics.Abstractions.dll  HØù   pÆÑ
MzÕ/?Â&Microsoft.Extensions.Diagnostics.dll  ÌÙ   ^G{^<ÿJ¤ã@öY0`ÝMicrosoft.Extensions.Diagnostics.HealthChecks.Abstractions.dll  ,kÙ    SX¬Þ¶µ@­#/ÀáªMicrosoft.Extensions.Diagnostics.HealthChecks.dll  ãE×ÿ à  
Hë¯ÆñA¨á[øYMicrosoft.Extensions.Features.dll  k>Ô     h`DÁé[Ø)Microsoft.Extensions.FileProviders.Abstractions.dll  	|   mhJ0ÝN|\­âMË»Microsoft.Extensions.FileProviders.Composite.dll  h8ÆÐ    .5ªA§áfÃHÛåMicrosoft.Extensions.FileProviders.Embedded.dll  	²» À  TÕw³3H¾¸8ì8ÂMicrosoft.Extensions.FileProviders.Physical.dll  ·r|ð   ëòÐYF®PMicrosoft.Extensions.FileSystemGlobbing.dll  ã!    U3W¿hõG¼4I2_¢Microsoft.Extensions.Hosting.Abstractions.dll  ê¤¬î    ÔÞ´§xCÛ£Ç"lMicrosoft.Extensions.Hosting.dll  Ý{~    Çr$¨ºF¯hîD?ÛMicrosoft.Extensions.Http.dll  T~Ú    4t²î*¨kMÃ³ûrMicrosoft.Extensions.Identity.Core.dll  nD¥ã  hl¬K)8µB¶ËGö.XMicrosoft.Extensions.Identity.Stores.dll  Ò¹°ã à  gS¯C¤èJí©wY-ÐMicrosoft.Extensions.Localization.Abstractions.dll  Ú
    [g¾Ð}O¬«þº<ÅªMicrosoft.Extensions.Localization.dll  Ë À  H}²PÉÓEÊK%¤TMicrosoft.Extensions.Logging.Abstractions.dll  ´ô¥    føä!ö<¾M!øÕ+Ý©Microsoft.Extensions.Logging.Configuration.dll  ýE»Ü   Ëå¾Ù®F»akâèØMicrosoft.Extensions.Logging.Console.dll  {°Ó    TäÊ¡wGÊ|:øMicrosoft.Extensions.Logging.Debug.dll  V'u£   
Æ_´+kMºÙýý»Î Microsoft.Extensions.Logging.dll  _Iê   2Üì<b@½¦/äMicrosoft.Extensions.Logging.EventLog.dll   åÆü   /öÞ`4N
i¥'ÄFkMicrosoft.Extensions.Logging.EventSource.dll  Å¼   ¨$²foFêZ>Microsoft.Extensions.Logging.TraceSource.dll  ½úWÌ   ^ ´÷®^8M¹Es§ïú
Microsoft.Extensions.ObjectPool.dll  è¥    q
<£XbNß[¯]PMicrosoft.Extensions.Options.ConfigurationExtensions.dll  )é   <bU£1I¾°òMicrosoft.Extensions.Options.DataAnnotations.dll  Çpâ½   ~Ã Vè@µ%î­>Microsoft.Extensions.Options.dll  h5 À  tã-WJ¯Æ¯Ô7èHMicrosoft.Extensions.Primitives.dll  µ&z    ï2¶!{E:â4"ÆMicrosoft.Extensions.WebEncoders.dll  wæöô    íB&*C©`o¡KÄMicrosoft.JSInterop.dll  ÒcF à  þÜ¾8ËBH¿äÕYÖyMicrosoft.Net.Http.Headers.dll  õ¢ à  +'"6O¤ø4µÛªÇMicrosoft.OpenApi.dll  gÜÌ À `Í«x¤ÃO®f´¡Microsoft.VisualBasic.Core.dll  ¼\¥Ö   <áÙVJ¾TÆ_Microsoft.VisualBasic.dll  jG¯ò   ¨\?uçFKpºymMicrosoft.Win32.Primitives.dll  »|   xÐ¬ËTK«.qÐï?¾CMicrosoft.Win32.Registry.dll  :Õ]©    |(M'4BÅ®{ñ\v'mscorlib.dll  O8´   .S	bC=\Ë¯netstandard.dll  úªôã À 7´©ÃþPBAoÔ¸+eïSwashbuckle.AspNetCore.Swagger.dll  ÿ¨    f4-QO×ÜD	iaSwashbuckle.AspNetCore.SwaggerGen.dll  bwÚÌ @ ±ÞàÆ"hI§	!éKùSwashbuckle.AspNetCore.SwaggerUI.dll  Â¨<È  # <@ëÁ09;D(ø(1Ïõ)System.AppContext.dll  ü£u    ×ý 1$éJ¯Ek	System.Buffers.dll  ¥Ã6¼   +«2LR¼ÝSystem.Collections.Concurrent.dll  H_©î    ê­|XÖ}ÈOQ«¶è¤ôSystem.Collections.dll   7ü©   ÂJäcáI÷4ÁC cQSystem.Collections.Immutable.dll   d¤ ` ªô0ÿ­b¼H¿9iw°CSystem.Collections.NonGeneric.dll  3*º    Ð)g§\qN¦®Æ3<RSystem.Collections.Specialized.dll  ©ÝÂ    !£ÏL÷WC^v«]ÌSystem.ComponentModel.Annotations.dll  ,ZE¬ À  è>>I¸ÎtIô%System.ComponentModel.DataAnnotations.dll  '¿þæ   §ëùbÅSI¶NtIÒ[qSystem.ComponentModel.dll  ×d¢   ò@uªY@L¢A4jÒkè System.ComponentModel.EventBasedAsync.dll  àÍß   ²"+>òL¨JâÈ?ëDìSystem.ComponentModel.Primitives.dll  Ç}9    · rNÂAµÐDYsùSystem.ComponentModel.TypeConverter.dll  ×UÌ à ïåçFL©.íñF}
ïSystem.Configuration.dll  4S    ûÚ44N´9Ko^<System.Console.dll  §äÅ    µNùÎ3üL­&=ÒSystem.Core.dll  íAÕ    hÛ%®W^¡N4Ñ	Úh/System.Data.Common.dll  °Kéò   ¡ôü	RpC¤ Ðs¦½(System.Data.DataSetExtensions.dll  ×Ã
÷   ý[§·7ÎAÝwÏ>)System.Data.dll  ¿	ªã    ¢\úOO~)¢Ò^åSystem.Diagnostics.Contracts.dll  	Ã   %9$cæF&MÁ®{System.Diagnostics.Debug.dll  èÔù   üðBfþö@ºü¯{VÎ;System.Diagnostics.DiagnosticSource.dll  bì- à  +ârÔêD{@B
íSystem.Diagnostics.EventLog.dll  ïñ5Ý À  «0²ÏøíKëjå» {System.Diagnostics.FileVersionInfo.dll  »
¯   +è'7M.mM³Ïûá_þSystem.Diagnostics.Process.dll  \Üå À  rh7=C£SK{yEª`System.Diagnostics.StackTrace.dll  8]tà    B¬¥Ãà"D¬UaëàjSystem.Diagnostics.TextWriterTraceListener.dll  »mHí   ·Á¶>áö
@{Ñ$·<õSystem.Diagnostics.Tools.dll  t¯¥   ÿ1¹l¾GµN©#2^System.Diagnostics.TraceSource.dll  ¤
@ì    ¢±"ñQ%Jªq¤æU´"System.Diagnostics.Tracing.dll  1á¢    Ñ~¥îKH©K¹ÞÇ¹;qõ8System.dll  lÓ   T=ìÛ)H¿<Æ4¶3ÀSystem.Drawing.dll  ²°ê   sÉÂPH¢)UVàÙSystem.Drawing.Primitives.dll  zX¡ À  åÔyT%xIF§BnÒ>System.Dynamic.Runtime.dll  ­À   À§K@5êDÌØæ&íSystem.Formats.Asn1.dll  ÔÀ    ¡Éûµ7Ñ;E©ú 4¥System.Formats.Tar.dll  ,ªÒê   ëõ¤4ìLr9ËzSystem.Globalization.Calendars.dll  Qò   @ãKÔÝJ£1Wý>ÆïSystem.Globalization.dll  °¯ú   jD3ßoñB 9i´BEîSystem.Globalization.Extensions.dll  béª   ëZ ôpqGùF3(wÆSystem.IO.Compression.Brotli.dll  äÛ   UØ²ó-[Oß³#Üfô4System.IO.Compression.dll  
Rù   »ðá°'¿F¸?
~Â-System.IO.Compression.FileSystem.dll  Â   ÷Ç½oÁúNºRæír7System.IO.Compression.ZipFile.dll  
s   ?M ¦æEÝ\©ËåzaSystem.IO.dll  ìæ¢   Y÷¬H¼¿·¡PþSystem.IO.FileSystem.AccessControl.dll  
 ÷¡   zm¡Ù×©¤@¿^XÐSystem.IO.FileSystem.dll  È¸¡   ©q¦Mÿ«ù^Á
TSystem.IO.FileSystem.DriveInfo.dll  T»ñÇ   ÈV¿MX;Fº`6 NÙovSystem.IO.FileSystem.Primitives.dll  þÿåÝ   zYQ	E·
F¾8±System.IO.FileSystem.Watcher.dll  ÃNh   ¿hÚÌ8FA¥;øý#wìÆSystem.IO.IsolatedStorage.dll  ý@°    ;j\«¯Sv@¯of2»ðSystem.IO.MemoryMappedFiles.dll  ÷zï   Û&o*ÂM²¹<¶ãJSystem.IO.Pipelines.dll  >`   |òÈÂÈ»I¶U.ùc¸¬System.IO.Pipes.AccessControl.dll  §ÚE¡   ¢óÒÆG ­M¤6çGhI~9System.IO.Pipes.dll  Íw§    WÐ\ôkG¿ßoöÈ ®System.IO.UnmanagedMemoryStream.dll  ÁKSî   ¨qpªeFÜÃÆzvwSystem.Linq.dll  Çs À  EÉ89jI«eáÕù7System.Linq.Expressions.dll  Dýz @ 
-FºäI{¹5¸¤ÓþSystem.Linq.Parallel.dll  "´Ø­ À  ®wzìÑ½A;Ò¾System.Linq.Queryable.dll  ñG¡ À  ë~»[cÎFH|¡K§(System.Memory.dll  i«@Ô   ò?Øjî@¸@¢æq=System.Net.dll   ùKÛ   c¤2·§K¨#Ú[Á|õxSystem.Net.Http.dll  Z!w   ì¢|è2M¸Nãòågë6System.Net.Http.Json.dll  ¦ÈÁ    àÍ'´«Ù=Eçsá8aSystem.Net.HttpListener.dll  pê    Ï×kòÿ·ºDrE[0½System.Net.Mail.dll  $%« À  ÚáþûrF¦M3_ÕÐBErSystem.Net.NameResolution.dll  qbº¤   ø#ûÕJµ6USystem.Net.NetworkInformation.dll  HYüÏ À  |uµ,kFæÃX¨ÇÂSystem.Net.Ping.dll  %¾Ä   ÆTf×êlE+HÛªRvcSystem.Net.Primitives.dll  y¨ÿ À  `KÑC5OYWs*·îSystem.Net.Quic.dll  Ulb    IÛÀÖV|HMzY&ô+System.Net.Requests.dll  çë§ À  @u.ØG¥òxhN*
 System.Net.Security.dll   Êý±   <6*K¤HN>:v(System.Net.ServicePoint.dll  n#ß   ÀúÂðñI_ô-äÊSystem.Net.Sockets.dll  L   !<³òóL]¯&<QûSystem.Net.WebClient.dll  CWÌ    wµºTÙÑF`fÏ?(iSystem.Net.WebHeaderCollection.dll  x:   îî{i H¨¡¹!ü¨System.Net.WebProxy.dll  & »   7~æED¶µziL|bSystem.Net.WebSockets.Client.dll  Y¥úÿ   |7)L&7cRþ³System.Net.WebSockets.dll  6B,    l¢ q`/£G$¶÷QgZSystem.Numerics.dll  Ì·ÔÜ   'æ£T|A¯,x##}ÿSystem.Numerics.Vectors.dll  fÿ à  Ï´p,QQH¶4ì*ë#èSystem.ObjectModel.dll  )¡ëË    µ_nÄY$EÖúM-
ÈSystem.Reflection.DispatchProxy.dll  a¥   ¾7
·OKK»õêoÀcVSystem.Reflection.dll  ,)¢   ×íÈ*¥eF®-ålK	System.Reflection.Emit.dll  ÅH0¬ à  è2eíáLL¾ìõ o[System.Reflection.Emit.ILGeneration.dll  Ýÿ´   S­M+ÐK;­â±-System.Reflection.Emit.Lightweight.dll  Eã   Lçwsbä©Cµµã
rM,System.Reflection.Extensions.dll  ÓÀê   £Ø\°QëyM³3õ LÐÒSystem.Reflection.Metadata.dll  qÿ   >Ò"¾:±LBîÁSystem.Reflection.Primitives.dll  ð(×í    GZÕÃÂkJk~GTSystem.Reflection.TypeExtensions.dll  éÆ   ­?éX>~$G·uÂd"ÚSystem.Resources.Reader.dll  HÐ¤   ¿Df¦¢H·[ëeÚSystem.Resources.ResourceManager.dll  Õs   iîÇ"BÛ©æ54System.Resources.Writer.dll  !wì   ÆÝ^÷pZIÙmvM÷eSystem.Runtime.CompilerServices.Unsafe.dll  Õf
Ñ   rC(óE.5ªÅìSystem.Runtime.CompilerServices.VisualC.dll  kz/Û   {4ç1 IyðSystem.Runtime.dll  JÙ¿ à è®?øJ¯+
|EQêSystem.Runtime.Extensions.dll  »£©   ¶bäìLº23CâSystem.Runtime.Handles.dll  Ôì+    ^ò¤Z0á @·&ªûl^System.Runtime.InteropServices.dll  ó5÷Û À 	K·ÌH-CdbHºÑ0System.Runtime.InteropServices.JavaScript.dll  ù[î    jJÄ¨_N+G¦n{0CSystem.Runtime.InteropServices.RuntimeInformation.dll  |£   g÷lL½Eq¤³System.Runtime.Intrinsics.dll  ìÿãá  bí/ÉÂè@ÓðÏ­â9System.Runtime.Loader.dll  Àë   ²âà<ö§@¤CÂÈ=É´System.Runtime.Numerics.dll  ]zÅ§ À  »¿H°EMï}nÉO3²System.Runtime.Serialization.dll  'Èê   (0°A¦]dáÈSystem.Runtime.Serialization.Formatters.dll  @ï    ¦Ð´¼ÒK¬*ÜØ1ëÌSystem.Runtime.Serialization.Json.dll  kÝÃ   ×¢ÂãÝAã: Ë5GSystem.Runtime.Serialization.Primitives.dll  Ü]ï   °áúO³ÝµCÂÍSystem.Runtime.Serialization.Xml.dll  2õ à  ÏuÚ7HgÒ%Y3System.Security.AccessControl.dll  q&Ì À  ·¤û å)¶K®,·whÆçSystem.Security.Claims.dll  tÔ À  ÑÉÖ¤òK[ÙM)JgÅSystem.Security.Cryptography.Algorithms.dll  ÞRÎÔ   ã177ßLÜG¥5	óñBSystem.Security.Cryptography.Cng.dll  B÷Úà   ó,"nW-CSVËâtSystem.Security.Cryptography.Csp.dll  _hÛ   `¥!kÿ FV²êtçåSystem.Security.Cryptography.dll  ]© @ pjl¹O©ºyºSystem.Security.Cryptography.Encoding.dll  ÕZÒ   ½Ë9¡BG²Õ¸½)¡±¦System.Security.Cryptography.OpenSsl.dll  ¸hÖã   y©þò¯ËD½}ëJUàµfSystem.Security.Cryptography.Primitives.dll  ·ÞÛ   v óë<EªäuLú¨System.Security.Cryptography.X509Certificates.dll  ÒrPâ   ñd¹ D¦p·N³âSystem.Security.Cryptography.Xml.dll  ©õ¯ à  y®+|'|B ·ÌlN¸áüSystem.Security.dll  êÒ   ðx.³ùEªÕ
ëÒSystem.Security.Principal.dll  «p°   a¶¬B«´¯Ä wêSystem.Security.Principal.Windows.dll  Cú
á    uBwy^Gè³
À'VSystem.Security.SecureString.dll  VÓ   £uÄÚôRF]N)à¶System.ServiceModel.Web.dll  î5Ì   2ÕÖp¦¶O¨[´±	XSystem.ServiceProcess.dll  ¿Ë5è   ¿N¼ÇËG¯T >ßSystem.Text.Encoding.CodePages.dll  L¢Í   J\¿¥:	æO<T1»RSystem.Text.Encoding.dll  ª   ¾y×ä6KöìÒ¦^72System.Text.Encoding.Extensions.dll  bp
ð   à3oO­Ã`{ÂµSystem.Text.Encodings.Web.dll  },    4×
% 'H§i|ESystem.Text.Json.dll  Æªç ` ö^xøÀAHF]9 s#System.Text.RegularExpressions.dll  ëcÐ À  Ò²_ê¬J¼ïqÎÜP1ôSystem.Threading.Channels.dll  ØàÉ   ü :áq±zJ6°Ð\óSystem.Threading.dll  a<ÚÊ À  ¤Â9àäB£OVú?System.Threading.Overlapped.dll  :W   ²6EVÞD?Kµ./s@System.Threading.RateLimiting.dll  ca§Ñ    °h
«OH¯GZæi¾5System.Threading.Tasks.Dataflow.dll  àÇ¤ À  éÏ~©ðGê@Î¿U/¶System.Threading.Tasks.dll  c   C.	UA]«àQ÷System.Threading.Tasks.Extensions.dll  YCN   °ªòTG¦¼¾\%u0System.Threading.Tasks.Parallel.dll  ]"Þ   J¹µJ¾äïE%=System.Threading.Thread.dll  ãnÞ    ø×f×¸áOÀ7ªSystem.Threading.ThreadPool.dll  =   k`Ä »ñFKß%ß2ÿSystem.Threading.Timer.dll  øA<   §ÍË&K>C´I_6à»RSystem.Transactions.dll  S{Tã   VÙ´ùþØN¯ó®ýipxJSystem.Transactions.Local.dll  ]ÙÅ    vRòW¦¾G³«\;ñÛSystem.ValueTuple.dll  ô¸§   Ö(ª²¨ÒHwkAÛîSystem.Web.dll  ^Æ¦   ×·OÛêÈHwfeáßSystem.Web.HttpUtility.dll  9¡   5þÌI«E¤æ¤ÊSystem.Windows.dll  ó{Ì   )gk²LCþ°{+ÐSystem.Xml.dll  ²YÉ±    ¬yìûÿ³BÀùõës§System.Xml.Linq.dll  ÙsM   ÐhP-þâuFx ä¯²System.Xml.ReaderWriter.dll  &×í   :ac$4OàÚ­L©System.Xml.Serialization.dll  EÌ   tI2þÚb1N¯u9]Sá1System.Xml.XDocument.dll  Gõ³ À  §ÿAì¡û@ÙT¸?í]System.Xml.XmlDocument.dll  ^â´   U@]ë¶KHz[]System.Xml.XmlSerializer.dll  0.ÿ   "×Yí5E¬geúSystem.Xml.XPath.dll  %`3   1¢'³%ÿ¥F]<äàæWSystem.Xml.XPath.XDocument.dll  Nâ®   ¨Èt½|:I¡¨p;Ñ:WindowsBase.dll  V`óÉ   Xñ VN¨]\ñË3  1 "  +  ! Microsoft.AspNetCore.BuilderMicrosoft.AspNetCore.HostingMicrosoft.AspNetCore.HttpMicrosoft.AspNetCore.Routing"Microsoft.Extensions.Configuration(Microsoft.Extensions.DependencyInjectionMicrosoft.Extensions.HostingMicrosoft.Extensions.LoggingSystemSystem.Collections.Generic	System.IOSystem.LinqSystem.Net.HttpSystem.Net.Http.JsonSystem.ThreadingSystem.Threading.TasksMicrosoft.AspNetCore.MvcUÀ SÞÀ SûÀ TÀ T2À TOÀ TrÀ TÀ T¸À TÕÀ TÜÀ T÷À UÀ U
À UÀ U2À UCÀ UZ(       %   #   (   !	$PÀ SÞÀ SûÀ TÀ T2À TOÀ TrÀ TÀ T¸À TÕÀ TÜÀ T÷À UÀ U
À UÀ U2À UC   "   '   K	    y  	3 y >  		 ';
```

---


## WebApplication2/obj/Debug/net8.0/WebApplication2.sourcelink.json

```json
{"documents":{"D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\*":"https://raw.githubusercontent.com/Mason-Zaccaro/College-projects_3.2/7a9517801a8d1172fecc5542e1656680d8c238d3/*"}}
```

---


## WebApplication2/obj/Debug/net8.0/staticwebassets/msbuild.build.WebApplication2.props

```text
﻿<Project>
  <Import Project="Microsoft.AspNetCore.StaticWebAssets.props" />
</Project>
```

---


## WebApplication2/obj/Debug/net8.0/staticwebassets/msbuild.buildMultiTargeting.WebApplication2.props

```text
﻿<Project>
  <Import Project="..\build\WebApplication2.props" />
</Project>
```

---


## WebApplication2/obj/Debug/net8.0/staticwebassets/msbuild.buildTransitive.WebApplication2.props

```text
﻿<Project>
  <Import Project="..\buildMultiTargeting\WebApplication2.props" />
</Project>
```

---


## WebApplication2/obj/Debug/net8.0/staticwebassets.build.json

```json
{
  "Version": 1,
  "Hash": "b1mWX6+bt/u1KlQWUIKtoOpbq6zU7qCxk7LR53mklEQ=",
  "Source": "WebApplication2",
  "BasePath": "_content/WebApplication2",
  "Mode": "Default",
  "ManifestType": "Build",
  "ReferencedProjectsConfiguration": [],
  "DiscoveryPatterns": [],
  "Assets": []
}
```

---


## WebApplication2/obj/WebApplication2.csproj.nuget.dgspec.json

```json
{
  "format": 1,
  "restore": {
    "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\WebApplication2.csproj": {}
  },
  "projects": {
    "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\WebApplication2.csproj": {
      "version": "1.0.0",
      "restore": {
        "projectUniqueName": "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\WebApplication2.csproj",
        "projectName": "WebApplication2",
        "projectPath": "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\WebApplication2.csproj",
        "packagesPath": "C:\\Users\\zapev\\.nuget\\packages\\",
        "outputPath": "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\obj\\",
        "projectStyle": "PackageReference",
        "configFilePaths": [
          "C:\\Users\\zapev\\AppData\\Roaming\\NuGet\\NuGet.Config",
          "C:\\Program Files (x86)\\NuGet\\Config\\Microsoft.VisualStudio.Offline.config"
        ],
        "originalTargetFrameworks": [
          "net8.0"
        ],
        "sources": {
          "C:\\Program Files (x86)\\Microsoft SDKs\\NuGetPackages\\": {},
          "https://api.nuget.org/v3/index.json": {}
        },
        "frameworks": {
          "net8.0": {
            "targetAlias": "net8.0",
            "projectReferences": {}
          }
        },
        "warningProperties": {
          "warnAsError": [
            "NU1605"
          ]
        }
      },
      "frameworks": {
        "net8.0": {
          "targetAlias": "net8.0",
          "dependencies": {
            "Swashbuckle.AspNetCore": {
              "target": "Package",
              "version": "[6.6.2, )"
            }
          },
          "imports": [
            "net461",
            "net462",
            "net47",
            "net471",
            "net472",
            "net48",
            "net481"
          ],
          "assetTargetFallback": true,
          "warn": true,
          "frameworkReferences": {
            "Microsoft.AspNetCore.App": {
              "privateAssets": "none"
            },
            "Microsoft.NETCore.App": {
              "privateAssets": "all"
            }
          },
          "runtimeIdentifierGraphPath": "C:\\Program Files\\dotnet\\sdk\\8.0.410/PortableRuntimeIdentifierGraph.json"
        }
      }
    }
  }
}
```

---


## WebApplication2/obj/WebApplication2.csproj.nuget.g.props

```text
﻿<?xml version="1.0" encoding="utf-8" standalone="no"?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup Condition=" '$(ExcludeRestorePackageImports)' != 'true' ">
    <RestoreSuccess Condition=" '$(RestoreSuccess)' == '' ">True</RestoreSuccess>
    <RestoreTool Condition=" '$(RestoreTool)' == '' ">NuGet</RestoreTool>
    <ProjectAssetsFile Condition=" '$(ProjectAssetsFile)' == '' ">$(MSBuildThisFileDirectory)project.assets.json</ProjectAssetsFile>
    <NuGetPackageRoot Condition=" '$(NuGetPackageRoot)' == '' ">$(UserProfile)\.nuget\packages\</NuGetPackageRoot>
    <NuGetPackageFolders Condition=" '$(NuGetPackageFolders)' == '' ">C:\Users\zapev\.nuget\packages\</NuGetPackageFolders>
    <NuGetProjectStyle Condition=" '$(NuGetProjectStyle)' == '' ">PackageReference</NuGetProjectStyle>
    <NuGetToolVersion Condition=" '$(NuGetToolVersion)' == '' ">6.8.0</NuGetToolVersion>
  </PropertyGroup>
  <ItemGroup Condition=" '$(ExcludeRestorePackageImports)' != 'true' ">
    <SourceRoot Include="C:\Users\zapev\.nuget\packages\" />
  </ItemGroup>
  <ImportGroup Condition=" '$(ExcludeRestorePackageImports)' != 'true' ">
    <Import Project="$(NuGetPackageRoot)microsoft.extensions.apidescription.server\6.0.5\build\Microsoft.Extensions.ApiDescription.Server.props" Condition="Exists('$(NuGetPackageRoot)microsoft.extensions.apidescription.server\6.0.5\build\Microsoft.Extensions.ApiDescription.Server.props')" />
    <Import Project="$(NuGetPackageRoot)swashbuckle.aspnetcore\6.6.2\build\Swashbuckle.AspNetCore.props" Condition="Exists('$(NuGetPackageRoot)swashbuckle.aspnetcore\6.6.2\build\Swashbuckle.AspNetCore.props')" />
  </ImportGroup>
  <PropertyGroup Condition=" '$(ExcludeRestorePackageImports)' != 'true' ">
    <PkgMicrosoft_Extensions_ApiDescription_Server Condition=" '$(PkgMicrosoft_Extensions_ApiDescription_Server)' == '' ">C:\Users\zapev\.nuget\packages\microsoft.extensions.apidescription.server\6.0.5</PkgMicrosoft_Extensions_ApiDescription_Server>
  </PropertyGroup>
</Project>
```

---


## WebApplication2/obj/WebApplication2.csproj.nuget.g.targets

```text
﻿<?xml version="1.0" encoding="utf-8" standalone="no"?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <ImportGroup Condition=" '$(ExcludeRestorePackageImports)' != 'true' ">
    <Import Project="$(NuGetPackageRoot)microsoft.extensions.apidescription.server\6.0.5\build\Microsoft.Extensions.ApiDescription.Server.targets" Condition="Exists('$(NuGetPackageRoot)microsoft.extensions.apidescription.server\6.0.5\build\Microsoft.Extensions.ApiDescription.Server.targets')" />
  </ImportGroup>
</Project>
```

---


## WebApplication2/obj/project.assets.json

```json
{
  "version": 3,
  "targets": {
    "net8.0": {
      "Microsoft.Extensions.ApiDescription.Server/6.0.5": {
        "type": "package",
        "build": {
          "build/Microsoft.Extensions.ApiDescription.Server.props": {},
          "build/Microsoft.Extensions.ApiDescription.Server.targets": {}
        },
        "buildMultiTargeting": {
          "buildMultiTargeting/Microsoft.Extensions.ApiDescription.Server.props": {},
          "buildMultiTargeting/Microsoft.Extensions.ApiDescription.Server.targets": {}
        }
      },
      "Microsoft.OpenApi/1.6.14": {
        "type": "package",
        "compile": {
          "lib/netstandard2.0/Microsoft.OpenApi.dll": {
            "related": ".pdb;.xml"
          }
        },
        "runtime": {
          "lib/netstandard2.0/Microsoft.OpenApi.dll": {
            "related": ".pdb;.xml"
          }
        }
      },
      "Swashbuckle.AspNetCore/6.6.2": {
        "type": "package",
        "dependencies": {
          "Microsoft.Extensions.ApiDescription.Server": "6.0.5",
          "Swashbuckle.AspNetCore.Swagger": "6.6.2",
          "Swashbuckle.AspNetCore.SwaggerGen": "6.6.2",
          "Swashbuckle.AspNetCore.SwaggerUI": "6.6.2"
        },
        "build": {
          "build/Swashbuckle.AspNetCore.props": {}
        }
      },
      "Swashbuckle.AspNetCore.Swagger/6.6.2": {
        "type": "package",
        "dependencies": {
          "Microsoft.OpenApi": "1.6.14"
        },
        "compile": {
          "lib/net8.0/Swashbuckle.AspNetCore.Swagger.dll": {
            "related": ".pdb;.xml"
          }
        },
        "runtime": {
          "lib/net8.0/Swashbuckle.AspNetCore.Swagger.dll": {
            "related": ".pdb;.xml"
          }
        },
        "frameworkReferences": [
          "Microsoft.AspNetCore.App"
        ]
      },
      "Swashbuckle.AspNetCore.SwaggerGen/6.6.2": {
        "type": "package",
        "dependencies": {
          "Swashbuckle.AspNetCore.Swagger": "6.6.2"
        },
        "compile": {
          "lib/net8.0/Swashbuckle.AspNetCore.SwaggerGen.dll": {
            "related": ".pdb;.xml"
          }
        },
        "runtime": {
          "lib/net8.0/Swashbuckle.AspNetCore.SwaggerGen.dll": {
            "related": ".pdb;.xml"
          }
        }
      },
      "Swashbuckle.AspNetCore.SwaggerUI/6.6.2": {
        "type": "package",
        "compile": {
          "lib/net8.0/Swashbuckle.AspNetCore.SwaggerUI.dll": {
            "related": ".pdb;.xml"
          }
        },
        "runtime": {
          "lib/net8.0/Swashbuckle.AspNetCore.SwaggerUI.dll": {
            "related": ".pdb;.xml"
          }
        },
        "frameworkReferences": [
          "Microsoft.AspNetCore.App"
        ]
      }
    }
  },
  "libraries": {
    "Microsoft.Extensions.ApiDescription.Server/6.0.5": {
      "sha512": "Ckb5EDBUNJdFWyajfXzUIMRkhf52fHZOQuuZg/oiu8y7zDCVwD0iHhew6MnThjHmevanpxL3f5ci2TtHQEN6bw==",
      "type": "package",
      "path": "microsoft.extensions.apidescription.server/6.0.5",
      "hasTools": true,
      "files": [
        ".nupkg.metadata",
        ".signature.p7s",
        "Icon.png",
        "build/Microsoft.Extensions.ApiDescription.Server.props",
        "build/Microsoft.Extensions.ApiDescription.Server.targets",
        "buildMultiTargeting/Microsoft.Extensions.ApiDescription.Server.props",
        "buildMultiTargeting/Microsoft.Extensions.ApiDescription.Server.targets",
        "microsoft.extensions.apidescription.server.6.0.5.nupkg.sha512",
        "microsoft.extensions.apidescription.server.nuspec",
        "tools/Newtonsoft.Json.dll",
        "tools/dotnet-getdocument.deps.json",
        "tools/dotnet-getdocument.dll",
        "tools/dotnet-getdocument.runtimeconfig.json",
        "tools/net461-x86/GetDocument.Insider.exe",
        "tools/net461-x86/GetDocument.Insider.exe.config",
        "tools/net461-x86/Microsoft.Win32.Primitives.dll",
        "tools/net461-x86/System.AppContext.dll",
        "tools/net461-x86/System.Buffers.dll",
        "tools/net461-x86/System.Collections.Concurrent.dll",
        "tools/net461-x86/System.Collections.NonGeneric.dll",
        "tools/net461-x86/System.Collections.Specialized.dll",
        "tools/net461-x86/System.Collections.dll",
        "tools/net461-x86/System.ComponentModel.EventBasedAsync.dll",
        "tools/net461-x86/System.ComponentModel.Primitives.dll",
        "tools/net461-x86/System.ComponentModel.TypeConverter.dll",
        "tools/net461-x86/System.ComponentModel.dll",
        "tools/net461-x86/System.Console.dll",
        "tools/net461-x86/System.Data.Common.dll",
        "tools/net461-x86/System.Diagnostics.Contracts.dll",
        "tools/net461-x86/System.Diagnostics.Debug.dll",
        "tools/net461-x86/System.Diagnostics.DiagnosticSource.dll",
        "tools/net461-x86/System.Diagnostics.FileVersionInfo.dll",
        "tools/net461-x86/System.Diagnostics.Process.dll",
        "tools/net461-x86/System.Diagnostics.StackTrace.dll",
        "tools/net461-x86/System.Diagnostics.TextWriterTraceListener.dll",
        "tools/net461-x86/System.Diagnostics.Tools.dll",
        "tools/net461-x86/System.Diagnostics.TraceSource.dll",
        "tools/net461-x86/System.Diagnostics.Tracing.dll",
        "tools/net461-x86/System.Drawing.Primitives.dll",
        "tools/net461-x86/System.Dynamic.Runtime.dll",
        "tools/net461-x86/System.Globalization.Calendars.dll",
        "tools/net461-x86/System.Globalization.Extensions.dll",
        "tools/net461-x86/System.Globalization.dll",
        "tools/net461-x86/System.IO.Compression.ZipFile.dll",
        "tools/net461-x86/System.IO.Compression.dll",
        "tools/net461-x86/System.IO.FileSystem.DriveInfo.dll",
        "tools/net461-x86/System.IO.FileSystem.Primitives.dll",
        "tools/net461-x86/System.IO.FileSystem.Watcher.dll",
        "tools/net461-x86/System.IO.FileSystem.dll",
        "tools/net461-x86/System.IO.IsolatedStorage.dll",
        "tools/net461-x86/System.IO.MemoryMappedFiles.dll",
        "tools/net461-x86/System.IO.Pipes.dll",
        "tools/net461-x86/System.IO.UnmanagedMemoryStream.dll",
        "tools/net461-x86/System.IO.dll",
        "tools/net461-x86/System.Linq.Expressions.dll",
        "tools/net461-x86/System.Linq.Parallel.dll",
        "tools/net461-x86/System.Linq.Queryable.dll",
        "tools/net461-x86/System.Linq.dll",
        "tools/net461-x86/System.Memory.dll",
        "tools/net461-x86/System.Net.Http.dll",
        "tools/net461-x86/System.Net.NameResolution.dll",
        "tools/net461-x86/System.Net.NetworkInformation.dll",
        "tools/net461-x86/System.Net.Ping.dll",
        "tools/net461-x86/System.Net.Primitives.dll",
        "tools/net461-x86/System.Net.Requests.dll",
        "tools/net461-x86/System.Net.Security.dll",
        "tools/net461-x86/System.Net.Sockets.dll",
        "tools/net461-x86/System.Net.WebHeaderCollection.dll",
        "tools/net461-x86/System.Net.WebSockets.Client.dll",
        "tools/net461-x86/System.Net.WebSockets.dll",
        "tools/net461-x86/System.Numerics.Vectors.dll",
        "tools/net461-x86/System.ObjectModel.dll",
        "tools/net461-x86/System.Reflection.Extensions.dll",
        "tools/net461-x86/System.Reflection.Primitives.dll",
        "tools/net461-x86/System.Reflection.dll",
        "tools/net461-x86/System.Resources.Reader.dll",
        "tools/net461-x86/System.Resources.ResourceManager.dll",
        "tools/net461-x86/System.Resources.Writer.dll",
        "tools/net461-x86/System.Runtime.CompilerServices.Unsafe.dll",
        "tools/net461-x86/System.Runtime.CompilerServices.VisualC.dll",
        "tools/net461-x86/System.Runtime.Extensions.dll",
        "tools/net461-x86/System.Runtime.Handles.dll",
        "tools/net461-x86/System.Runtime.InteropServices.RuntimeInformation.dll",
        "tools/net461-x86/System.Runtime.InteropServices.dll",
        "tools/net461-x86/System.Runtime.Numerics.dll",
        "tools/net461-x86/System.Runtime.Serialization.Formatters.dll",
        "tools/net461-x86/System.Runtime.Serialization.Json.dll",
        "tools/net461-x86/System.Runtime.Serialization.Primitives.dll",
        "tools/net461-x86/System.Runtime.Serialization.Xml.dll",
        "tools/net461-x86/System.Runtime.dll",
        "tools/net461-x86/System.Security.Claims.dll",
        "tools/net461-x86/System.Security.Cryptography.Algorithms.dll",
        "tools/net461-x86/System.Security.Cryptography.Csp.dll",
        "tools/net461-x86/System.Security.Cryptography.Encoding.dll",
        "tools/net461-x86/System.Security.Cryptography.Primitives.dll",
        "tools/net461-x86/System.Security.Cryptography.X509Certificates.dll",
        "tools/net461-x86/System.Security.Principal.dll",
        "tools/net461-x86/System.Security.SecureString.dll",
        "tools/net461-x86/System.Text.Encoding.Extensions.dll",
        "tools/net461-x86/System.Text.Encoding.dll",
        "tools/net461-x86/System.Text.RegularExpressions.dll",
        "tools/net461-x86/System.Threading.Overlapped.dll",
        "tools/net461-x86/System.Threading.Tasks.Parallel.dll",
        "tools/net461-x86/System.Threading.Tasks.dll",
        "tools/net461-x86/System.Threading.Thread.dll",
        "tools/net461-x86/System.Threading.ThreadPool.dll",
        "tools/net461-x86/System.Threading.Timer.dll",
        "tools/net461-x86/System.Threading.dll",
        "tools/net461-x86/System.ValueTuple.dll",
        "tools/net461-x86/System.Xml.ReaderWriter.dll",
        "tools/net461-x86/System.Xml.XDocument.dll",
        "tools/net461-x86/System.Xml.XPath.XDocument.dll",
        "tools/net461-x86/System.Xml.XPath.dll",
        "tools/net461-x86/System.Xml.XmlDocument.dll",
        "tools/net461-x86/System.Xml.XmlSerializer.dll",
        "tools/net461-x86/netstandard.dll",
        "tools/net461/GetDocument.Insider.exe",
        "tools/net461/GetDocument.Insider.exe.config",
        "tools/net461/Microsoft.Win32.Primitives.dll",
        "tools/net461/System.AppContext.dll",
        "tools/net461/System.Buffers.dll",
        "tools/net461/System.Collections.Concurrent.dll",
        "tools/net461/System.Collections.NonGeneric.dll",
        "tools/net461/System.Collections.Specialized.dll",
        "tools/net461/System.Collections.dll",
        "tools/net461/System.ComponentModel.EventBasedAsync.dll",
        "tools/net461/System.ComponentModel.Primitives.dll",
        "tools/net461/System.ComponentModel.TypeConverter.dll",
        "tools/net461/System.ComponentModel.dll",
        "tools/net461/System.Console.dll",
        "tools/net461/System.Data.Common.dll",
        "tools/net461/System.Diagnostics.Contracts.dll",
        "tools/net461/System.Diagnostics.Debug.dll",
        "tools/net461/System.Diagnostics.DiagnosticSource.dll",
        "tools/net461/System.Diagnostics.FileVersionInfo.dll",
        "tools/net461/System.Diagnostics.Process.dll",
        "tools/net461/System.Diagnostics.StackTrace.dll",
        "tools/net461/System.Diagnostics.TextWriterTraceListener.dll",
        "tools/net461/System.Diagnostics.Tools.dll",
        "tools/net461/System.Diagnostics.TraceSource.dll",
        "tools/net461/System.Diagnostics.Tracing.dll",
        "tools/net461/System.Drawing.Primitives.dll",
        "tools/net461/System.Dynamic.Runtime.dll",
        "tools/net461/System.Globalization.Calendars.dll",
        "tools/net461/System.Globalization.Extensions.dll",
        "tools/net461/System.Globalization.dll",
        "tools/net461/System.IO.Compression.ZipFile.dll",
        "tools/net461/System.IO.Compression.dll",
        "tools/net461/System.IO.FileSystem.DriveInfo.dll",
        "tools/net461/System.IO.FileSystem.Primitives.dll",
        "tools/net461/System.IO.FileSystem.Watcher.dll",
        "tools/net461/System.IO.FileSystem.dll",
        "tools/net461/System.IO.IsolatedStorage.dll",
        "tools/net461/System.IO.MemoryMappedFiles.dll",
        "tools/net461/System.IO.Pipes.dll",
        "tools/net461/System.IO.UnmanagedMemoryStream.dll",
        "tools/net461/System.IO.dll",
        "tools/net461/System.Linq.Expressions.dll",
        "tools/net461/System.Linq.Parallel.dll",
        "tools/net461/System.Linq.Queryable.dll",
        "tools/net461/System.Linq.dll",
        "tools/net461/System.Memory.dll",
        "tools/net461/System.Net.Http.dll",
        "tools/net461/System.Net.NameResolution.dll",
        "tools/net461/System.Net.NetworkInformation.dll",
        "tools/net461/System.Net.Ping.dll",
        "tools/net461/System.Net.Primitives.dll",
        "tools/net461/System.Net.Requests.dll",
        "tools/net461/System.Net.Security.dll",
        "tools/net461/System.Net.Sockets.dll",
        "tools/net461/System.Net.WebHeaderCollection.dll",
        "tools/net461/System.Net.WebSockets.Client.dll",
        "tools/net461/System.Net.WebSockets.dll",
        "tools/net461/System.Numerics.Vectors.dll",
        "tools/net461/System.ObjectModel.dll",
        "tools/net461/System.Reflection.Extensions.dll",
        "tools/net461/System.Reflection.Primitives.dll",
        "tools/net461/System.Reflection.dll",
        "tools/net461/System.Resources.Reader.dll",
        "tools/net461/System.Resources.ResourceManager.dll",
        "tools/net461/System.Resources.Writer.dll",
        "tools/net461/System.Runtime.CompilerServices.Unsafe.dll",
        "tools/net461/System.Runtime.CompilerServices.VisualC.dll",
        "tools/net461/System.Runtime.Extensions.dll",
        "tools/net461/System.Runtime.Handles.dll",
        "tools/net461/System.Runtime.InteropServices.RuntimeInformation.dll",
        "tools/net461/System.Runtime.InteropServices.dll",
        "tools/net461/System.Runtime.Numerics.dll",
        "tools/net461/System.Runtime.Serialization.Formatters.dll",
        "tools/net461/System.Runtime.Serialization.Json.dll",
        "tools/net461/System.Runtime.Serialization.Primitives.dll",
        "tools/net461/System.Runtime.Serialization.Xml.dll",
        "tools/net461/System.Runtime.dll",
        "tools/net461/System.Security.Claims.dll",
        "tools/net461/System.Security.Cryptography.Algorithms.dll",
        "tools/net461/System.Security.Cryptography.Csp.dll",
        "tools/net461/System.Security.Cryptography.Encoding.dll",
        "tools/net461/System.Security.Cryptography.Primitives.dll",
        "tools/net461/System.Security.Cryptography.X509Certificates.dll",
        "tools/net461/System.Security.Principal.dll",
        "tools/net461/System.Security.SecureString.dll",
        "tools/net461/System.Text.Encoding.Extensions.dll",
        "tools/net461/System.Text.Encoding.dll",
        "tools/net461/System.Text.RegularExpressions.dll",
        "tools/net461/System.Threading.Overlapped.dll",
        "tools/net461/System.Threading.Tasks.Parallel.dll",
        "tools/net461/System.Threading.Tasks.dll",
        "tools/net461/System.Threading.Thread.dll",
        "tools/net461/System.Threading.ThreadPool.dll",
        "tools/net461/System.Threading.Timer.dll",
        "tools/net461/System.Threading.dll",
        "tools/net461/System.ValueTuple.dll",
        "tools/net461/System.Xml.ReaderWriter.dll",
        "tools/net461/System.Xml.XDocument.dll",
        "tools/net461/System.Xml.XPath.XDocument.dll",
        "tools/net461/System.Xml.XPath.dll",
        "tools/net461/System.Xml.XmlDocument.dll",
        "tools/net461/System.Xml.XmlSerializer.dll",
        "tools/net461/netstandard.dll",
        "tools/netcoreapp2.1/GetDocument.Insider.deps.json",
        "tools/netcoreapp2.1/GetDocument.Insider.dll",
        "tools/netcoreapp2.1/GetDocument.Insider.runtimeconfig.json",
        "tools/netcoreapp2.1/System.Diagnostics.DiagnosticSource.dll"
      ]
    },
    "Microsoft.OpenApi/1.6.14": {
      "sha512": "tTaBT8qjk3xINfESyOPE2rIellPvB7qpVqiWiyA/lACVvz+xOGiXhFUfohcx82NLbi5avzLW0lx+s6oAqQijfw==",
      "type": "package",
      "path": "microsoft.openapi/1.6.14",
      "files": [
        ".nupkg.metadata",
        ".signature.p7s",
        "README.md",
        "lib/netstandard2.0/Microsoft.OpenApi.dll",
        "lib/netstandard2.0/Microsoft.OpenApi.pdb",
        "lib/netstandard2.0/Microsoft.OpenApi.xml",
        "microsoft.openapi.1.6.14.nupkg.sha512",
        "microsoft.openapi.nuspec"
      ]
    },
    "Swashbuckle.AspNetCore/6.6.2": {
      "sha512": "+NB4UYVYN6AhDSjW0IJAd1AGD8V33gemFNLPaxKTtPkHB+HaKAKf9MGAEUPivEWvqeQfcKIw8lJaHq6LHljRuw==",
      "type": "package",
      "path": "swashbuckle.aspnetcore/6.6.2",
      "files": [
        ".nupkg.metadata",
        ".signature.p7s",
        "build/Swashbuckle.AspNetCore.props",
        "swashbuckle.aspnetcore.6.6.2.nupkg.sha512",
        "swashbuckle.aspnetcore.nuspec"
      ]
    },
    "Swashbuckle.AspNetCore.Swagger/6.6.2": {
      "sha512": "ovgPTSYX83UrQUWiS5vzDcJ8TEX1MAxBgDFMK45rC24MorHEPQlZAHlaXj/yth4Zf6xcktpUgTEBvffRQVwDKA==",
      "type": "package",
      "path": "swashbuckle.aspnetcore.swagger/6.6.2",
      "files": [
        ".nupkg.metadata",
        ".signature.p7s",
        "lib/net5.0/Swashbuckle.AspNetCore.Swagger.dll",
        "lib/net5.0/Swashbuckle.AspNetCore.Swagger.pdb",
        "lib/net5.0/Swashbuckle.AspNetCore.Swagger.xml",
        "lib/net6.0/Swashbuckle.AspNetCore.Swagger.dll",
        "lib/net6.0/Swashbuckle.AspNetCore.Swagger.pdb",
        "lib/net6.0/Swashbuckle.AspNetCore.Swagger.xml",
        "lib/net7.0/Swashbuckle.AspNetCore.Swagger.dll",
        "lib/net7.0/Swashbuckle.AspNetCore.Swagger.pdb",
        "lib/net7.0/Swashbuckle.AspNetCore.Swagger.xml",
        "lib/net8.0/Swashbuckle.AspNetCore.Swagger.dll",
        "lib/net8.0/Swashbuckle.AspNetCore.Swagger.pdb",
        "lib/net8.0/Swashbuckle.AspNetCore.Swagger.xml",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.Swagger.dll",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.Swagger.pdb",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.Swagger.xml",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.Swagger.dll",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.Swagger.pdb",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.Swagger.xml",
        "package-readme.md",
        "swashbuckle.aspnetcore.swagger.6.6.2.nupkg.sha512",
        "swashbuckle.aspnetcore.swagger.nuspec"
      ]
    },
    "Swashbuckle.AspNetCore.SwaggerGen/6.6.2": {
      "sha512": "zv4ikn4AT1VYuOsDCpktLq4QDq08e7Utzbir86M5/ZkRaLXbCPF11E1/vTmOiDzRTl0zTZINQU2qLKwTcHgfrA==",
      "type": "package",
      "path": "swashbuckle.aspnetcore.swaggergen/6.6.2",
      "files": [
        ".nupkg.metadata",
        ".signature.p7s",
        "lib/net5.0/Swashbuckle.AspNetCore.SwaggerGen.dll",
        "lib/net5.0/Swashbuckle.AspNetCore.SwaggerGen.pdb",
        "lib/net5.0/Swashbuckle.AspNetCore.SwaggerGen.xml",
        "lib/net6.0/Swashbuckle.AspNetCore.SwaggerGen.dll",
        "lib/net6.0/Swashbuckle.AspNetCore.SwaggerGen.pdb",
        "lib/net6.0/Swashbuckle.AspNetCore.SwaggerGen.xml",
        "lib/net7.0/Swashbuckle.AspNetCore.SwaggerGen.dll",
        "lib/net7.0/Swashbuckle.AspNetCore.SwaggerGen.pdb",
        "lib/net7.0/Swashbuckle.AspNetCore.SwaggerGen.xml",
        "lib/net8.0/Swashbuckle.AspNetCore.SwaggerGen.dll",
        "lib/net8.0/Swashbuckle.AspNetCore.SwaggerGen.pdb",
        "lib/net8.0/Swashbuckle.AspNetCore.SwaggerGen.xml",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.SwaggerGen.dll",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.SwaggerGen.pdb",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.SwaggerGen.xml",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.SwaggerGen.dll",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.SwaggerGen.pdb",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.SwaggerGen.xml",
        "package-readme.md",
        "swashbuckle.aspnetcore.swaggergen.6.6.2.nupkg.sha512",
        "swashbuckle.aspnetcore.swaggergen.nuspec"
      ]
    },
    "Swashbuckle.AspNetCore.SwaggerUI/6.6.2": {
      "sha512": "mBBb+/8Hm2Q3Wygag+hu2jj69tZW5psuv0vMRXY07Wy+Rrj40vRP8ZTbKBhs91r45/HXT4aY4z0iSBYx1h6JvA==",
      "type": "package",
      "path": "swashbuckle.aspnetcore.swaggerui/6.6.2",
      "files": [
        ".nupkg.metadata",
        ".signature.p7s",
        "lib/net5.0/Swashbuckle.AspNetCore.SwaggerUI.dll",
        "lib/net5.0/Swashbuckle.AspNetCore.SwaggerUI.pdb",
        "lib/net5.0/Swashbuckle.AspNetCore.SwaggerUI.xml",
        "lib/net6.0/Swashbuckle.AspNetCore.SwaggerUI.dll",
        "lib/net6.0/Swashbuckle.AspNetCore.SwaggerUI.pdb",
        "lib/net6.0/Swashbuckle.AspNetCore.SwaggerUI.xml",
        "lib/net7.0/Swashbuckle.AspNetCore.SwaggerUI.dll",
        "lib/net7.0/Swashbuckle.AspNetCore.SwaggerUI.pdb",
        "lib/net7.0/Swashbuckle.AspNetCore.SwaggerUI.xml",
        "lib/net8.0/Swashbuckle.AspNetCore.SwaggerUI.dll",
        "lib/net8.0/Swashbuckle.AspNetCore.SwaggerUI.pdb",
        "lib/net8.0/Swashbuckle.AspNetCore.SwaggerUI.xml",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.SwaggerUI.dll",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.SwaggerUI.pdb",
        "lib/netcoreapp3.0/Swashbuckle.AspNetCore.SwaggerUI.xml",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.SwaggerUI.dll",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.SwaggerUI.pdb",
        "lib/netstandard2.0/Swashbuckle.AspNetCore.SwaggerUI.xml",
        "package-readme.md",
        "swashbuckle.aspnetcore.swaggerui.6.6.2.nupkg.sha512",
        "swashbuckle.aspnetcore.swaggerui.nuspec"
      ]
    }
  },
  "projectFileDependencyGroups": {
    "net8.0": [
      "Swashbuckle.AspNetCore >= 6.6.2"
    ]
  },
  "packageFolders": {
    "C:\\Users\\zapev\\.nuget\\packages\\": {}
  },
  "project": {
    "version": "1.0.0",
    "restore": {
      "projectUniqueName": "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\WebApplication2.csproj",
      "projectName": "WebApplication2",
      "projectPath": "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\WebApplication2.csproj",
      "packagesPath": "C:\\Users\\zapev\\.nuget\\packages\\",
      "outputPath": "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\obj\\",
      "projectStyle": "PackageReference",
      "configFilePaths": [
        "C:\\Users\\zapev\\AppData\\Roaming\\NuGet\\NuGet.Config",
        "C:\\Program Files (x86)\\NuGet\\Config\\Microsoft.VisualStudio.Offline.config"
      ],
      "originalTargetFrameworks": [
        "net8.0"
      ],
      "sources": {
        "C:\\Program Files (x86)\\Microsoft SDKs\\NuGetPackages\\": {},
        "https://api.nuget.org/v3/index.json": {}
      },
      "frameworks": {
        "net8.0": {
          "targetAlias": "net8.0",
          "projectReferences": {}
        }
      },
      "warningProperties": {
        "warnAsError": [
          "NU1605"
        ]
      }
    },
    "frameworks": {
      "net8.0": {
        "targetAlias": "net8.0",
        "dependencies": {
          "Swashbuckle.AspNetCore": {
            "target": "Package",
            "version": "[6.6.2, )"
          }
        },
        "imports": [
          "net461",
          "net462",
          "net47",
          "net471",
          "net472",
          "net48",
          "net481"
        ],
        "assetTargetFallback": true,
        "warn": true,
        "frameworkReferences": {
          "Microsoft.AspNetCore.App": {
            "privateAssets": "none"
          },
          "Microsoft.NETCore.App": {
            "privateAssets": "all"
          }
        },
        "runtimeIdentifierGraphPath": "C:\\Program Files\\dotnet\\sdk\\8.0.410/PortableRuntimeIdentifierGraph.json"
      }
    }
  }
}
```

---


## WebApplication2/obj/project.nuget.cache

```text
{
  "version": 2,
  "dgSpecHash": "CYmr7OPSutXz7VitRvsRr4BqSzMLx3l5Yh37HIo5RS8EtB2a2dumOx2AFko/h/bTJGnkAKov2Y9fWXgFTGlasw==",
  "success": true,
  "projectFilePath": "D:\\Study\\3_Course\\2_Semester\\College-projects_3.2\\ASP-ADO.NET\\WebApplication2\\WebApplication2\\WebApplication2.csproj",
  "expectedPackageFiles": [
    "C:\\Users\\zapev\\.nuget\\packages\\microsoft.extensions.apidescription.server\\6.0.5\\microsoft.extensions.apidescription.server.6.0.5.nupkg.sha512",
    "C:\\Users\\zapev\\.nuget\\packages\\microsoft.openapi\\1.6.14\\microsoft.openapi.1.6.14.nupkg.sha512",
    "C:\\Users\\zapev\\.nuget\\packages\\swashbuckle.aspnetcore\\6.6.2\\swashbuckle.aspnetcore.6.6.2.nupkg.sha512",
    "C:\\Users\\zapev\\.nuget\\packages\\swashbuckle.aspnetcore.swagger\\6.6.2\\swashbuckle.aspnetcore.swagger.6.6.2.nupkg.sha512",
    "C:\\Users\\zapev\\.nuget\\packages\\swashbuckle.aspnetcore.swaggergen\\6.6.2\\swashbuckle.aspnetcore.swaggergen.6.6.2.nupkg.sha512",
    "C:\\Users\\zapev\\.nuget\\packages\\swashbuckle.aspnetcore.swaggerui\\6.6.2\\swashbuckle.aspnetcore.swaggerui.6.6.2.nupkg.sha512"
  ],
  "logs": []
}
```

---


## WebApplication2.sln

```text
﻿
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.8.34511.84
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "WebApplication2", "WebApplication2\WebApplication2.csproj", "{3B4B7498-10F6-498E-9902-16A6A65FC27A}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{3B4B7498-10F6-498E-9902-16A6A65FC27A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3B4B7498-10F6-498E-9902-16A6A65FC27A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3B4B7498-10F6-498E-9902-16A6A65FC27A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3B4B7498-10F6-498E-9902-16A6A65FC27A}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {81841606-1395-457E-8404-F67E6A46B4EE}
	EndGlobalSection
EndGlobal

```

---

