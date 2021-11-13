using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentWebApi.Models
{
    public class Student
    {
        public int STT { set; get; }
        public String MASV { get; set; }
        public String HOTEN { get; set; }
        public DateTime NGSINH { get; set; }
        public String GT { get; set; }
        public String KHOA { get; set; }

    }
}