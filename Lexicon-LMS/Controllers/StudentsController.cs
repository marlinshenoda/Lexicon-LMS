using Lexicon_LMS.Core.Entities;
using Lexicon_LMS.Core.Entities.ViewModel;
using Lexicon_LMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult> WelcomePage()
        {
            var logedinUser = _context.Users.Find(_userManager.GetUserId(User));

            return View(logedinUser);
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
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentsController/Create
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: StudentsController/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentsController/Edit/5
        [HttpPost]
        [Authorize(Roles = "Teacher")]
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
