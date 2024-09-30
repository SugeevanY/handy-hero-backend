using HandyHero.Common;
using HandyHero.DTO;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HandyHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldWorkerController : ControllerBase
    {
        IFieldWorker _fieldWorker;
        private IConfiguration _config;
        public FieldWorkerController(IFieldWorker fieldWorker, IConfiguration config)
        {
            _fieldWorker = fieldWorker;
            _config = config;
        }

        // In your FieldWorkerController

        [HttpPost("createAccount")]
        public IActionResult RequestAccount([FromForm] FieldWorker fieldWorker, IFormFile[] certificates, IFormFile[] experienceLetters, IFormFile NIC, IFormFile profile)
        {
            if (ModelState.IsValid)
            {
                PasswordHash ph = new PasswordHash();
                var Password = ph.HashPassword(fieldWorker.Password);
                Console.WriteLine(Password);
                fieldWorker.Password = Password;
                Console.WriteLine(fieldWorker.Password);

                // Upload certificates and experience letters
                fieldWorker.Certificates = _fieldWorker.UploadFiles(certificates);
                fieldWorker.ExperienceLetter = _fieldWorker.UploadFiles(experienceLetters);


                fieldWorker.NIC = _fieldWorker.UploadFile(NIC);
                fieldWorker.ProfileImage = _fieldWorker.UploadFile(profile);

                var result = _fieldWorker.signUp(fieldWorker);

                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("signup failed");
                }
            }
            else
            {
                return BadRequest("Model failed");
            }
        }


        [HttpPost("login")]
        public IActionResult login([FromBody] LoginRequest fieldWorker)
        {
            var email = fieldWorker.Email;
            var password = fieldWorker.Password;

            var result = _fieldWorker.login(email, password);

            if (!result)
            {
                return Unauthorized($"Username Password Incorrect {result}");
            }

            var loggedInWorker = _fieldWorker.GetFieldWorkerByEmail(email);

            // Authentication successful, generate JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, loggedInWorker.Email),  // Email claim
            new Claim(ClaimTypes.NameIdentifier, loggedInWorker.Id.ToString()),  // Field worker ID claim
            new Claim(ClaimTypes.GivenName, loggedInWorker.Name)  // Field worker name claim
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token), Id = loggedInWorker.Id });
        }

        [HttpPatch("acceptProject")]
        public IActionResult acceptProject(Project project)
        {

            var result = _fieldWorker.acceptProject(project);
            if (result)
            {
                return Ok("Project Accepted");
            }
            else
            {
                return BadRequest("Project Accept failed");
            }


        }

        [HttpPatch("rejectProject")]
        public IActionResult rejectProject(Project project)
        {
            var result = _fieldWorker.rejectProject(project);
            if (result)
            {
                return Ok("Project Rejected");
            }
            else
            {
                return BadRequest("Project reject failed");
            }
        }

        [HttpGet("projects")]
        public IActionResult getProject([FromQuery(Name = "id")] int Id)
        {
            var projects = _fieldWorker.GetProjects(Id);
            if (projects != null)
            {
                return Ok(projects);
            }
            else
            {
                return BadRequest("There is no project");
            }
        }

        [HttpPut("Hire")]
        public IActionResult getHiredUser([FromQuery(Name = "id")] int id)
        {


            var obj= _fieldWorker.addWorkerHired(id);

            return Ok(obj);
         


        }

        [HttpGet("Request")]
        public IActionResult getRequestDetails(int id) 
        {
            return Ok();
        }

       


        [HttpPost("complaint")]
        public IActionResult makeComplaint([FromBody] ComplaintRequest complaint)
        {
            Console.WriteLine("FieldWorker Complaint");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Invalid model state");
                return BadRequest("Error in model");
            }

            var fieldWorker = _fieldWorker.findFieldWorkerById(complaint.complanantId);
            if (fieldWorker == null)
            {
                Console.WriteLine("Field Worker not found");
                Console.WriteLine(complaint.complanantId.GetType());
                Console.WriteLine(complaint.complanantId.ToString());
                return NotFound("FieldWorker not found");
            }

            var complainantEmail = fieldWorker.Email;

            Complaint complaint1 = new Complaint();
            complaint1.Complainant = complainantEmail;
            complaint1.Accused = complaint.accused;
            complaint1.ComplaintMessage = complaint.complaint;
            complaint1.TimeStamp = DateTime.Now;

            var result = _fieldWorker.createComplaint(complaint1);
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
