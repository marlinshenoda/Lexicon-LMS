﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class AssignmentListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public IFormFile UploadedFile { get; set; }

        [DisplayName("End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DateEndDate { get; set; }

        [DisplayName("Finished")]
        public double Finished { get; set; }
    }
}
