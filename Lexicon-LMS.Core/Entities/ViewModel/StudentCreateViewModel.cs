using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class StudentCreateViewModel : StudentViewModel
    {
        public List<SelectListItem>? AvailableCourses { get; set; }
        public string Password { get; set; }
    }
}
