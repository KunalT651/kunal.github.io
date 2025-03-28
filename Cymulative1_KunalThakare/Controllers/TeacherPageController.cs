using Microsoft.AspNetCore.Mvc;
using Cymulative1_KunalThakare.Models;

namespace Cymulative1_KunalThakare.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly SchoolDbContext _context;

        public TeacherPageController(SchoolDbContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            List<Teacher> teachersList = new List<Teacher>();

            using (var connection = _context.AccessDatabase())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM teachers";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
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

                        teachersList.Add(teacher);
                    }
                }
            }

            return View(teachersList); //Returns teachers list to the List.cshtml view
        }

        public IActionResult Show(int? id)
        {
            if (!id.HasValue)
            {
                return View();
            }

            Teacher teacher = null;

            using (var connection = _context.AccessDatabase())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM teachers WHERE teacherid = @id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        teacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(reader["hiredate"]),
                            Salary = Convert.ToDecimal(reader["salary"])
                        };
                    }
                }
            }

            return View(teacher); // Pass the teacher(according to id) to Show.cshtml
        }
    }
}