using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lexicon_LMS.Core.Entities;
using Lexicon_LMS.Data;
using AutoMapper;
using Lexicon_LMS.Core.Entities.ViewModel;

namespace Lexicon_LMS.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly Lexicon_LMSContext _context;
        private readonly IMapper mapper;

        public ActivitiesController(Lexicon_LMSContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;

        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            
            var logedinUser = _userManager.GetUserAsync(User).Result;
            var lexicon_LMSContext = _context.Activity.Include(a => a.ActivityType).Include(a => a.Module);
            if (logedinUser != null)
            {
                //var usermodules = _context.Course.Include(a => a.Users).Include(a => a.Modules).Where(a => a.Users.Equals(logedinUser)).Select(a => a.Modules);
                //lexicon_LMSContext = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Activity, Module>)lexicon_LMSContext.Where(a => a.Module.Equals(usermodules));
            }
                
            
            //var lexicon_LMSContext = _context.Activity.Include(a => a.ActivityType).Include(a => a.Module);
            return View(await lexicon_LMSContext.ToListAsync());
        }  
        public async Task<IActionResult> PartialView()
        {
            var lexicon_LMSContext = _context.Activity.Include(a => a.ActivityType).Include(a => a.Module);
            return View(await lexicon_LMSContext.ToListAsync());
            //var viewModel = await mapper.ProjectTo<TeacherViewModel>(_context.Module.Include(a => a.Activities).Include(a => a.Course))
            //     .ToListAsync();

            //return View(viewModel);
            //var model = _context.Activity
            // .Include(m => m.Module)
            // .Select(m => new TeacherViewModel
            // {
            //     ModuleList = m.ModuleId,
            //     ActivityList = m.Id,
            // });

            //return View(await model.ToListAsync());

        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "ActivityTypeName");
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Id");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActivityName,Description,StartDate,EndDate,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "ActivityTypeName", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Id", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "ActivityTypeName", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Id", activity.ModuleId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActivityName,Description,StartDate,EndDate,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "ActivityTypeName", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "Id", "Id", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Activity == null)
            {
                return Problem("Entity set 'Lexicon_LMSContext.Activity'  is null.");
            }
            var activity = await _context.Activity.FindAsync(id);
            if (activity != null)
            {
                _context.Activity.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
          return (_context.Activity?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
