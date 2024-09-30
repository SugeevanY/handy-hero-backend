using HandyHero.Data;
using HandyHero.DTO;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;

namespace HandyHero.Services.Repository
{
    public class ProjectRepository : IProject
    {
        private ApplicationDbContext _context;
        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool createProject(ProjectRequest project)
        {
            throw new NotImplementedException();
        }

        public List<Project> getAllProjects()
        {
            throw new NotImplementedException();
        }

        public bool updateProject(int projectId, bool ProjectStatus)
        {
            throw new NotImplementedException();
        }
    }
}
