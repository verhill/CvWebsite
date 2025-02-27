using System.Security.Claims;
using CV_Website.Models;
using CV_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NuGet.Packaging.Signing;
using Project = CV_Website.Models.Project;

namespace CV_Website.Controllers
{
    public class ProjectController : BaseController
    {
        private CVContext _context;
        private readonly UserManager<User> _userManager;

        public ProjectController(CVContext context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateProject()
        {
            Project project = new Project();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["CreatorId"] = userId;
            return View(project);


        }


        [HttpPost]
        [Authorize]
        public IActionResult CreateProject(Project project)
        {

            _context.Add(project);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");


        }



        [HttpGet]
        [Authorize]
        public IActionResult EditProject(int Id)
        {


            var project = _context.Project.Find(Id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int loggedInUserId = int.Parse(userId);
            if (project == null)
            {
                return RedirectToAction("ShowError", new { errorMessage = "Projektet hittades inte." });
            }
            if (project.CreatorId != loggedInUserId)
            {
                return RedirectToAction("ShowError", new { errorMessage = "Du är ej skaparen av detta projekt." });
            }
            var viewModel = new EditProjectViewModel
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
                Description = project.Description,
                Information = project.Information
            };
            return View("EditProject", viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditProject(int projectId, EditProjectViewModel updatedProject)
        {


            if (!ModelState.IsValid)
            {
                return View(updatedProject);
            }
            if (projectId != updatedProject.ProjectId)
            {
                return BadRequest();
            }

            var project = _context.Project.FirstOrDefault(u => u.ProjectId == projectId);
            if (project == null)
            {
                return RedirectToAction("ShowError", new { errorMessage = "Projektet hittades inte." });
            }

            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;
            project.Information = updatedProject.Information;


            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult ListProject()
        {
            IQueryable<Project> ProjectList = from project in _context.Project select project;

            return View(ProjectList.ToList());
        }

        public IActionResult ProjectPage(int Id)
        {
            var project = _context.Project
        .Include(p => p.Users)
        .Include(p => p.Creator)
        .FirstOrDefault(u => u.ProjectId == Id);//tar med all information kopplad till projekt

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["LoggedInUserId"] = loggedInUserId;

            if (!User.Identity.IsAuthenticated) // kollar om personen är inloggad
            {
                project.Users = project.Users.Where(u => !u.Private).ToList();  //Tar bort privata profiler             
            }

            project.Users = project.Users.Where(u => !u.Deactivated).ToList();
            
            if (project == null)
            {
                return RedirectToAction("ShowError", new { errorMessage = "Projektet hittades inte." });
            }


            return View(project);
        }
        [Authorize]
        public IActionResult LeaveProject(int id)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(loggedInUserId);
            var project = _context.Project.Include(p => p.Users).FirstOrDefault(p => p.ProjectId == id);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null && project.Users.Contains(user))
            {

                project.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("ProjectPage", new { id = id });
        }
        [Authorize]
        public IActionResult JoinProject(int id)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(loggedInUserId);
            var project = _context.Project.Include(p => p.Users).FirstOrDefault(p => p.ProjectId == id);

            if (!project.Users.Any(u => u.Id == userId)) //´kollar om användaren inte är med i projektet
            {

                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    project.Users.Add(user);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("ProjectPage", new { id = id });
        }
        [Authorize]
        [HttpPost]
        public IActionResult DeleteProject(int id)
        {
            var project = _context.Project
            .Include(p => p.Users)
            .FirstOrDefault(p => p.ProjectId == id);//hämtar all data kopplad till projekt så i sambandstabellen även raderas de rader som ska bort

            if (project != null)
            {
                _context.Project.Remove(project);
                _context.SaveChanges();
            }

            return RedirectToAction("ListProject");
        }
    }
}
