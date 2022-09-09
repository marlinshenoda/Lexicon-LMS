using Microsoft.AspNet.Identity.EntityFramework;

namespace Lexicon_LMS.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;   
        public string FullName => FirstName + LastName;

        //Fk
        public int? CourseId { get; set; }
        
        //Nav Prop
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        

    }
}   