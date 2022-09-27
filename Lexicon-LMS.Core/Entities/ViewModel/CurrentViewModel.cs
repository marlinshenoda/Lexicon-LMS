using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class CurrentViewModel
    {
        public Course course { get; set; }
        public ICollection<AssignmentsViewModel> Assignments { get; set; }
        public Module Module { get; set; }


    }
}
