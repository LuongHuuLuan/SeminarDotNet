using StudentWebApi.Models;
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace StudentWebApi.Controllers
{
    public class StudentsController : ApiController
    {
        // GET: api/Students
        [HttpGet]
        public ArrayList Get()
        {
            ArrayList students = new ArrayList();
            // tạo chuỗi kết nối
            String conString = @"Data Source=LUAN-PC\SQLEXPRESS;Database=QLSV;Integrated Security=True";
            // tạo đối tượng kết nối
            SqlConnection connect = new SqlConnection(conString);
            // tạo command
            SqlCommand command = connect.CreateCommand();
            // set kiểu dùng cho command text là stored procedure
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            // chọn stored
            //command.CommandText = "dbo.GETSV";
            command.CommandText = "SELECT * FROM dbo.SINHVIEN";
            //mở kết nối database
            connect.Open();
            // thực thi công việc trong database
            SqlDataReader reader = command.ExecuteReader();
            // xử lý dữ liệu trả về
            while (reader.Read())
            {
                Student student = new Student();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    // lấy ra tên cột
                    String colName = reader.GetName(i);
                    // lấy giá trị của cột
                    var value = reader.GetValue(i);
                    // lấy ra proertyifo của student có tên thuộc tính trùng với colName
                    PropertyInfo property = student.GetType().GetProperty(colName);
                    if (property != null && value != DBNull.Value)
                    {
                        // set lại giá trị cho thuộc tính được lấy ra
                        property.SetValue(student, value);
                    }

                }
                students.Add(student);
            }

            return students;
        }

        // GET: api/Students/mssv
        [HttpGet]
        public HttpResponseMessage Get(String mssv)
        {
            Student result = new Student();
            String conString = @"Data Source=LUAN-PC\SQLEXPRESS;Database=QLSV;Integrated Security=True";
            SqlConnection connect = new SqlConnection(conString);
            SqlCommand command = connect.CreateCommand();
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            //command.CommandText = "dbo.GETSV1";
            command.CommandText = "SELECT * FROM dbo.SINHVIEN sv WHERE sv.MASV = @MASV";
            command.Parameters.AddWithValue("@MASV", mssv);
            connect.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    String colname = reader.GetName(i);
                    var value = reader.GetValue(i);

                    PropertyInfo property = result.GetType().GetProperty(colname);
                    if (property != null && value != DBNull.Value)
                    {
                        property.SetValue(result, value);
                    }
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/Student
        [HttpPost]
        //[ResponseType(typeof(Student))]
        public HttpResponseMessage Post([FromBody] Student s)
        {
            bool result = true;
            String conString = @"Data Source=LUAN-PC\SQLEXPRESS;Database=QLSV;Integrated Security=True";
            SqlConnection connect = new SqlConnection(conString);
            SqlCommand command = connect.CreateCommand();
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            //command.CommandText = "dbo.POSTSV";
            command.CommandText = "INSERT INTO dbo.SINHVIEN (STT, MASV, HOTEN, NGSINH, GT, KHOA) VALUES (@STT, @MASV, @HOTEN, @NGSINH, @GT, @KHOA)";
            //mở kết nối database
            command.Parameters.AddWithValue("@STT", s.STT);
            command.Parameters.AddWithValue("@MASV", s.MASV);
            command.Parameters.AddWithValue("@HOTEN", s.HOTEN);
            command.Parameters.AddWithValue("@NGSINH", s.NGSINH);
            command.Parameters.AddWithValue("@GT", s.GT);
            command.Parameters.AddWithValue("@KHOA", s.KHOA);
            connect.Open();
            try
            {
                int reader = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                result = false;
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
            connect.Close();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT: api/Student/mssv
        [HttpPut]
        public HttpResponseMessage Put([FromBody] Student s)
        {
            bool result = true;
            String conString = @"Data Source=LUAN-PC\SQLEXPRESS;Database=QLSV;Integrated Security=True";
            SqlConnection connect = new SqlConnection(conString);
            SqlCommand command = connect.CreateCommand();
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            //command.CommandText = "dbo.UPDATESV";
            command.CommandText = "UPDATE dbo.SINHVIEN SET STT = @STT, HOTEN = @HOTEN, NGSINH = @NGSINH, GT = @GT, KHOA = @KHOA WHERE MASV = @MASV";
            command.Parameters.AddWithValue("@STT", s.STT);
            command.Parameters.AddWithValue("@MASV", s.MASV);
            command.Parameters.AddWithValue("@HOTEN", s.HOTEN);
            command.Parameters.AddWithValue("@NGSINH", s.NGSINH);
            command.Parameters.AddWithValue("@GT", s.GT);
            command.Parameters.AddWithValue("@KHOA", s.KHOA);
            connect.Open();
            try
            {
                int reader = command.ExecuteNonQuery();
                if (reader < 1)
                {
                    result = false;
                    return Request.CreateResponse(HttpStatusCode.NotFound, result);
                }
            }
            catch (Exception e)
            {
                result = false;
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
            connect.Close();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE: api/Student/mssv
        public HttpResponseMessage Delete(String mssv)
        {
            bool result = true;
            String conString = @"Data Source=LUAN-PC\SQLEXPRESS;Database=QLSV;Integrated Security=True";
            SqlConnection connect = new SqlConnection(conString);
            SqlCommand command = connect.CreateCommand();
            //command.CommandType = System.Data.CommandType.StoredProcedure;
            //command.CommandText = "dbo.DELETESV";
            command.CommandText = "DELETE FROM dbo.SINHVIEN WHERE MASV = @MASV";
            command.Parameters.AddWithValue("@MASV", mssv);
            connect.Open();
            try
            {
                int reader = command.ExecuteNonQuery();
                if (reader < 1)
                {
                    result = false;
                    return Request.CreateResponse(HttpStatusCode.NotFound, result);
                }
            }
            catch (Exception e)
            {
                result = false;
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
            connect.Close();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
