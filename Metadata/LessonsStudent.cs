﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Metadata
{
    public partial class LessonsStudent
    {
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }
        public int LessonID { get; set; }
        public virtual Lesson Lesson { get; set; }
        public bool Presence { get; set; }
    }
}