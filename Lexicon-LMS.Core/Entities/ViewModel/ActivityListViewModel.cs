using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class ActivityListViewModel
    {
        public int Id { get; set; }
        public string ActivityName { get; set; }
        [DisplayName("Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime EndDate { get; set; }
        [DisplayName("Activity Type")]
        public string ActivityTypeActivityTypeName{ get; set; }
        public int ModuleId { get; set; }
        public int ActivityTypeId { get; set; }
        public Document Document { get; set; }
        public string ModuleModulName { get; set; }
        public int CourseId { get; set; }

        public IFormFile UploadedFile { get; set; }
        public string Name { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
