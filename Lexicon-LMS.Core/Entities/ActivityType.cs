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
        public int Id { get; set; }
        
        [Required]
        public string ActivityTypeName { get; set; } = string.Empty;

        //Navigation Properties
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

    }
}
