using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers;

//https://localhost:xxxxx:api/students
[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    // GET https://localhost:xxxxx:api/students
    [HttpGet]
    public IActionResult GetAllStudents()
    {
        string[] studentNames = ["Taras", "Daryna", "Denys", "Anton"];

        return Ok(studentNames);
    }
}