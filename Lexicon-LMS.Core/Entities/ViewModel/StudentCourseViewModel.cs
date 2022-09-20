using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class StudentCourseViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => FirstName + LastName;
        public string ImagePicture { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public int? CourseId { get; set; }

        [DisplayName("Course Name")]
        public string CourseCourseName { get; set; } = string.Empty;

        [StringLength(200)]
        [DisplayName("Description")]
        public string CourseDescription { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime CourseStartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime CourseEndDate { get; set; }

        public ICollection<User> CourseUsers { get; set; } = new List<User>();
        public ICollection<ModuleViewModel> CourseModules { get; set; } = new List<ModuleViewModel>();
        public ICollection<DocumentViewModel> Documents { get; set; } = new List<DocumentViewModel>();
    }
}
