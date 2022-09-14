using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class TeacherModuleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Start Time")]
        public DateTime StartTime { get; set; }

        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }
        public bool IsCurrentModule { get; set; }
    }
}
