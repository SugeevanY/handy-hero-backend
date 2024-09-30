using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using HandyHero.Common;
using HandyHero.Data;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HandyHero.Services.Repository
{
    public class FieldWorkerRepository : IFieldWorker
    {
        private ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;

       

        public FieldWorkerRepository(ApplicationDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }
        public bool acceptProject(Project project)
        {
            try
            {
                project.ProjectStatus = 1;
                _context.Project.Update(project);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

       

        public bool createComplaint(Complaint complaint)
        {
            try
            {
                ComplaintRepository comp = new ComplaintRepository(_context);
                comp.createComplaint(complaint);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public FieldWorker findFieldWorkerById(int Id)
        {
            FieldWorker worker = _context.FieldWorker.FirstOrDefault(f => f.Id == Id);
            return worker;
        }

        public FieldWorker addWorkerHired(int Id)
        {
            FieldWorker worker =_context.FieldWorker.FirstOrDefault(f => f.Id == Id);
            worker.isHired = true;
            _context.SaveChangesAsync();
            return worker;  
        }

       

        public FieldWorker GetFieldWorkerByEmail(string email)
        {
            return _context.FieldWorker.FirstOrDefault(x => x.Email == email);
        }

        public ICollection<Project> GetProjects(int id)
        {
            var projects = _context.Project.Where(p => p.ProjectWorker == id).ToList();
            return projects;
        }

      

        public bool isUser(string email)
        {
            throw new NotImplementedException();
        }

        public bool login(string email, string password)
        {
            var fieldWorker = _context.FieldWorker.FirstOrDefault(a => a.Email == email);

            if (fieldWorker == null)
            {
                return false;
            }

            PasswordHash ph = new PasswordHash();

            bool isValidPassword = ph.VerifyPassword(password, fieldWorker.Password);
            Console.WriteLine($"Password Validation : {isValidPassword}");

            if (isValidPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool logout()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool rejectProject(Project project)
        {
            try
            {
                project.ProjectStatus = -1;
                _context.Project.Update(project);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

       
        public bool signUp(FieldWorker fieldWorker)
        {
            try
            {
                _context.FieldWorker.Add(fieldWorker);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string UploadFile(IFormFile file)
        {
            try
            {
                // Check if the file exists
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentNullException(nameof(file), "No file uploaded");
                }

                // Upload file to Cloudinary
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };

                var uploadResult = _cloudinary.Upload(uploadParams);

                // Return the URL of the uploaded file
                return uploadResult.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }

        public async Task<FieldWorker> GetFieldWorkerAsync(int id)
        {
            return await _context.FieldWorker.FindAsync(id);
        }

        public async Task UpdateFieldWorkerAsync(FieldWorker fieldworker)
        {
            _context.FieldWorker.Update(fieldworker);
            await _context.SaveChangesAsync();
        }

        public string[] UploadFiles(IFormFile[] files)
        {
            try
            {
                if (files == null || files.Length == 0)
                {
                    throw new ArgumentNullException(nameof(files), "No files uploaded");
                }

                var uploadedUrls = new List<string>();

                foreach (var file in files)
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream())
                    };

                    var uploadResult = _cloudinary.Upload(uploadParams);
                    uploadedUrls.Add(uploadResult.Uri.AbsoluteUri);
                }

                return uploadedUrls.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
    }
}
