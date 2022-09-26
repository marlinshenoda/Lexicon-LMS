using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bogus.DataSets;
using Lexicon_LMS.Core.Entities;
using Lexicon_LMS.Core.Entities.ViewModel;
using Lexicon_LMS.Data;
using Lexicon_LMS.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting.Internal;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly Lexicon_LMSContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public UserController(IWebHostEnvironment webHostEnvironment, UserManager<User> userManager, Lexicon_LMSContext context, IMapper mapper)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            this._mapper = mapper;

        }

        // GET: UserController
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

            //var viewModel = mapper.ProjectTo<StudentCourseViewModel>(_context.Users).FirstOrDefault(u => u.Id == userId);
            var viewModel =  _mapper.Map<StudentCourseViewModel>(_context.Users.Include(u => u.Course).FirstOrDefault(u => u.Id == userId));
            
            return View(viewModel);
        }

        public async Task<ActionResult> StudentsByCourseIdPage()
        {
            var user = await _userManager.GetUserAsync(User);

            var viewModelStudents = await _context.Users
                .Include(u => u.Course)
                .Where(u => u.CourseId == user.CourseId)
                .ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return viewModelStudents is null ? NotFound() : View(viewModelStudents);
        }

        // GET: UserController
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

        //public async Task<IActionResult> Welcome(int? id)
        //{
        //    var viewModel = await _context.Course
        //        .Select(a => new Course
        //        {
        //            CourseName = a.CourseName,
        //            Description = a.Description,
        //        })
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    var Details = viewModel.CourseName;


        //    return View(Details);


        // }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentsController/Create
        [Authorize(Roles = "Teacher")]
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

        // POST: UserController/Create
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("FirstName,LastName,Email,PhoneNumber,CourseId")] User Student)
        {
            ModelState.Remove("Course");
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

        // GET: UserController/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
                CourseCourseName = x.Course.CourseName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName,
                ImagePicture = x.ImagePicture

            }).Where(s => s.CourseId!=null);
        }

        private IQueryable<StudentViewModel> GetStudents2()
        {
            return _context.Users.Select(x => new StudentViewModel
            {
                Id = x.Id,
                CourseId = x.CourseId,
                CourseCourseName = x.Course.CourseName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName,
                ImagePicture = x.ImagePicture

            }).Where(s => s.CourseId != null);
        }
        //public async Task<IActionResult> WelcomeCourse(int? id)
        //{

        //    var user = await _context.Course.Select(u => new StudentCourseViewModel
        //    {
        //        Id = u.Id,
        //        CourseName = u.Course.CourseName,
        //        CourseDescription = u.Course.Description,
        //        Documents = u.Documents
        //        //Add more....
        //    })
        //    .FirstOrDefaultAsync(u => u.Id == userId);// _context.Users.Find(_userManager.GetUserId(User));
        //}
        //}
        //return View(viewModel);
        //  }
        public async Task<IActionResult> TeacherHome()
            {
            var logedinUser = _context.Users.Find(_userManager.GetUserId(User));
            var viewModel = await _mapper.ProjectTo<CourseViewModel>(_context.Course.Include(a => a.Modules).Include(a => a.Documents))
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
                    //EndDate = x.EndDate,
                    ActivityTypeActivityTypeName = x.ActivityType.ActivityTypeName,
                    //UploadedFile = (IFormFile)x.Documents

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
        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload([Bind(Prefix = "item")]ActivityListViewModel viewModel)
        {

            var fullPath = await UploadFile(viewModel);
            //var DocumentFile = viewModel.UploadedFile;
            //var DocumentPath = Path.GetFileName("Upload");


            var document = new Core.Entities.Document()
            {
                DocumentName = viewModel.UploadedFile.FileName,
                FilePath = fullPath,
                ActivityId = viewModel.Id
               // CourseId = viewModel.CourseId
            };


            //add
            //savechangeews
            _context.Add(document);
            await _context.SaveChangesAsync();
            //var documentPath = $"~/Upload/";
            //document.FilePath = documentPath;
            //var path = Path.Combine(webHostEnvironment.WebRootPath, documentPath);
            TempData["msg"] = "File uploaded successfully";
            return RedirectToAction(
               "Teacher",
               new { id = viewModel.Id });
        }

        [HttpGet]
        public IActionResult DownloadFile(string filepath)
        {
            var fileName = Path.GetFileName(filepath);
            var path = Path.Combine(webHostEnvironment.WebRootPath, filepath);
            var fs = System.IO.File.ReadAllBytes(path);

            return File(fs, "application/octet-stream", fileName);
        }

        public async Task<string> UploadFile([Bind(Prefix = "item")]ActivityListViewModel viewModel)
        {
            var courseName = _context.Course.FirstOrDefault(c => c.Id == viewModel.CourseId)?.CourseName;
            var moduleName = _context.Module.FirstOrDefault(c => c.Id == viewModel.ModuleId)?.ModulName;
            var activityName = _context.Activity.FirstOrDefault(c => c.Id == viewModel.Id)?.ActivityName;


            var PathToFile = Path.Combine(courseName, moduleName, activityName);
                                          //viewModel.CourseId.ToString(),
                                          //viewModel.ModuleId.ToString(),
                                          //viewModel.Id.ToString());
           // var pathToFile = $"~/upload/{Path.Combine(viewModel.Name, "~/Upload")}/{(viewModel.ModuleModulName, "~/Upload")}/{(viewModel.ActivityName, "~/Upload")}";
            var path = Path.Combine(webHostEnvironment.WebRootPath, PathToFile);

            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }

            string fileName = Path.GetFileName(viewModel.UploadedFile.FileName);
            
            var fullPath = Path.Combine(path, fileName);
            using (FileStream FileStream = new FileStream(fullPath, FileMode.Create))
            {
                viewModel.UploadedFile.CopyTo(FileStream);
            }

            var savePath = Path.Combine(PathToFile, fileName);
            return savePath;
        }
    }
}
