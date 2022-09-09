using Lexicon_LMS.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lexicon_LMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Activity>? Activities { get; set; }
        public DbSet<ActivityType>? ActivitiesType { get; set; }
        public DbSet<Module>? Modules { get; set; }
        public DbSet<Course>? Courses { get; set; }
        public DbSet<Document>? Documents { get; set; }
    }
}