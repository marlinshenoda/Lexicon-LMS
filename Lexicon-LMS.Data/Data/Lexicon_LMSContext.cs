using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lexicon_LMS.Core.Entities;

namespace Lexicon_LMS.Data
{
    public class Lexicon_LMSContext : DbContext
    {
        public Lexicon_LMSContext (DbContextOptions<Lexicon_LMSContext> options)
            : base(options)
        {
        }

        public DbSet<Lexicon_LMS.Core.Entities.Activity> Activity { get; set; } = default!;

        public DbSet<Lexicon_LMS.Core.Entities.ActivityType>? ActivityType { get; set; }

        public DbSet<Lexicon_LMS.Core.Entities.Course>? Course { get; set; }

        public DbSet<Lexicon_LMS.Core.Entities.Document>? Document { get; set; }

        public DbSet<Lexicon_LMS.Core.Entities.Module>? Module { get; set; }
    }
}
