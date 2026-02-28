using System.Collections.Generic;

namespace AmatoFluent.Models
{
    public class CvData
    {
        public ContactInfo Contact { get; set; } = new();
        public List<string> Summary { get; set; } = new();
        public List<JobInfo> Jobs { get; set; } = new();
        public List<EducationInfo> Education { get; set; } = new();
        public List<SkillInfo> TechnicalSkills { get; set; } = new();
        public List<ProjectInfo> Projects { get; set; } = new();
        public List<UniversityProjectInfo> UniversityProjects { get; set; } = new();
    }

    public class ContactInfo
    {
        public string Email { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
    }

    public class JobInfo
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public List<string> Tasks { get; set; } = new();
    }

    public class EducationInfo
    {
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class SkillInfo
    {
        public string Category { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public class ProjectInfo
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Tasks { get; set; } = new();
    }

    public class UniversityProjectInfo
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
