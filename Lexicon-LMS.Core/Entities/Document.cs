using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS.Core.Entities
{
#nullable disable
    public class Document
    {
        public int Id { get; set; }

        [DisplayName("Document Name")]
        public string DocumentName { get; set; } = string.Empty;

        [StringLength(200)]
        [DisplayName("Description")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Time Stamp")]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        [StringLength(100)]
        [Display(Name = "User Information")]
        public string FilePath { get; set; }
        public bool? IsFinished { get; set; }
        //Fk
        public string UserId { get; set; }
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }        
        public int? ActivityId { get; set; }


    }
}