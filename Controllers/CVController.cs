using CV_Website.Models;
using CV_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CV_Website.Controllers
{
    public class CVController : BaseController
    {
        private CVContext _context;

        public CVController(CVContext context) : base(context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditCV(int userId)
        {
            
            var userCV = _context.CVs
            .Include(cv => cv.Skills)
            .Include(cv => cv.Education)
            .Include(cv => cv.Experience)
            .FirstOrDefault(cv => cv.UserId == userId);

            // Allt som finns lagrat
            var allSkills = _context.Skills.ToList();
            var allEducations = _context.Education.ToList();
            var allExperiences = _context.Experience.ToList();

            
            var viewModel = new CVViewModel
            {
                UserId = userId,
                AllSkills = allSkills,
                AllEducations = allEducations,
                AllExperiences = allExperiences,
                SelectedSkills = userCV?.Skills.Select(s => s.SkillsId).ToList() ?? new List<int>(),
                SelectedEducations = userCV?.Education.Select(e => e.EducationId).ToList() ?? new List<int>(),
                SelectedExperiences = userCV?.Experience.Select(ex => ex.ExperienceId).ToList() ?? new List<int>(),
            };

            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditCV(CVViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var userCV = _context.CVs
                .Include(cv => cv.Skills)
                .Include(cv => cv.Education)
                .Include(cv => cv.Experience)
                .FirstOrDefault(cv => cv.UserId == model.UserId);

               
                //Har inget? Skapa ett
                if (userCV == null)
                {
                    userCV = new CV
                    {
                        UserId = model.UserId,
                        Skills = new List<Skills>(),
                        Education = new List<Education>(),
                        Experience = new List<Experience>()
                    };
                    _context.CVs.Add(userCV);
                }

                
                if (model.SelectedSkills != null)
                {
                    userCV.Skills.Clear();
                    var sSkills = _context.Skills
                    .Where(s => model.SelectedSkills.Contains(s.SkillsId)).ToList();

                    foreach (var skill in sSkills)
                    {
                        userCV.Skills.Add(skill);
                    }
                }

                
                if (model.SelectedEducations != null)
                {
                    userCV.Education.Clear();
                    var sEducations = _context.Education
                    .Where(e => model.SelectedEducations.Contains(e.EducationId)).ToList();
                    foreach (var Education in sEducations)
                    {
                        userCV.Education.Add(Education);
                    }
                }


                if (model.SelectedExperiences != null)
                {
                    userCV.Experience.Clear();
                    var sEducations = _context.Experience
                     .Where(e => model.SelectedExperiences.Contains(e.ExperienceId)).ToList();
                    foreach (var Ex in sEducations)
                    {
                        userCV.Experience.Add(Ex);
                    }
                
                }

                    _context.SaveChanges();


                return RedirectToAction("GoToUserPage", "User", new { userId = model.UserId });
            }

            
            return View(model);
        }


        [HttpPost]
        [Authorize]
        public IActionResult CreateT(CVViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditCV",model);
            }

            if (model.NewSkill != null)
                {
                var skill = new Skills
                {
                    Name = model.NewSkill
                };
                _context.Skills.Add(skill);
            }

            if (model.NewEducation != null)
            {
                var ed = new Education
                {
                    Name = model.NewEducation
                };
                _context.Education.Add(ed);
            }

            if (model.NewExperience != null)
            {
                var ex = new Experience
                {
                    Name = model.NewExperience
                };
                _context.Experience.Add(ex);
            }

            _context.SaveChanges();

            return RedirectToAction("EditCV", "CV", new { userId = ViewBag.CurrentUserId });

        }
    }
}
