using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Microsoft.AspNetCore;


namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class StudentCreateViewModel : StudentViewModel
    {
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>? AvailableCourses { get; set; }
        public string Password { get; set; }
    }
}
