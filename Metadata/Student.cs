﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Metadata
{
    public partial class Student
    {
        public Student()
        {
        }
        public int ID { get; set; }
        public string Studentname { get; set; }
        public string Cpf { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phonenumber { get; set; }
        public string Register { get; set; }
        public string Picture { get; set; }
        public string Passcode { get; set; }
        public bool Active { get; set; }
        public int ClassID { get; set; }
        public virtual Class Class { get; set; }
    }
}