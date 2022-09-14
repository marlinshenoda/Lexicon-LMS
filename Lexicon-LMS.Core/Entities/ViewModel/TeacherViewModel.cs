using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class TeacherViewModel
    {
        public IEnumerable<Module> ModuleList { get; set; }
        public IEnumerable<Activity> ActivityList { get; set; }
    }
}
