using AutoMapper;
using Lexicon_LMS.Core.Entities;
using Lexicon_LMS.Core.Entities.ViewModel;
using Lexicon_LMS.Data;
using Lexicon_LMS.Extensions;
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
        private readonly IMapper mapper;
        private readonly UserManager<User> _userManager;

        public StudentsController(UserManager<User> userManager, Lexicon_LMSContext context, IMapper mapper)
        {
            _context = context;   
            _userManager = userManager;
            this.mapper = mapper;

        }

        // GET: StudentsController
        public async Task<ActionResult> WelcomePage()
        {
            var userId = _userManager.GetUserId(User);

            //var user = await _context.Users.Select(u => new StudentCourseViewModel
            //{
            //    Id = u.Id,
            //    CourseName = u.Course.CourseName,
            //    CourseDescription = u.Course.Description,
            //    Documents = u.Documents
            //    //Add more....
            //})
            //.FirstOrDefaultAsync(u => u.Id == userId);// _context.Users.Find(_userManager.GetUserId(User));

            var viewModel = mapper.ProjectTo<StudentCourseViewModel>(_context.Users).FirstOrDefault(u => u.Id == userId);
          
            return View(viewModel);
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
        public async Task<ActionResult> WelcomeCourse(int id)
        {
            var viewModel = await _context.Course.FirstOrDefaultAsync(c => c.Id == id);
            //(a => new Course
            //    {
            //        CourseName = a.CourseName,  
            //        Description = a.Description,    
            //    })
            var Details = viewModel.CourseName.ToList();
           

            return View(Details);



            return View(viewModel);
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Teacher(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var assignmentList = await AssignmentListTeacher(id);
            var moduleList = await GetTeacherModuleListAsync(id);
            var module = moduleList.Find(y => y.IsCurrentModule);
            var activityList = new List<ActivityListViewModel>();

            if (module != null)
                activityList = await GetModuleActivityListAsync(module.Id);

            var model = new TeacherViewModel
            {
                AssignmentList = assignmentList,
                ModuleList = moduleList,
                ActivityList = activityList
            };

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
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

                return PartialView("TeacherModuleAndActivityPartial", teacherModel);
            }

            return BadRequest();
        }
      

     
        public async Task<List<TeacherAssignmentListViewModel>> AssignmentListTeacher(int? id)
        {
            var students = _context.Course.Find(id);


            var assignments = await _context.Activity
                .Where(a => a.ActivityType.ActivityTypeName == "Assignment" && a.Module.CourseId == id)
                .Select(a => new TeacherAssignmentListViewModel
                {
                    Id = a.Id,
                    Name = a.ActivityName,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                })
                .ToListAsync();

            return assignments;
        }
 
        private async Task<List<ActivityListViewModel>> GetModuleActivityListAsync(int id)
        {
            var model = await _context.Activity
                .Include(a => a.ActivityType)
                .Where(a => a.Module.Id == id)
                .OrderBy(a => a.StartDate)
                .Select(a => new ActivityListViewModel
                {
                    Id = a.Id,
                    ActivityName = a.ActivityName,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    ActivityTypeActivityTypeName = a.ActivityType.ActivityTypeName
                })
                .ToListAsync();

            return model;
        }
        public async Task<List<ModuleViewModel>> GetTeacherModuleListAsync(int? id)
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
    //    public async Task<IActionResult> TeacherHome(int? CourseId)
     //   {
            //var logedinUser = _context.Users.Find(_userManager.GetUserId(User));
            //var viewModel = await mapper.ProjectTo<CourseViewModel>(_context.Course.Include(a => a.Modules).Include(a => a.Documents))
            //    .OrderBy(s => s.Id)
            //    .ToListAsync();
            //if (logedinUser != null && logedinUser.CourseId != null)
            //{
            //    var course = await _context.Course
            //    .Include(c => c.Modules)
            //    .ThenInclude(m => m.Activities)
            //    .ThenInclude(a => a.ActivityType)
            //    .FirstOrDefaultAsync(c => c.Id == logedinUser.CourseId);

            //    var activities = course.Modules.SelectMany(m => m.Activities).Select(x => new ActivityListViewModel
            //    {
            //        Id = x.Id,
            //        ActivityName = x.ActivityName,
            //        StartDate = x.StartDate,
            //        EndDate = x.EndDate,
            //        ActivityTypeActivityTypeName = x.ActivityType.ActivityTypeName,
            //        //ModuleId = x.Module.Id,

            //        //ModulName = x.Module.ModulName

            //    }).ToList();

            //    return View(activities);

            //}
            //return View(viewModel);
      //  }
            public async Task<IActionResult> TeacherHome()
            {
            var logedinUser = _context.Users.Find(_userManager.GetUserId(User));
            var viewModel = await mapper.ProjectTo<CourseViewModel>(_context.Course.Include(a => a.Modules).Include(a => a.Documents))
                .OrderBy(s => s.Id)
              .ToListAsync();
            if (logedinUser != null && logedinUser.CourseId != null)
            {
                var course = await _context.Course
                .Include(c => c.Modules)
                .ThenInclude(m => m.Activities)
                .ThenInclude(a => a.ActivityType)
                .FirstOrDefaultAsync(c => c.Id == logedinUser.CourseId);

                var activities = course.Modules.SelectMany(m => m.Activities).Select(x => new ActivityListViewModel
                {
                    Id = x.Id,
                    ActivityName = x.ActivityName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ActivityTypeActivityTypeName = x.ActivityType.ActivityTypeName,
                    //ModuleId = x.Module.Id,

                    //ModulName = x.Module.ModulName

                }).ToList();

                return View(activities);

                //TempData["CourseId"] = id;

            }      return View(viewModel);


        }
        //[Authorize(Roles = "Teacher")]
        //public async Task<IActionResult> TeacherHome(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var Elearnig = await ElearningtListTeacher(id);

        //    var assignmentList = await AssignmentListTeacher(id);
        //    var moduleList = await GetTeacherModuleListAsync(id);
        //    var module = moduleList.Find(y => y.IsCurrentModule);
        //    var activityList = new List<ActivityListViewModel>();

        //    if (module != null)
        //        activityList = await GetModuleActivityListAsync(module.Id);

        //    var model = new TeacherViewModel
        //    {
        //        Elearning = Elearnig,
        //        AssignmentList = assignmentList,
        //        ModuleList = moduleList,
        //        ActivityList = activityList
        //    };

        //    if (model == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(model);
        //}
    }
}
