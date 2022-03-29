using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project1.Models;
using System.Data;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private string sqlDatasource;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
        }

        [HttpGet]
        public JsonResult Get() {
            string query = @"select * from mytestdb.Employee";
            DataTable table = new DataTable();
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"insert into mytestdb.Employee(EmployeeName, Department, 
                DateOfJoining,PhotoFileName) values (@EmployeeName,@Department,@DateOfJoining,
                @PhotoFileName);";
            DataTable table = new DataTable();
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.employeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.departmentName);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.dateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.photoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("New Employee added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"update mytestdb.Employee set EmployeeName=@EmployeeName,
                    Department=@Department,DateOfJoining=@DateOfJoining,
                    PhotoFileName=@PhotoFileName where EmployeeId=@EmployeeId;";
            DataTable table = new DataTable();
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.employeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.departmentName);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.dateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.photoFileName);
                    myCommand.Parameters.AddWithValue("@EmployeeId", emp.employeeId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Employee details updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from mytestdb.Employee where EmployeeId=@EmployeeId;";
            DataTable table = new DataTable();
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Employee entry deleted Successfully");
        }

        [HttpGet("{id}")]
        public JsonResult getEmployeebyId(int id) {
            string query = @"select * from mytestdb.Employee where EmployeeId=@EmployeeId;";
            DataTable table = new DataTable();

            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile(int id)
        {
            try 
            {
                var httpReq = Request.Form;
                var postedFile = httpReq.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create)) 
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception e)
            {
                return new JsonResult("anonymous.png");
            }
        }
        }
}
