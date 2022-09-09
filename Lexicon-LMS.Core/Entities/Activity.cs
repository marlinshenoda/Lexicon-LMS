using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lexicon_LMS.Core.Entities
{
#nullable disable
    public class Activity
    {
        public int Id { get; set; }   
        
        [Required]
        [DisplayName("Activity Name")]
        public string ActivityName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Description")] 
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        //FK
        public int ModuleId { get; set; }
        public int ActivityTypeId { get; set; }

        //Navigation Properties
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ActivityType ActivityType { get; set; }
        public Module Module { get; set; }

    }
}
