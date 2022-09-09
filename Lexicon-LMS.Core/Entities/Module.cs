using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS.Core.Entities
{
#nullable disable
    public class Module
    {
        public int Id { get; set; }

        [StringLength(20)]
        [DisplayName ("Module Name")]
        public string ModuleName { get; set; }

        [StringLength(200)]
        [DisplayName ("Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate => StartDate.AddDays(14);


        public ICollection<Activity> User { get; set; } = new List<Activity>();
        public ICollection<Document> Document { get; set; } = new List<Document>();
    }
}