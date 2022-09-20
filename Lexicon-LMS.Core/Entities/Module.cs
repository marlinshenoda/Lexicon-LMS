using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS.Core.Entities
{
#nullable disable
    public class Module
    {
        public int Id { get; set; }

        [DisplayName ("Modul Name")]
        public string ModulName { get; set; } = string.Empty;

        [StringLength(200)]
        [DisplayName ("Description")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }


        //Nav Prop
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public Course Course { get; set; }
        
        //Fk
        public int CourseId { get; set; }
    }

    
}