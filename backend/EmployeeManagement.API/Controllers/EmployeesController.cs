using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<CreateEmployeeDto> _createValidator;
        private readonly IValidator<UpdateEmployeeDto> _updateValidator;

        public EmployeesController(
        IEmployeeService employeeService,
        IValidator<CreateEmployeeDto> createValidator,
        IValidator<UpdateEmployeeDto> updateValidator)
        {
            _employeeService = employeeService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET /api/employees?search=juan
        [HttpGet]
        [ProducesResponseType(typeof(List<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EmployeeDto>>> GetAll([FromQuery] string? search, CancellationToken cancellationToken)
        {
            var employees = await _employeeService.GetAllAsync(search, cancellationToken);
            return Ok(employees);
        }

        // GET /api/employees/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetByIdAsync(id, cancellationToken);
            return Ok(employee);
        }

        // POST /api/employees
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDto dto, CancellationToken cancellationToken)
        {
            await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);

            var created = await _employeeService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);
        }

        // PUT /api/employees/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] UpdateEmployeeDto dto, CancellationToken cancellationToken)
        {
            await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);

            var updated = await _employeeService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }
    }
}
