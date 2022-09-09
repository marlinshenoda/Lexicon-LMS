using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS.Core.Entities
{
    public class Module
    {
        public int Id { get; set; }

        [DisplayName ("Modul Name")]
        public string ModulName { get; set; }

        [StringLength(200)]
        [DisplayName ("Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        


        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}