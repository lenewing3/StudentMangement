using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using StudentMangement.Models;
using StudentMangement.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentMangement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly HostingEnvironment _hostingEnvironment;

        //构造函数注入IStudentRepository
        public HomeController(IStudentRepository studentRepository, HostingEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Student> students = _studentRepository.GetAllStudents();
            //返回学生列表
            return View(students);
        }
        public IActionResult Details(int? id)   //声明参数时在类型后加?号代表该参数为可空参数
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Student = _studentRepository.GetStudent(id??1), //??判断赋值表达式，当表达式左边的值为null时，将表达式右边的值赋给左边，否则跳过。
                PageTitle = "学生信息"
            };
            //返回学生信息
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //返回Create视图
            return View();
        }
        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img");
                    uniqueFileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    ClassName = model.ClassName,
                    PhotoPath = uniqueFileName
                };
                _studentRepository.Add(newStudent);
                return RedirectToAction("Details", new { id = newStudent.Id });
            }
            return View();
        }
    }
}
