using CV_Website.Models;
using CV_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Packaging.Signing;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;



namespace CV_Website.Controllers
{
    public class UserController : BaseController
    {
        private CVContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserController(UserManager<User> userManager, CVContext context, SignInManager<User> signInManager) : base(context)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            user.Deactivated = true;
            
            user.LockoutEnd = DateTimeOffset.MaxValue; 
            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GoToUserPage(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => (u.Id == userId) && u.Deactivated == false);
            if (user == null || (user.Private && GetCurrentUserId()==null))
            {
                return RedirectToAction("ShowError", new { errorMessage = "Användaren finns ej eller är privat." });
            }

            var projects = _context.Project
          .Where(p => p.Users.Any(u => u.Id == userId))
          .ToList();

            //Hämtar all data samtidigt, istället för att hämta en sak åt gången
            var userCV = _context.CVs
                .Include(cv => cv.Skills)
                .Include(cv => cv.Experience)
                .Include(cv => cv.Education)
                .FirstOrDefault(cv => cv.UserId == userId);

            var viewModel = new UserPageViewModel
            {
                User = user,
                Projects = projects,
                UserCV = userCV,
                Skills = userCV?.Skills?.ToList(),
                Experiences = userCV?.Experience?.ToList(),
                Educations = userCV?.Education?.ToList()
            };

            if (userId != GetCurrentUserId())
            {
                //Kollar ifall användaren har sett personens CV innan med hjälp av cookies som tas bort efter 10min
                if (!Request.Cookies.ContainsKey($"ViewedCV_{userId}"))
                {
                    if (userCV != null)
                    {
                        userCV.ViewCount++;
                        _context.SaveChanges();

                        Response.Cookies.Append($"ViewedCV_{userId}", "true", new Microsoft.AspNetCore.Http.CookieOptions
                        {
                            Expires = System.DateTime.Now.AddMinutes(10)
                        });
                    }
                }
            }

            return View("UserPage", viewModel);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var passwordValidationResult = await _userManager.PasswordValidators
            .FirstOrDefault()
            .ValidateAsync(_userManager, user, model.NewPassword);
                if (passwordValidationResult != IdentityResult.Success)
                {
                    
                    return View(model);
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)// lägger till error för vad som gick fel förmodligen felaktigt nuvarande lösenord
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult DownloadProfile(int Id)
        {

            var user = _context.Users.Include(p => p.Project).FirstOrDefault(u => u.Id == Id); // hämtar användaren som ska laddas ner och projekt
            if ((user.Private == true && GetCurrentUserId() != Id) || (user.Deactivated == true)|| user == null) //Säkerställer att användaren inte är privat eller deactiverad
            {
                return RedirectToAction("ShowError", new { errorMessage = "Användaren är privat eller finns ej." });
            }
            var userCV = _context.CVs // hämtar cv info om användaren
                .Include(cv => cv.Skills)
                .Include(cv => cv.Experience)
                .Include(cv => cv.Education)
                .FirstOrDefault(cv => cv.UserId == Id);
            var xmlModel = new XmlFileViewModel // gör en viewmodel med datan som ska laddas ner
            {
                Name = user.Name,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,

                //Alla rader här tar datan och sätter den i viewmodels som kan bli serialized
                Skills = userCV.Skills
                ?.Select(s => new SkillXml
                {
                    Name = s.Name
                })
                .ToList(),
                Educations = userCV.Education
                ?.Select(s => new EducationXml
                {

                    Name = s.Name
                })
                .ToList(),
                Experiences = userCV.Experience
                ?.Select(s => new ExperienceXml
                {

                    Name = s.Name
                })
                .ToList(),
                Projects = user.Project
                ?.Select(s => new ProjectXml
                {
                    Title = s.Title,
                    Description = s.Description,
                    Information = s.Information


                })
                .ToList(),

            };
            var xmlSerializer = new XmlSerializer(typeof(XmlFileViewModel));

            //Sparar datan i minne och skickar vidare till nedladdning
            using (var memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, xmlModel);


                return File(memoryStream.ToArray(), "application/xml", "UserCV.xml");
            }
        }

        public IActionResult Search(string inputstring)
        {
            if (string.IsNullOrWhiteSpace(inputstring))
            {
                return PartialView("_Partialview", new List<User>());
            }

            var searchTerms = inputstring.Split(' ');

            //Kontrollerar vilken användare termerna matchar med genom att kolla både i skills och namn
            var users = _context.Users
                .Include(user => user.CVs)
                .ThenInclude(cv => cv.Skills)
                .Where(user =>
                    !user.Deactivated &&
                    searchTerms.All(term =>
                        user.Name.ToLower().Contains(term.ToLower()) ||
                        user.CVs.Any(cv => cv.Skills.Any(skill => skill.Name.ToLower().Contains(term.ToLower())))))
                .ToList();

            
            return PartialView("_Partialview", users);
        }


        [Authorize]
        [HttpGet]
        public IActionResult SettingsUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => (u.Id == userId) && u.Deactivated == false);
            if (user == null || GetCurrentUserId() != userId)
            {
                return RedirectToAction("ShowError", new { errorMessage = "EJ din profil." });
            }
            var viewModel = new UserSettingsViewModel
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Private = user.Private,
                Email = user.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SettingsUser(UserSettingsViewModel updatedUser)
        {
            if (!ModelState.IsValid)
            {

                return View(updatedUser);
            }


            var user = _context.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user != null)
            {
                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.UserName; // ta ej bort även om det ser ut som dubble lagring för då kan vi inte logga in
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.Address = updatedUser.Address;
                user.Name = updatedUser.Name;
                user.Private = updatedUser.Private;

                _context.Users.Update(user);
                _context.SaveChanges();
            }

            return RedirectToAction("GoToUserPage", new { userId = GetCurrentUserId() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(int id, IFormFile profileImage)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction("ShowError", new { errorMessage = "Användaren finns ej." });
            }
            
            //Kollar så en bild har skickats in
            if (profileImage != null && profileImage.Length > 0)
                {
                    try
                    {


                    //kollar så att det är en bild
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
                    var fileExtension = Path.GetExtension(profileImage.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return RedirectToAction("ShowError", new { errorMessage = "Ogiltig fil" });
                    }
                    //gör om bilden till bytes och sparar den i databasen
                    using (var memoryStream = new MemoryStream())
                        {
                            await profileImage.CopyToAsync(memoryStream);
                            user.img = memoryStream.ToArray();
                        }
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("ShowError", new { errorMessage = "Ogiltig fil" });
                    }
               
                }

                return RedirectToAction("GoToUserPage", new { userId = id });
            
            

           
        }

        [HttpPost]
        public IActionResult SendMessageFromUser(Models.Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Messages.Add(message);
                _context.SaveChanges();

                return RedirectToAction("GoToUserPage", "User", new { userId = message.ReceiverId });
            }
            //Hämtar all info som behövs för att kunna kalla på userpage viewn igen och att eventuella felmeddelanden visas
            var user = _context.Users.FirstOrDefault(u => u.Id == message.ReceiverId);
            var projects = _context.Project
                .Where(p => p.Users.Any(u => u.Id == message.ReceiverId))
                .ToList();
            var userCV = _context.CVs
                .Include(cv => cv.Skills)
                .Include(cv => cv.Experience)
                .Include(cv => cv.Education)
                .FirstOrDefault(cv => cv.UserId == message.ReceiverId);

            var viewModel = new UserPageViewModel
            {
                User = user,
                Projects = projects,
                UserCV = userCV,
                Skills = userCV?.Skills?.ToList(),
                Experiences = userCV?.Experience?.ToList(),
                Educations = userCV?.Education?.ToList()
            };
            return View("UserPage", viewModel);
        }

        [HttpGet]
        public IActionResult FindMatchingUsers(int cvid)
        {
            var cv = _context.CVs
                .Include(cv => cv.Skills)
                .FirstOrDefault(cv => cv.CVId == cvid);
            if(cv == null)
            {
                return RedirectToAction("ShowError", new { errorMessage = "Hittar ej CV." });
            }
            //Kollar ifall det finns andra cv med liknande skills
            var matchingCVs = _context.CVs
                .Include(c => c.Skills)
                .Where(c => c.CVId != cvid && c.Skills.Any(skill => cv.Skills.Select(s => s.SkillsId).Contains(skill.SkillsId)));
            //tar bort privata profiler sen tar den bort deaktiverade profiler
            if (GetCurrentUserId() == null)
            {
                matchingCVs = matchingCVs.Where(c => !c.User.Private);
            }
            matchingCVs = matchingCVs.Where(c => !c.User.Deactivated);
            var matchingCVslist = matchingCVs.ToList();

            return View(matchingCVslist);
        }

    }
}
