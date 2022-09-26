  using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lexicon_LMS.Core.Entities;
using Lexicon_LMS.Data;
using Microsoft.AspNetCore.Authorization;
using Lexicon_LMS.Core.Entities.ViewModel;
using Lexicon_LMS.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly Lexicon_LMSContext _context;
        private readonly UserManager<User> _userManager;

        public CoursesController(Lexicon_LMSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Courses
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
              return _context.Course != null ? 
                          View(await _context.Course.ToListAsync()) :
                          Problem("Entity set 'Lexicon_LMSContext.Course'  is null.");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.        
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseName,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Teacher")]
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseName,Description,StartDate,EndDate")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'Lexicon_LMSContext.Course'  is null.");
            }

            //var course = await _context.Course.FindAsync(id);

            var course = await _context.Course
                .Include(u=> u.Users)
                .Include(u => u.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CourseInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var current = await CurrentCourse(id);
            var currentCourse = current.course;


            //if (current.course.Modules.Count == 0)

            //    return View(new TeacherViewModel
            //    {
            //        Current = new CurrentViewModel
            //        {
            //            course = current.course,

            //            Assignments = null,
            //        },
            // Orsaskade Error för kurser som hadde inga moduler, assignments och activities
            //        AssignmentList = null,
            //        ModuleList = null,
            //        ActivityList = null
            //});

            var assignmentList = await AssignmentListTeacher(id);
            var moduleList = await GetModuleListAsync(id);
            var module = moduleList.Find(y => y.IsCurrentModule);
            var activityList = new List<ActivityListViewModel>();
            var documentList = new List<ActivityListViewModel>();


            if (module != null)
                activityList = await GetModuleActivityListAsync(module.Id);

            var model = new TeacherViewModel
            {
                Current = current,
                ModuleList = moduleList,
                ActivityList = activityList,
                AssignmentList = assignmentList,
                DocumentList = documentList

            };

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<CurrentViewModel> CurrentCourse(int? id)
        {
            var userId = _userManager.GetUserId(User);
            var course = _context.Course.Include(a => a.Users)
                 .Include(a => a.Modules)
                .ThenInclude(a => a.Activities)
                .FirstOrDefault(a => a.Id == id);

            var students = course.Users.Count();

            var assignments = await _context.Activity.Where(c => c.ActivityType.ActivityTypeName == "Assignment" && c.Module.CourseId == id)
              .OrderBy(a => a.StartDate)
              .Select(a => new AssignmentsViewModel
              {
                  Id = a.Id,
                  Name = a.ActivityName,
                  DueTime = a.EndDate,
                  Finished = a.Documents.Where(d => d.IsFinished.Equals(true)).Count() * 100 / students
              })
              .ToListAsync();
            var model = new CurrentViewModel
            {
                course = course,
                Assignments = assignments
            };

            return model;
        }


        public async Task<List<AssignmentListViewModel>> AssignmentListTeacher(int? id)
        {
            var students = _context.Course.Find(id);


            var assignments = await _context.Activity
                .Where(a => a.ActivityType.ActivityTypeName == "Assignment" && a.Module.CourseId == id)
                .Select(a => new AssignmentListViewModel
                {
                    Id = a.Id,
                    Name = a.ActivityName,
                    StartDate = a.StartDate,
                    DateEndDate = a.EndDate,
                })
                .ToListAsync();

            return assignments;
        }

        public async Task<List<ModuleViewModel>> GetModuleListAsync(int? id)
        {
            var timeNow = DateTime.Now;

            var modules = await _context.Module.Include(a => a.Course)
                .Where(a => a.Course.Id == id)
                .Select(a => new ModuleViewModel
                {
                    Id = a.Id,
                    Name = a.ModulName,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    IsCurrentModule = false
                })
                .OrderBy(m => m.StartDate)
                .ToListAsync();



            return modules;
        }


        private async Task<List<ActivityListViewModel>> GetModuleActivityListAsync(int id)
        {
            var model = await _context.Activity
                .Include(a => a.ActivityType)
                .Include(a => a.Documents)
                .Where(a => a.Module.Id == id)
                .OrderBy(a => a.StartDate)
                .Select(a => new ActivityListViewModel
                {
                    Id = a.Id,
                    ActivityName = a.ActivityName,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    ActivityTypeActivityTypeName = a.ActivityType.ActivityTypeName,
                    Documents = a.Documents,
                    CourseId = a.Module.CourseId,
                    ModuleId = a.ModuleId,
                })
                .ToListAsync();

            return model;
        }


        public async Task<IActionResult> GetTeacherActivityAjax(int? id)
        {
            if (id == null) return BadRequest();

            if (Request.IsAjax())
            {
                var module = await _context.Module.FirstOrDefaultAsync(m => m.Id == id);
                var modules = await _context.Module
                    .Where(m => m.CourseId == module.CourseId)
                    .OrderBy(m => m.StartDate)
                    .Select(m => new ModuleViewModel
                    {
                        Id = m.Id,
                        Name = m.ModulName,
                        StartDate = m.StartDate,
                        EndDate = m.EndDate,
                        IsCurrentModule = false

                    })
                   //.FirstOrDefaultAsync(m => m.Id == id);
                   .ToListAsync();


                var teacherModel = new TeacherViewModel()
                {
                    ModuleList = modules,
                    ActivityList = GetModuleActivityListAsync((int)id).Result,
                };

                return PartialView("ModuleAndActivityPartial", teacherModel);
            }

            return BadRequest();
        }

        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
