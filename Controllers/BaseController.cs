using Microsoft.AspNetCore.Mvc;
using CV_Website.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
namespace CV_Website.Controllers
{
    public class BaseController : Controller
    {
        private readonly CVContext _context;

        public BaseController(CVContext context)
        {
            _context = context;
        }

        protected void SetUnreadMessageCount(int userId)
        {
            var unreadMessagesCount = _context.Messages
                .Count(m => m.ReceiverId == userId && !m.Read && (m.Sender == null || !m.Sender.Deactivated));
 

            ViewData["TotalUnreadMessages"] = unreadMessagesCount;
        }
        public IActionResult ShowError(string errorMessage)
        {
            ViewData["ErrorMessage"] = errorMessage;
            return View();
        }
        protected int? GetCurrentUserId()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null || string.IsNullOrEmpty(userId.Value))
            {
                return null;
            }

            return int.Parse(userId.Value);
        }

        //Körs varje gång en action körs
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(GetCurrentUserId() != null)
            {
                SetUnreadMessageCount(GetCurrentUserId().Value);
                base.OnActionExecuting(context);

                ViewBag.CurrentUserName = _context.Users
                .Where(u => u.Id == GetCurrentUserId().Value)
                .Select(u => u.Name)
                .FirstOrDefault();
                
            }
            ViewBag.CurrentUserId = GetCurrentUserId();
            
            

        }
    }
}
