using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class ModuleEditViewModel
    {
        public int CourseId { get; set; }
        public int? ModuleId { get; set; }
        [DisplayName("Name")]
        public string ModuleName { get; set; }
        [DisplayName("Description")]

        public string ModuleDescription { get; set; }
        [DisplayName("Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime ModuleStartDate { get; set; }
        [DisplayName("End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime ModuleEndDate { get; set; }
    }
}
