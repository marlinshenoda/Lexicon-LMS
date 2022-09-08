using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities
{
#nullable disable
    public class ActivityType
    {
        public int ActivityTypeId { get; set; }
        [Required]
        [StringLength(20)]
        public string ActivityTypeName { get; set; } = String.Empty;

        //Navigation Properties
        public ICollection<Activity> Activitys { get; set; } = new List<Activity>();

    }
}
