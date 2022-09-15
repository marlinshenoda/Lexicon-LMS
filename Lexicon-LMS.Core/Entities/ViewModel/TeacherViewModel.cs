using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class TeacherViewModel
    {
        public IEnumerable<TeacherAssignmentListViewModel> AssignmentList { get; set; }

        public IEnumerable<TeacherModuleViewModel> ModuleList { get; set; }
        public IEnumerable<ActivityListViewModel> ActivityList { get; set; }
        public TeacherCurrentViewModel Current { get; set; }

    }
}
