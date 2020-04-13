using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMangement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Name = "小欢喜",
                    ClassName = ClassNameEnum.FirstGrade,
                    Email = "xiaohuanxi@qq.com"
                },
                new Student
                {
                    Id = 2,
                    Name = "小彭宇",
                    ClassName = ClassNameEnum.SecondGrade,
                    Email = "xiaopengyu@sohu.com"
                },
                new Student
                {
                    Id = 3,
                    Name = "彭于晏",
                    ClassName = ClassNameEnum.ThirdGrade,
                    Email = "pengyuyan@163.com"
                }
            );
        }
    }
}
