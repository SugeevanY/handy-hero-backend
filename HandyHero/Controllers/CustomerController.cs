using HandyHero.DTO;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;

namespace HandyHero.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        ICustomer _customer;
        private readonly IConfiguration _config;
       
        public CustomerController(ICustomer customer, IConfiguration config)
        {
            _customer = customer;
            _config = config;
            
        }

        [HttpPost("create")]
        public IActionResult createAccount([FromBody] Customer customer)
        {
            if (ModelState.IsValid)
            {

                var result = _customer.SignUp(customer);
                if (result)
                {
                    Console.WriteLine("registration success");
                    return Ok(result);

                }
                else
                {
                    Console.WriteLine("registration failed");
                    return BadRequest(result);
                }
            }
            else
            {
                return BadRequest("ModelError");
            }
        }

        [HttpPost("login")]
        public IActionResult login([FromBody] Customer customer)
        {
            var email = customer.Email;
            var password = customer.Password;

            var result = _customer.Login(email, password);
            if (result)
            {
                var LoggedInCustomer = _customer.getCustomerByMail(email);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, email),  // Email claim
                new(ClaimTypes.NameIdentifier, LoggedInCustomer.Id.ToString()),  // Admin ID claim
                new(ClaimTypes.GivenName, LoggedInCustomer.Name)  // Admin name claim
                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { token = tokenHandler.WriteToken(token), Id = LoggedInCustomer.Id });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("requestWork")]
        public IActionResult requestWork([FromForm] ProjectRequest projectRequest)
        {
            try
            {
                Project project = new Project
                {
                    ProjectId = int.Parse(projectRequest.ProjectId),
                    ProjectName = projectRequest.ProjectName,
                    ProjectOwner = int.Parse(projectRequest.ProjectOwner),
                    ProjectWorker = int.Parse(projectRequest.ProjectWorker),
                    ProjectLocation = projectRequest.ProjectLocation,
                    ProjectBudget = projectRequest.ProjectBudget,
                    ProjectDuration = projectRequest.ProjectDuration,
                    ProjectType = projectRequest.ProjectType,
                    ProjectStatus = int.Parse(projectRequest.ProjectStatus)
                };

                var result = _customer.createProject(project);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                // Log exception details (ex) if necessary
                return BadRequest("An error occurred while processing the request.");
            }
        }


        [HttpGet("myProject")]
        public IActionResult getMyProjects([FromQuery(Name = "id")] int Id)
        {
            var projects = _customer.getMyProject(Id);

            if (projects == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(projects);
            }
        }

        [HttpPost("complaint")]
        public IActionResult makeComplaint([FromBody] ComplaintRequest complaint)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Invalid model state");
                return BadRequest("Error in model");
            }

            var customer = _customer.findCustomerById(complaint.complanantId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found");
                Console.WriteLine(complaint.complanantId.GetType());
                Console.WriteLine(complaint.complanantId.ToString());
                return NotFound("Customer not found");
            }

            var complainantEmail = customer.Email;

            Complaint complaint1 = new Complaint();
            complaint1.Complainant = complainantEmail;
            complaint1.Accused = complaint.accused;
            complaint1.ComplaintMessage = complaint.complaint;
            complaint1.TimeStamp = DateTime.Now;

            var result = _customer.createComplaint(complaint1);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                Console.WriteLine("Something went wrong while creating complaint");
                return BadRequest("Error in create complaint");
            }
        }

       


    }
}
