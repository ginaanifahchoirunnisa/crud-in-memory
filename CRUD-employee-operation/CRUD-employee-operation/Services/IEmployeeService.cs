using System.Collections.Generic;
using CRUD_employee_operation.DTO;
using CRUD_employee_operation.Models;

namespace CRUD_employee_operation.Services
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDTO> GetAllEmployees();
        bool isDataDuplicate(CreateUpdateEmployeeDTO createUpdateEmployeeDTO);
        bool isAtLeast17YearsOld(DateOnly birthDate);
        object GetEmployeeById(string employeeId, bool isApi);
        string CreateEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteById(string employeeId);
    }
}
