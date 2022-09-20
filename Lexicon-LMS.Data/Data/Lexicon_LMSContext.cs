using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lexicon_LMS.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Lexicon_LMS.Data
{
    public class Lexicon_LMSContext : IdentityDbContext<User, IdentityRole, string>
    {
        public Lexicon_LMSContext (DbContextOptions<Lexicon_LMSContext> options)
            : base(options)
        {
        }

        public DbSet<Activity> Activity { get; set; } = default!;

        public DbSet<ActivityType>? ActivityType { get; set; }

        public DbSet<Course>? Course { get; set; }

        public DbSet<Document>? Document { get; set; }

        public DbSet<Module>? Module { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Cascade delete Course -< Modules
            builder.Entity<Module>()
                .HasOne(p => p.Course)
                .WithMany(b => b.Modules)
                .OnDelete(DeleteBehavior.Cascade);

            //Cascade delete Module -< Ativities
            builder.Entity<Activity>()
                .HasOne(p => p.Module)
                .WithMany(b => b.Activities)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
