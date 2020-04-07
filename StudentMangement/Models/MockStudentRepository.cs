using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMangement.Models
{
    public class MockStudentRepository : IStudentRepository
    {
        private List<Student> _studentsList;
        public MockStudentRepository()
        {
            _studentsList = new List<Student>()
            {
                new Student(){Id=1,Name="张三",ClassName="一年级",Email="Tony_zhang@test.com"},
                new Student(){Id=2,Name="李四",ClassName="二年级",Email="Lisi@test.com"},
                new Student(){Id=3,Name="王五",ClassName="二年级",Email="King_five@test.com"}
            };
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentsList;
        }

        public Student GetStudent(int id)
        {
            return _studentsList.FirstOrDefault(a => a.Id == id);
        }
    }
}
