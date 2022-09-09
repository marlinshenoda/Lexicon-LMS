using Microsoft.AspNet.Identity.EntityFramework;

namespace Lexicon_LMS.Core.Entities
{
    public class User : IdentityUser
    {
        public String FirstName { get; set; } = String.Empty;
        public String LastName { get; set; } = String.Empty;   
        public String FullName => FirstName + LastName;

        public int CourseId { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        

    }
}