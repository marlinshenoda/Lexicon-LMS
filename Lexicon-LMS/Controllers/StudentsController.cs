using Lexicon_LMS.Core.Entities;
using Lexicon_LMS.Core.Entities.ViewModel;
using Lexicon_LMS.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Lexicon_LMS.Controllers
{
    public class StudentsController : Controller
    {
        private readonly Lexicon_LMSContext _context;
        private readonly UserManager<User> _userManager;

        public StudentsController(UserManager<User> userManager, Lexicon_LMSContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: StudentsController
        public async Task<ActionResult> Index()
        {
            var logedinUser = _context.Users.Find(_userManager.GetUserId(User));

            var viewModel = GetStudents();

            if (logedinUser != null && logedinUser.CourseId != null)
            {  
                var CourseSuers = viewModel.Where(c => c.CourseId == logedinUser.CourseId);

                return View(CourseSuers.ToList());
            }

            return View(await viewModel.ToListAsync());
        }

        // GET: StudentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            var courses = await _context.Course.ToListAsync();
            var studentV = new StudentCreateViewModel
            {
                AvailableCourses = courses.Select(c => new SelectListItem
                {
                    Text = c.CourseName.ToString(),
                    Value = c.Id.ToString(),
                    Selected = false
                }).ToList()
            };     

            return View(studentV);
        }

        // POST: StudentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("FirstName,LastName,Email,CourseId")] User Student)
        {
            if (ModelState.IsValid)
            {
                Student.UserName = Student.Email;
                var result = await _userManager.CreateAsync(Student, "StudentPW123!");

                if (result.Succeeded)
                {
                    var result2 = await _userManager.AddToRoleAsync(Student, "Student");
                    if (!result2.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                }


                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index));

        }

        // GET: StudentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private IQueryable<StudentViewModel> GetStudents()
        {
            return _context.Users.Select(x => new StudentViewModel
            {
                Id = x.Id,
                CourseId = x.CourseId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName,
                ImagePicture = x.ImagePicture

            });
        }
    }
}
