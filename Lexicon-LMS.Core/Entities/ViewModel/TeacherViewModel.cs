using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class TeacherViewModel
    {

        public IEnumerable<AssignmentListViewModel> AssignmentList { get; set; }

        public IEnumerable<ModuleViewModel> ModuleList { get; set; }
        public IEnumerable<ActivityListViewModel> ActivityList { get; set; }

        public CurrentViewModel Current { get; set; }
        public int CourseId { get; set; }

    }
}
