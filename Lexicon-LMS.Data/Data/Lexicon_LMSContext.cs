using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lexicon_LMS.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Lexicon_LMS.Data
{
    public class Lexicon_LMSContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Activity> Activity { get; set; } = default!;

        public DbSet<ActivityType>? ActivityType { get; set; }

        public DbSet<Course>? Course { get; set; }

        public DbSet<Document>? Document { get; set; }

        public DbSet<Lexicon_LMS.Core.Entities.Module>? Module { get; set; }
        public DbSet<Lexicon_LMS.Core.Entities.User>? User { get; set; }
    }
}
