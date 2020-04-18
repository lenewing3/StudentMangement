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
                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    ClassName = model.ClassName,
                    PhotoPath = ProcessUploadFile(model)
                };
                newStudent = _studentRepository.Add(newStudent);
                return RedirectToAction("Details", new { id = newStudent.Id });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);
            if (student != null)
            {
                StudentEditViewModel studentEditViewModel = new StudentEditViewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    ClassName = student.ClassName,
                    Email = student.Email,
                    ExistingPhotoPath = student.PhotoPath
                };
                return View(studentEditViewModel);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = _studentRepository.GetStudent(model.Id);
                if (student != null)
                {
                    student.Name = model.Name;
                    student.Email = model.Email;
                    student.ClassName = model.ClassName;

                    //判断是否更新头像
                    if (model.Photo != null)
                    {
                        //判断原来是否有头像
                        if (model.ExistingPhotoPath != null)
                        {
                            //获得原头像图片路径
                            string oldFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "img", model.ExistingPhotoPath);
                            //删除原头像
                            System.IO.File.Delete(oldFilePath);
                        }
                        student.PhotoPath = ProcessUploadFile(model);
                    }
                    _studentRepository.Update(student);
                    return RedirectToAction("Details", new { id = student.Id });
                }
            }
            return View(model);
        }
        /// <summary>
        /// 将上传文件保存到指定的路径中,并返回上传文件保存的路径
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                //通过已经依赖注入的服务:HostingEnvironment的WebRootPath属性获取"wwwroot"目录的路径
                //使用Path.Combine()方法生成img目录的路径,该目录是"wwwroot"的子目录
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img");
                //使用Guid.NewGuid()方法生成唯一标识后重命名上传的文件名,重命名格式:"唯一标识_原文件名"
                uniqueFileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";
                //使用Path.Combine()方法生成文件存储路径
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //FileStream为非托管型资源,需要手动释放,使用using语句进行释放.
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    //将上传的文件复制到目标流
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
