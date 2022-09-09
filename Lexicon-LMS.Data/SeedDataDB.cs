
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Data
{
    public class SeedDataDB
    {
        private static Lexicon_LMSContext db = default;
        public static async Task SeedTheData(Lexicon_LMSContext context, IServiceProvider service)
        {
            if (context == null) throw new ArgumentNullException(nameof(db));

            db = context;



            if (db.Courses.Count() < 1)
            {
                var Courses = GetCourses();
                await db.AddRangeAsync(Courses);
            }

            await db.SaveChangesAsync();
        }
    }
}