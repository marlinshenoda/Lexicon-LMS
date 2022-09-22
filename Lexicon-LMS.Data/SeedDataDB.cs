
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
                await db.SaveChangesAsync();
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
                string[] activitytypeslist = { "E-Learning", "Assignment", "Lecture", "Group Meeting" };
                var ActivityTypes = GetActivityType(activitytypeslist);
                await db.AddRangeAsync(ActivityTypes);
                await db.SaveChangesAsync();
            }


            if (db.Course.Count() < 1)
            {
                string[] courselist = { "Cours 1", "Cours 2", "Cours 3", "Cours 4", "Cours 5" };
                string[] documentlist = { "Training 1", "Training 2", "Training 3", "Training 4", "Trial" };
                string[] modulelist = { "Java", "C#", "C++", "SQL", "MVC" };
                string[] activitylist = { "Activity 1", "Activity 2", "Activity 3", "Activity 4", "Activity 5" };
                var AT = db.ActivityType.ToList();
                var Courses = await GetCoursesAsync(AT, courselist, modulelist, documentlist, activitylist);
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

        private static ICollection<ActivityType> GetActivityType(string[] types)
        {
            var ActivityTypes = new List<ActivityType>();

            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                   var atype = new ActivityType { ActivityTypeName = t };
                   ActivityTypes.Add(atype);
            }


            return ActivityTypes;
        }

    private static async Task<IEnumerable<Course>> GetCoursesAsync(List<ActivityType> aT, string[] courselist, string[] modulelist, string[] documentlist, string[] activitylist)
        {
            var faker = new Faker("sv");

            var Courses = new List<Course>();

            for (int i = 0; i < courselist.Length; i++)
            {
                var temp = new Course
                {
                    CourseName = courselist[i],
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5)),
                    EndDate = DateTime.Now.AddDays(faker.Random.Int(10, 15)),
                    Description = "Denna kursen har lektioner och studier innom " + courselist[i],
                    Modules = GetModules(modulelist, activitylist, aT),
                    Documents = GetDocuments(documentlist),
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

        private static ICollection<Module> GetModules(string[] courselist, string[] activitylist, List<ActivityType> aT)
        {
            var faker = new Faker("sv");
            var Modules = new List<Module>();
            int num = faker.Random.Int(3, 5);

            for (int i = 0; i < courselist.Length; i++)
            {
                var title = faker.Hacker.Verb();
                var temp = new Module
                {
                    ModulName = courselist[i],
                    Description = "Dena module har info innom " + courselist[i],
                    Activities = GetActivities(activitylist,aT),
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(10, 15)),
                };

                Modules.Add(temp);
            }

            return Modules;
        }

        private static ICollection<Document> GetDocuments(string[] documentlist)
        {
            var faker = new Faker("sv");
            var Documents = new List<Document>();
            int num = faker.Random.Int(2, 7);

            for (int i = 0; i < documentlist.Length; i++)
            {
                var temp = new Document
                {
                    DocumentName = documentlist[i],
                    Description = "Detta documentet har info innom " + documentlist[i],
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
                        PhoneNumber = faker.Phone.PhoneNumber(),
                        EmailConfirmed = true
                    };

                    Users.Add(temp);
                }              

            }

            return Users;
        }

        private static ICollection<Activity> GetActivities(string[] Activitylist, List<ActivityType> aT)
        {
            var rnd = new Random();
            
            List<ActivityType> activityTypes = aT;


            var faker = new Faker("sv");
            var Activities = new List<Activity>();
            int num = rnd.Next(1, 6);

            for (int i = 0; i < Activitylist.Length; i++)
            {
                int rn = rnd.Next(0, activityTypes.Count());
                var title = faker.Hacker.Verb();
                var temp = new Activity
                {
                    ActivityName = Activitylist[i],
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