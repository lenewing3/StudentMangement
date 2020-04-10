﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMangement.Models
{
    public interface IStudentRepository
    {
        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student GetStudent(int id);
        /// <summary>
        /// 获取所有学生列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Student> GetAllStudents();
        /// <summary>
        /// 新增一名学生的信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Student Add(Student student);
        /// <summary>
        /// 更新一名学生的信息
        /// </summary>
        /// <param name="updateStudent"></param>
        /// <returns></returns>
        Student Update(Student updateStudent);
        /// <summary>
        /// 删除一名学生的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student Delete(int id);
    }
}
