using Lexicon_LMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class TeacherCurrentViewModel
    {
        public Lexicon_LMS.Core.Entities.Activity Activity { get; set; }
        public Course Course { get; set; }
        public ICollection<TeacherAssignmentsViewModel> Assignments { get; set; }
        public Module Module { get; set; }
        public string Finished { get; set; }
        public DateTime DueDate { get; set; }
    }
}
