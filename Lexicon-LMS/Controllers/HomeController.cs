using Lexicon_LMS.Core.Entities;
using Lexicon_LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;
using Activity = System.Diagnostics.Activity;
using Lexicon_LMS.Core.Entities.ViewModel;

namespace Lexicon_LMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(UpLoadDocumentViewModel viewModel)
        {
            await UploadFile(viewModel.UploadedFile);
            TempData["msg"] = "File uploaded successfully";
            return View();
        }

        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            bool isCopy = false;
            try
            {
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));
                    using (var filestream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    isCopy = true;
                }
                else
                {
                    isCopy = false;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return isCopy;
        }

    }
}