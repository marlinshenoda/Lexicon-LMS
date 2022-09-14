
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

            if(db.ActivityType.Count() < 1)
            {
                var ActivityTypes = GetActivityType();
                await db.AddRangeAsync(ActivityTypes);
                await db.SaveChangesAsync();
            }



            if (db.Course.Count() < 1)
            {
                var AT = db.ActivityType.ToList();
                var Courses = await GetCoursesAsync(AT);
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
                EmailConfirmed = true,
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

        private static ICollection<ActivityType> GetActivityType()
        {
            var faker = new Faker("sv");

            var ActivityTypes = new List<ActivityType>();

            for (int i = 0; i < 10; i++)
            {
                var title = faker.Hacker.Adjective()+" " +faker.Hacker.Verb() + "ing" ;
                var temp = new ActivityType
                {
                    ActivityTypeName = title                    
                    
                };

                ActivityTypes.Add(temp);
            }


            return ActivityTypes;
        }

        private static async Task<IEnumerable<Course>> GetCoursesAsync(List<ActivityType> aT)
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
                    Modules = GetModules(title, aT),
                    Documents = GetDocuments(title),
                    Users = GetStudnetUsers(),
                };

                foreach(var user in temp.Users)
                {
                    await userManager.CreateAsync(user, "StudentPW123!");
                    await AddToRoleAsync(user, "Student");
                }

                Courses.Add(temp);
            }

            return Courses;
        }

        private static ICollection<Module> GetModules(string coursetitle, List<ActivityType> aT)
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
                    Activities = GetActivities(title, aT),
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
            

            for (int i = 0; i < num; i++)
            {
                string FName = faker.Name.FirstName();
                string LName = faker.Name.LastName();
                if (Users.Any(u => u.UserName == FName + "." + LName))
                {
                    i--;
                }
                else
                {
                    var temp = new User
                    {
                        FirstName = FName,
                        LastName = LName,
                        UserName = FName + "." + LName + "@email.com",
                        Email = FName + "." + LName + "@email.com",
                        EmailConfirmed = true
                    };

                    Users.Add(temp);
                }              

            }

            return Users;
        }

        private static ICollection<Activity> GetActivities(string mdouletitle, List<ActivityType> aT)
        {
            var rnd = new Random();
            
            List<ActivityType> activityTypes = aT;


            var faker = new Faker("sv");
            var Activities = new List<Activity>();
            int num = rnd.Next(1, 6);

            for (int i = 0; i < num; i++)
            {
                int rn = rnd.Next(0, activityTypes.Count());
                var title = faker.Hacker.Verb();
                var temp = new Activity
                {
                    ActivityName = mdouletitle + "-" + title,
                    Description = "Dena aktivitet har information för " + title,
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(10, 15)),
                    ActivityType = activityTypes[rn],
                };

                Activities.Add(temp);
            }

            return Activities;
        }
    }
}