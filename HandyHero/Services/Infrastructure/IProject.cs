using HandyHero.DTO;
using HandyHero.Models;


namespace HandyHero.Services.Infrastructure
{
    public interface IProject
    {
        public bool createProject(ProjectRequest project);

        public bool updateProject(int projectId, bool ProjectStatus);

        public List<Project> getAllProjects();
    }
}
