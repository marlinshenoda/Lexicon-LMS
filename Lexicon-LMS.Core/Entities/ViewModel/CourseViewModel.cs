using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [DisplayName("Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [StringLength(200)]
        [DisplayName("Description")]
        public string CourseDescription { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        public IEnumerable<User> Users { get; set; } = new List<User>();
        public IEnumerable<Module> Modules { get; set; } = new List<Module>();
        public IEnumerable<Document> Documents { get; set; } = new List<Document>();

    }
}
