using System.Collections.Generic;

namespace AmatoFluent.Models
{
    public class CvData
    {
        public ContactInfo Contact { get; set; }
        public List<string> Summary { get; set; }
        public List<JobInfo> Jobs { get; set; }
        public List<EducationInfo> Education { get; set; }
        public List<SkillInfo> TechnicalSkills { get; set; }
        public List<ProjectInfo> Projects { get; set; }
        public List<UniversityProjectInfo> UniversityProjects { get; set; }
    }

    public class ContactInfo
    {
        public string Email { get; set; }
        public string LinkedIn { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
    }

    public class JobInfo
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public string Date { get; set; }
        public List<string> Tasks { get; set; }
    }

    public class EducationInfo
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }

    public class SkillInfo
    {
        public string Category { get; set; }
        public string Details { get; set; }
    }

    public class ProjectInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Tasks { get; set; }
    }

    public class UniversityProjectInfo
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
