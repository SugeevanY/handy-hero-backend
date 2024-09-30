using System.ComponentModel.DataAnnotations;

namespace HandyHero.DTO
{
    public class ProjectRequest
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectOwner { get; set; }
        public string ProjectWorker { get; set; }
        public string ProjectLocation { get; set; }
        public string ProjectBudget { get; set; }
        public string ProjectDuration { get; set; }
        public string ProjectType { get; set; }
        public string ProjectStatus { get; set; }
    }
}
