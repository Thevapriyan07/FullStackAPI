using FullStackAPI.Data;
using FullStackAPI.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly FullStackDbContext _fullStackDbContext;

        public EmployeesController(FullStackDbContext fullStackDbContext)
        {
            _fullStackDbContext = fullStackDbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
          var employees = await  _fullStackDbContext.Employees.ToListAsync();
            return Ok(employees);
        }
        [HttpPost]  
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            employeeRequest.id = Guid.NewGuid();
            await _fullStackDbContext.Employees.AddAsync(employeeRequest);
            await _fullStackDbContext.SaveChangesAsync();
            return Ok(employeeRequest);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute]Guid id)
        {
          var employee =  await _fullStackDbContext.Employees.FirstOrDefaultAsync(x => x.id == id); 
            if(employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult>UpdateEmployee([FromRoute]Guid id, Employee updateEmployeeRequest)
        {
            var employee = await _fullStackDbContext.Employees.FindAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            employee.name = updateEmployeeRequest.name;
            employee.email = updateEmployeeRequest.email;
            employee.salary = updateEmployeeRequest.salary;
            employee.phone = updateEmployeeRequest.phone;
            employee.department = updateEmployeeRequest.department;

           await _fullStackDbContext.SaveChangesAsync();
            return Ok(employee);

        }
    }
}
