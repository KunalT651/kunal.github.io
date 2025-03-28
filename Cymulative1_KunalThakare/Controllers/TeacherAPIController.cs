using Microsoft.AspNetCore.Mvc;
using Cymulative1_KunalThakare.Models;

namespace Cymulative1_KunalThakare.Controllers
{
    [ApiController]
    [Route("api/Teacher")]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all the teachers.
        /// This list will be shown in a Tabular form is Teacher List Tab using List.cshtml
        /// </summary>
        /// <returns>List of teachers(All the information related to each Teacher.)</returns>
        [HttpGet]
        [Route("ListAllTeachers")]
        public List<Teacher> ListAllTeachers()
        {
            List<Teacher> TeacherList = new List<Teacher>(); // List to hold teacher details

            using (var connection = _context.AccessDatabase())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM teachers"; // This query fetches all the data from teachers table

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Object to hold details of teacher row
                        Teacher teacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(reader["hiredate"]),
                            Salary = Convert.ToDecimal(reader["salary"])
                        };

                        if (teacher != null)
                            TeacherList.Add(teacher);
                    }
                }
            }

            return TeacherList;
        }

        /// <summary>
        /// Returns all information about a teacher by ID.
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <returns>Teacher details</returns>
        [HttpGet("{id}")]
        public IActionResult GetTeacher(int id)
        {
            using (var connection = _context.AccessDatabase())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM teachers WHERE teacherid = @id"; //Query to fetch a row from teacher table by id
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Teacher teacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(reader["hiredate"]),
                            Salary = Convert.ToDecimal(reader["salary"])
                        };


                        return Ok(teacher);
                    }
                }
            }

            return NotFound("Teacher not found.");
        }
    }
}