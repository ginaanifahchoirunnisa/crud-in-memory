using Swashbuckle.AspNetCore.Annotations;

namespace CRUD_employee_operation.DTO
{
    /*This DTO is using for response GET all data employees and getById with the BirthDate format is dd-MMM-yyyy(ex : 12-aug-2001) 
     * format default is number -> DateOnly
     */
    public class EmployeeDTO
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }

        [SwaggerSchema(Description = "Date format: dd-mm-yyyy")]
        public string BirthDate { get; set; }
    }
}
