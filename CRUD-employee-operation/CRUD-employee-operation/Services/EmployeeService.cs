using System.Collections.Generic;
using System.Linq;
using CRUD_employee_operation.DTO;
using CRUD_employee_operation.Models;

namespace CRUD_employee_operation.Services
{
    public class EmployeeService : IEmployeeService
    {
        // Static data store
        private static readonly List<Employee> employees = new List<Employee>
        {
            /*comment this all data and let the data is only {} to test GET API allemployees if the data condition is null or empty*/
            new Employee { EmployeeId = "1001", FullName = "Yoegi", BirthDate = new DateOnly(1998, 9, 17) },
            new Employee { EmployeeId = "1002", FullName = "Hana", BirthDate = new DateOnly(1976, 8, 1) },
            new Employee { EmployeeId = "1003", FullName = "Jefry Nichol", BirthDate = new DateOnly(1995, 7, 10) },
            new Employee { EmployeeId = "1004", FullName = "Taylor Swift Rahana", BirthDate = new DateOnly(1991, 12, 11) },
            new Employee { EmployeeId = "1005", FullName = "Shibun Nikeya", BirthDate = new DateOnly(2001, 3, 15) }
        };

        // Returns all employees
        public IEnumerable<EmployeeDTO> GetAllEmployees() {
            return employees.Select(employees => new EmployeeDTO
            {
                EmployeeId = employees.EmployeeId,
                FullName = employees.FullName,
                BirthDate = employees.BirthDate.ToString("dd-MMM-yyyy")
            });
        }

        /*To check if the data duplicate or not, if true, the data is duplicate*/
        public bool isDataDuplicate(CreateUpdateEmployeeDTO employee)
        {
            // Convert the FullName to lowercase for case-insensitive comparison
            var normalizedFullName = employee.FullName.ToLower();
            var employeeBirthDate = DateOnly.ParseExact(employee.BirthDate, "dd-MM-yyyy", null);

            // Check if there exists any employee with the same FullName and BirthDate
            return employees.Any(e =>
                e.FullName.ToLower() == normalizedFullName &&
                e.BirthDate == employeeBirthDate);
        }


        /*
         * function with generic type
         * using oop concept
         * with bool isApi as conditional
         * if isApi is true, response with employeesDTO format, else response with employees model format
         */
        public object GetEmployeeById(string employeeId, bool isApi)
        {
            Employee employee = employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                return null;
            }

            if (!isApi)
            {
                return employee;
            }
            else
            {
                EmployeeDTO employeeDTO = new EmployeeDTO
                {
                    EmployeeId = employee.EmployeeId,
                    FullName = employee.FullName,
                    BirthDate = employee.BirthDate.ToString("dd-MMM-yyyy") 
                };

                return employeeDTO;
            }


        }

        public bool isAtLeast17YearsOld(DateOnly birthDate) {
            DateOnly currDate = DateOnly.FromDateTime(DateTime.Today);
            
            int age = currDate.Year - birthDate.Year;

            if (birthDate > currDate.AddYears(-age)) { age--; }


            return age >= 17;

        }


        // Add a new employee
        public string CreateEmployee(Employee employee)
        {
            // generate employee id
            int maxId = employees.Select(e => int.Parse(e.EmployeeId)).Max();
            employee.EmployeeId = (maxId + 1).ToString("D4"); 
            employees.Add(employee);

            return employee.EmployeeId;
        }

   
        public void UpdateEmployee(Employee employee)
        {
            var index = employees.FindIndex(e => e.EmployeeId == employee.EmployeeId);
            if (index >= 0)
            {
                employees[index] = employee;
            }
        }

        /* Delete an employee by employeeId*/
        public void DeleteById(string employeeId)
        {
            var employee = employees.FirstOrDefault(e => e.EmployeeId == employeeId);
            if (employee != null)
            {
                employees.Remove(employee);
            }
        }

    }
}
