using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class DocumentViewModel
    {
        public int Id { get; set; }

        [DisplayName("Document Name")]
        public string DocumentName { get; set; } = string.Empty;
    }
}
