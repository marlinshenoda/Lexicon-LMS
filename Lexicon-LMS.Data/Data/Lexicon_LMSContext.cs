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
        public Lexicon_LMSContext(DbContextOptions<Lexicon_LMSContext> options)
            : base(options)
        {
        }

        public DbSet<Activity> Activity { get; set; } = default!;

        public DbSet<ActivityType>? ActivityType { get; set; }

        public DbSet<Course>? Course { get; set; }

        public DbSet<Document>? Document { get; set; }

        public DbSet<Module>? Module { get; set; }
        public DbSet<User>? User { get; set; }
    }
}
