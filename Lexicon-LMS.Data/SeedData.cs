using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS.Data
{
    public static class SeedData
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider;
                var db = service.GetRequiredService<Lexicon_LMSContext>();
                try
                {
                    await SeedDataDB.SeedTheData(db, service);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return app;
        }
    }
}
