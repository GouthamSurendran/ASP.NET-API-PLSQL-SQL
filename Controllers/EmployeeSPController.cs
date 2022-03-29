using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project1.Models;
using System.Data;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSPController : Controller
    {
        private readonly IConfiguration _configuration;
        private string sqlDatasource;

        public EmployeeSPController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable table = new DataTable();
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand("get_employee_details", mycon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
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

            using (MySqlConnection mycon = new MySqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand("insert_employee", mycon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add(new MySqlParameter("PEmployeeName", emp.employeeName));
                    myCommand.Parameters.Add(new MySqlParameter("PDepartmentName", emp.departmentName));
                    myCommand.Parameters.Add(new MySqlParameter("PDateOfJoining", emp.dateOfJoining));
                    myCommand.Parameters.Add(new MySqlParameter("PPhotoFileName", emp.photoFileName));
                    myCommand.ExecuteReader();
                    mycon.Close();
                }
            }
            return new JsonResult("New Employee Inserted Successfully.");
        }
    }
}
