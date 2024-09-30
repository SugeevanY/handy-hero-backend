using System.ComponentModel.DataAnnotations;

namespace HandyHero.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(30)]
        public string ProjectName { get; set; }

        [Required]
        public int ProjectOwner { get; set; }

        [Required]
        public int ProjectWorker { get; set; }
        [Required]
        public string ProjectLocation { get; set; }
        [Required]
        public string ProjectBudget { get; set; }
        [Required]
        public string ProjectDuration { get; set; }
        [Required]
        public string ProjectType { get; set; }
        public int ProjectStatus { get; set; }
    }
}
