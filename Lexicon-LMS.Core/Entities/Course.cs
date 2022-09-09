using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities
{
#nullable disable
    public class Course
    {
        public int Id { get; set; }
        
        [DisplayName ("Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [StringLength(200)]
        [DisplayName("Description")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();


    }
}
