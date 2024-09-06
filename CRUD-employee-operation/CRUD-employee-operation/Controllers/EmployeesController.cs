using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CRUD_employee_operation.Models;
using System.Text.RegularExpressions;
using CRUD_employee_operation.DTO;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using CRUD_employee_operation.Services;

namespace CRUD_employee_operation.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeRepository)
        {
            _employeeService = employeeRepository;
        }

        /* Get All Employees data
         * GET: api/employees*/
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeDTO>> GetAllEmployees()
        {
            Console.WriteLine("Start to get all data employees");

            try
            {
                int totalEmployees = _employeeService.GetAllEmployees().Count();
                var employees = _employeeService.GetAllEmployees();

                Console.WriteLine($"Count of employees data :{totalEmployees}"); //print to console total of employees data

                if (employees == null || totalEmployees < 1)
                {
                    return NotFound("Employees data is empty ");
                }
                return Ok(employees);
            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /* GET: api/employees/{id} */
        [HttpGet("{employeeId}")]
        public ActionResult GetEmployeeById(string employeeId)
        {
            Console.WriteLine($"Get data employee with id = {employeeId}");
            try
            {
                object employee = _employeeService.GetEmployeeById(employeeId, true);
                if (employee == null)
                {
                    return NotFound($"Data with id = {employeeId} is not found");
                }
                return Ok(employee);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


        /* POST: api/employees
         * format birthdate : dd-mm-yyyy
         * 
         */
        [HttpPost]
        public ActionResult CreateEmployee(CreateUpdateEmployeeDTO createEmployeeDto)
        {
           
            Console.WriteLine($"Create Employee Started");

            /*for duplicate data handling*/
            bool isDataDuplicate = _employeeService.isDataDuplicate(createEmployeeDto);

            Console.WriteLine($"status data duplicate = {isDataDuplicate}");

         

            try
            {
                
                if (isDataDuplicate)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "data is exist, data that will input cannot be duplicate");
                }
                else {
                    if (createEmployeeDto == null)
                    {
                        Console.WriteLine("data createEmployeeDTO is null");
                        return BadRequest("Body request cannot be null");
                    }

                    if (createEmployeeDto.FullName == null || createEmployeeDto.FullName == "" || createEmployeeDto.FullName == " ")
                    {
                        Console.WriteLine("data fullName is null");
                        return StatusCode(StatusCodes.Status500InternalServerError, "Data fullName cannot be empty");
                    }

                    if (createEmployeeDto.BirthDate != null)
                    {
                        

                        if (!DateOnly.TryParseExact(createEmployeeDto.BirthDate, "dd-MM-yyyy", out var birthDate))
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, "Invalid date format. Please use DD-MM-YYYY.");

                        }
                    
                        else {

                            bool checkAge = _employeeService.isAtLeast17YearsOld(DateOnly.ParseExact(createEmployeeDto.BirthDate, "dd-MM-yyyy", null));
                            Console.WriteLine($"employee is 17 or more old {checkAge}");

                            if (!checkAge) { return StatusCode(StatusCodes.Status500InternalServerError, "Employee age must >= 17 years old"); }
                        }
                    }
                    else
                    {
                        return BadRequest("BirthDate cannot be null");

                    }

                    var newEmployee = new Employee
                    {
                        EmployeeId = " ",
                        FullName = createEmployeeDto.FullName,
                        BirthDate = DateOnly.ParseExact(createEmployeeDto.BirthDate, "dd-MM-yyyy", null)
                    };

                    string newEmployeeId = _employeeService.CreateEmployee(newEmployee);

                    // Ensure the route parameter name matches the expected name in GetEmployeeById method
                    //return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeId }, newEmployee);
                    return Ok(_employeeService.GetEmployeeById(newEmployeeId, true));
                }


            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }




        /* PUT: api/employees/{id} 
         Birthdate format dd-mm-yyyy see desc on swagger
        employee age must >= 17
         */
        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(string employeeId, CreateUpdateEmployeeDTO employee)
        {
            try
            {
                var existingEmployee = _employeeService.GetEmployeeById(employeeId, false);


                if (existingEmployee == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Data employee with id = {employeeId} is not exist");
                }

                var employeeRes = (Employee)existingEmployee;

                Console.WriteLine($"employeeRes :{employeeRes}");

                if (employee == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "data cannot updated");
                }

                var newEmployee = new Employee();

                /*Set the data will be updated*/
                newEmployee.EmployeeId = employeeId;


                /*Set birthDate*/
                if (employee.BirthDate != null)
                {
                    if (!DateOnly.TryParseExact(employee.BirthDate, "dd-MM-yyyy", out var birthDate))
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Invalid date format. Please use DD-MM-YYYY.");

                    }
                    else
                    {
                        bool checkAge = _employeeService.isAtLeast17YearsOld(DateOnly.ParseExact(employee.BirthDate, "dd-MM-yyyy", null));
                        Console.WriteLine($"employee is 17 or more old {checkAge}");

                        if (!checkAge) { return StatusCode(StatusCodes.Status500InternalServerError, "Employee age must >= 17 years old"); }
                        newEmployee.BirthDate = DateOnly.ParseExact(employee.BirthDate, "dd-MM-yyyy", null);
                    }

                }
                else
                {

                    newEmployee.BirthDate = employeeRes.BirthDate;
                }

                /*set FullName
                 * if from payload with this condition
                 * the data will set to default data (before updated)
                 */
                if (employee.FullName == null || employee.FullName == "" || employee.FullName == " ")
                {
                    newEmployee.FullName = employeeRes.FullName;
                 
                }
                else {

                    newEmployee.FullName = employee.FullName;

                }

                _employeeService.UpdateEmployee(newEmployee);
                return Ok(_employeeService.GetEmployeeById(employeeId, true));

            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        /* DELETE: api/employees/{id}*/
        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(string employeeId)
        {
            Console.WriteLine("Delete employee started");
            try
            {
                var employee = _employeeService.GetEmployeeById(employeeId, false);
                if (employee == null)
                {
                    return NotFound("Data is not exist");
                }
                _employeeService.DeleteById(employeeId);
                Console.WriteLine($"Data employee with id = {employeeId} has been deleted");
                return NoContent();
            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


    }
}
