using CV_Website.Models;
using CV_Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CV_Website.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CVContext _context;

        public HomeController(ILogger<HomeController> logger, CVContext context) : base(context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            var CVList = _context.CVs.ToList();
            var ProjList = _context.Project.ToList();

            //Lägger alla cv och projekt i en viewmodel
            ProjecthomeViewModel ProjecthomeViewModel = new ProjecthomeViewModel()
            {
                Allprojects = ProjList,
                Allcv = CVList
            };

           
            //om det inte finns några cvn så visar den ett felmedellande på index html
            if (CVList == null || !CVList.Any())
            {
                ViewBag.Message = "No CVs found.";
            }

            return View(ProjecthomeViewModel); 
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
    }
}
