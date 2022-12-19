using CakesByRAndRCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace CakesByRAndRCore.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateStudent()
        {
            return View();
        }

        public IActionResult ShowHideDropdown()
        {
            return View();
        }

        

        [HttpPost]
        public JsonResult CreateStudent(StudentModel student)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, Data = "Model was invalid." });

            //return this.Ok("Form Data received!");
            return Json(new { success = true, Data = "Student was created successfully." });
        }

    }
}
