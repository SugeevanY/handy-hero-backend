using CloudinaryDotNet;
using HandyHero.Common;
using HandyHero.Data;
using HandyHero.DTO;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HandyHero.Services.Repository
{
    public class AdminRepository : IAdmin
    {
        private ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly Cloudinary _cloudinary;
        public AdminRepository(ApplicationDbContext context, IConfiguration config, Cloudinary cloudinary)
        {
            _context = context;
            _config = config;
            _cloudinary = cloudinary;
        }

        public bool BlockFieldWorker(FieldWorker fieldWorker)
        {
            try
            {
                _context.FieldWorker.Update(fieldWorker);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool CreateAdmin(Admin admin)
        {
            try
            {
                PasswordHash ph = new PasswordHash();
                admin.password = ph.HashPassword(admin.password);
                _context.Admin.Add(admin);
                _context.SaveChanges(true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

         public IEnumerable<Customer> GetCustomers()
         {
             return _context.Customer;
         }

         public IEnumerable<FieldWorker> GetFieldWorkers()
         {
             return _context.FieldWorker;
         }
        
        public bool IsAdmin(Admin admin)
        {
            return _context.Admin.Any(a => a.Email == admin.Email);
        }

        public bool Login(string email, string password)
        {
            var admin = _context.Admin.FirstOrDefault(a => a.Email == email);

            if (admin == null)
            {
                return false;
            }
        
            PasswordHash ph = new PasswordHash();

            bool isValidPassword = ph.VerifyPassword(password, admin.password);

            if (isValidPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Logout()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        

        public ClaimsPrincipal validateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if (validatedToken != null)
                {
                    return new ClaimsPrincipal(new ClaimsIdentity(((JwtSecurityToken)validatedToken).Claims));
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public Admin GetAdminByEmail(string Email)
        {
            return _context.Admin.FirstOrDefault(x => x.Email == Email);
        }

        public Admin GetAdminById(int Id)
        {
            return _context.Admin.FirstOrDefault(x => x.Id == Id);
        }

        public bool AcceptFieldWorker(string fieldworkerEmail, int adminId)
         {
             try
             {

                 FieldWorkerRepository _fieldWorker = new FieldWorkerRepository(_context, _cloudinary);
                 FieldWorker fieldWorker = _fieldWorker.GetFieldWorkerByEmail(fieldworkerEmail);
                 if (fieldWorker != null)
                 {
                     fieldWorker.Status = "true";
                     fieldWorker.AcceptOrRejectBy = adminId;
                     _context.FieldWorker.Update(fieldWorker);
                     _context.SaveChanges();
                     return true;
                 }
                 else
                 {

                     return false;
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex);
                 return false;
             }
         }

         public bool RejectFieldWorker(string fieldworkerEmail, int adminId)
         {
             try
             {

                 FieldWorkerRepository _fieldWorker = new FieldWorkerRepository(_context, _cloudinary);
                 FieldWorker fieldWorker = _fieldWorker.GetFieldWorkerByEmail(fieldworkerEmail);
                 if (fieldWorker != null)
                 {
                     fieldWorker.Status = "false";
                     fieldWorker.AcceptOrRejectBy = adminId;
                     _context.FieldWorker.Update(fieldWorker);
                     _context.SaveChanges();
                     return true;
                 }
                 else
                 {

                     return false;
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex);
                 return false;
             }
         }

        

        public List<ComplaintView> gettAllComplaints()
        {
            ComplaintRepository comp = new ComplaintRepository(_context);
            return comp.GetComplaints();
        }
    }

    }



