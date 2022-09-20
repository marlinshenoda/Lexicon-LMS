using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Lexicon_LMS.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;   
        public string FullName => FirstName + LastName;
        public string ImagePicture { get; set; } = string.Empty;

        //Fk
        public int? CourseId { get; set; }

        
        //Nav Prop
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public Course Course { get; set; } = null!;


    }
}   