using Swashbuckle.AspNetCore.Annotations;

namespace CRUD_employee_operation.DTO
{
    public class CreateUpdateEmployeeDTO
    {
        public string FullName { get; set; }

        [SwaggerSchema(Description = "Date format: dd-MM-yyyy")]
        public string BirthDate { get; set; }
    }
}
