
using Bogus;
using Bogus.DataSets;
using FluentAssertions.Common;
using Lexicon_LMS.Core.Entities;
using Microsoft.AspNetCore.Identity;
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
        private static RoleManager<IdentityRole> roleManager;
        private static UserManager<User> userManager;
        public static async Task SeedTheData(Lexicon_LMSContext context, IServiceProvider services, IServiceProvider service)
        {
            if (context == null) throw new ArgumentNullException(nameof(db));
            db = context;
            if(db.Users.Count()< 1)
            {
                roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                userManager = services.GetRequiredService<UserManager<User>>();


                var roleNames = new[] { "Student", "Teacher" };

                await AddRolesAsync(roleNames);   

                var teacherEmail = "teacher@lms.se";
                string adminPW = "TeacherPW123";


                var admin = await AddAdminAsync(teacherEmail, adminPW);

                await AddToRolesAsync(admin, roleNames);
            }

            if (db.Course.Count() < 1)
            {
                var Courses = GetCourses();
                await db.AddRangeAsync(Courses);
            }

            await db.SaveChangesAsync();
        }

        private static async Task<User> AddAdminAsync(string teacherEmail, string adminPW)
        {
            var found = await userManager.FindByEmailAsync(teacherEmail);

            if (found != null) return null!;

            var admin = new User
            {
                FirstName = "Adam",
                LastName = "Aven",
                UserName = teacherEmail,
                Email = teacherEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return admin;
        }
        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task AddToRolesAsync(User teatcher, string[] roleNames)
        {
            foreach (var role in roleNames)
            {
                if (await userManager.IsInRoleAsync(teatcher, role)) continue;
                var result = await userManager.AddToRoleAsync(teatcher, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static IEnumerable<Course> GetCourses()
        {
            var faker = new Faker("sv");

            var Courses = new List<Course>();

            for (int i = 0; i < 10; i++)
            {
                var title = faker.Hacker.Verb();
                var temp = new Course
                {
                    CourseName = title,
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5)),
                    EndDate = DateTime.Now.AddDays(faker.Random.Int(10, 15)),
                    Description = "Denna kursen har lektioner innom " + title,
                    Modules = GetModules(title),
                    Documents = GetDocuments(title),
                    //Users = GetStudnetUsers(),
                };

                Courses.Add(temp);
            }

            return Courses;
        }

        private static ICollection<Document> GetDocuments(string coursetitle)
        {
            var faker = new Faker("sv");
            var Documents = new List<Document>();
            int num = faker.Random.Int(2, 7);

            for (int i = 0; i < num; i++)
            {
                var title = faker.Hacker.Verb();
                var temp = new Document
                {
                    DocumentName = coursetitle +"-"+ title,
                    Description = "Detta documentet har info innom " + title,
                    //FilePath =""
                    IsFinished = false
                };

                Documents.Add(temp);
            }

            return Documents;
        }

        private static ICollection<Module> GetModules(string coursetitle)
        {
            var faker = new Faker("sv");
            var Modules = new List<Module>();
            int num = faker.Random.Int(3, 6);

            for (int i = 0; i < num; i++)
            {
                var title = faker.Hacker.Verb();
                var temp = new Module
                {
                    ModulName = coursetitle + "-" + title,
                    Description = "Dena module har info innom " + title,
                    Activities = GetActivities(title),
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(10, 15)),
                };

                Modules.Add(temp);
            }

            return Modules;
        }

        private static ICollection<Activity> GetActivities(string mdouletitle)
        {
            var faker = new Faker("sv");
            var Activities = new List<Activity>();
            int num = faker.Random.Int(1, 5);

            for (int i = 0; i < num; i++)
            {
                var title = faker.Hacker.Verb();
                var temp = new Activity
                {
                    ActivityName = mdouletitle + "-" + title,
                    Description = "Dena aktivitet har information för " + title,
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(10, 15)),
                };

                Activities.Add(temp);
            }

            return Activities;
        }
    }
}