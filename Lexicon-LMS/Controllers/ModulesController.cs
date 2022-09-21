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
using AutoMapper;
using Lexicon_LMS.Extensions;
//using static Lexicon_LMS.Helper;

namespace Lexicon_LMS.Controllers
{
    public class ModulesController : Controller
    {
        private readonly Lexicon_LMSContext _context;
        private readonly IMapper mapper;

        public ModulesController(Lexicon_LMSContext context,IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
              return _context.Module != null ? 
                          View(await _context.Module.ToListAsync()) :
                          Problem("Entity set 'Lexicon_LMSContext.Module'  is null.");
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var module = await _context.Module
                .FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // GET: Modules/Create
        //[Authorize(Roles = "Teacher")]

        public IActionResult Create(int? id)
        {
            if (id is null || !CourseExists((int)id))
            {
                return NotFound();
            }

            if (TempData["ValidationError"] != null)
            {
                ModelState.AddModelError("", (string)TempData["ValidationError"]);
            }

            var moduleDefaultStartTime = GetCourseStartTime((int)id).AddHours(8);

            var model = new CreateModuleViewModel
            {
                CourseId = (int)id,
                ModuleStartDate = moduleDefaultStartTime,
                ModuleEndDate = moduleDefaultStartTime.AddHours(1),
               
            };
           
            return PartialView("CreateModulePartailView", model);

        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateModuleViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var errorMessage = "";
                if (!IsModuleTimeCorrect(ref errorMessage, viewModel.CourseId, viewModel.ModuleStartDate, viewModel.ModuleEndDate, null))
                {
                    TempData["ValidationError"] = errorMessage;
                      return Json(new { redirectToUrl = Url.Action("Edit", "Modules", new { id = viewModel.CourseId }) });

                }

                var module = new Module
                {
                    CourseId = viewModel.CourseId,
                    ModulName = viewModel.ModuleName,
                    Description = viewModel.ModuleDescription,
                    StartDate = viewModel.ModuleStartDate,
                    EndDate = viewModel.ModuleEndDate
                };

                _context.Add(module);
                await _context.SaveChangesAsync();

                return RedirectToAction("MainPage", "Students", new { id = viewModel.CourseId }
               
                 );
              
            }

            return View(viewModel);
        }
        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
        private bool IsModuleTimeCorrect(ref string errorMessage, int courseId, DateTime startTime, DateTime endTime, int? thisModuleId)
        {
            // Module starttime must be < module endtime
            if (endTime < startTime)
            {
                errorMessage = "Module end time is before its start time";
                return false;
            }
            if (endTime == startTime)
            {
                errorMessage = "Module end time is equal to its start time";
                return false;
            }
            //  Module ModuleStartTime must be >= course start time  
            //var courseStartTime = GetCourseStartTime(courseId);
            //if (startTime < courseStartTime)
            //{
            //    errorMessage = $"Module start time is before course start time ({courseStartTime}) ";
            //    return false;
            //}
            //var modules = _context.Module
            //  .Where(m => m.CourseId == courseId)
            //  .ToList();


            return true;
        }
        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
        private DateTime GetCourseStartTime(int courseId)
        {
            var d = _context.Course.FirstOrDefault(c => c.Id == courseId).StartDate;
                
                
            return new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
        }
        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var module = await _context.Module.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Module module)
        {
            if (id != module.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.Id))
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
            return View(module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var module = await _context.Module
                .FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Module == null)
            {
                return Problem("Entity set 'Lexicon_LMSContext.Module'  is null.");
            }
            var module = await _context.Module.FindAsync(id);
            if (module != null)
            {
                _context.Module.Remove(module);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
    }
}
