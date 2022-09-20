using Microsoft.AspNetCore.Http;


namespace Lexicon_LMS.Core.Entities.ViewModel
{
    public class UpLoadDocumentViewModel
    {
        public IFormFile UploadedFile { get; set; }
        public string Name { get; set; } 
    }
}
