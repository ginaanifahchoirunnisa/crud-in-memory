using CRUD_employee_operation.Functionalities;
using System.Text.Json.Serialization;

namespace CRUD_employee_operation.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }


        //[JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly BirthDate { get; set; }
    }
}
