﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Metadata
{
    public partial class Teacher
    {
        public Teacher()
        {
            this.Subjects = new HashSet<SubjectTeachers>();
        }
        public int ID { get; set; }
        public string Teachername { get; set; }
        public string Cpf { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phonenumber { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Passcode { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<SubjectTeachers> Subjects { get; set; }
    }
}