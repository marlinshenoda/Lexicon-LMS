
using Bogus;
using Bogus.DataSets;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Tracing;
using Lexicon_LMS.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public static async Task SeedTheData(Lexicon_LMSContext context, IServiceProvider services)
        {
            if (context == null) throw new ArgumentNullException(nameof(db));
            db = context;
            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            userManager = services.GetRequiredService<UserManager<User>>();


            if (db.Roles.Count() < 1)
            {
                var roleNames = new[] { "Student", "Teacher" };

                await AddRolesAsync(roleNames);
            }
            var teachers = userManager.GetUsersInRoleAsync("Teacher").Result.ToList();
            if (teachers.Count() < 1)
            {        
                var teacherEmail = "teacher@lms.se";
                string adminPW = "TeacherPW123!";


                var teacher = await AddAdminAsync(teacherEmail, adminPW);

                await AddToRoleAsync(teacher, "Teacher");
            }

            if (db.Course.Count() < 1)
            {
                var Courses = GetCoursesAsync();
                await db.AddRangeAsync(Courses);
            }

            await db.SaveChangesAsync();
        }

        private static async Task<User> AddAdminAsync(string teacherEmail, string adminPW)
        {
            var found = await userManager.FindByEmailAsync(teacherEmail);

            if (found != null) return null!;

            var teatcher = new User
            {
                FirstName = "Adam",
                LastName = "Aven",
                UserName = teacherEmail,
                Email = teacherEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(teatcher, adminPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return teatcher;
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

        private static async Task AddToRoleAsync(User user, string roleName)
        {
            
            var result = await userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            
        }

        private static async Task<IEnumerable<Course>> GetCoursesAsync()
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
                    Description = "Denna kursen har lektioner och studier innom " + title,
                    Modules = GetModules(title),
                    Documents = GetDocuments(title),
                    Users = GetStudnetUsers(),
                };

                foreach(var user in temp.Users)
                {
                    await AddToRoleAsync(user, "Student");
                }

                Courses.Add(temp);
            }

            return Courses;
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

        private static ICollection<User> GetStudnetUsers()
        {
            var faker = new Faker("sv");
            var Users = new List<User>();
            int num = faker.Random.Int(7, 23);
            string FName = faker.Name.FirstName();
            string LName = faker.Name.LastName();

            for (int i = 0; i < num; i++)
            {
                var temp = new User
                {
                    FirstName = FName,
                    LastName = LName,
                    UserName = FName+"."+LName,
                    Email = FName+"."+LName +"@email.com",
                };

                Users.Add(temp);
            }

            return Users;
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