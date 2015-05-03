using System.Web.Mvc;
using ChatApp.Application.Queries;
using ChatApp.Application.Queries.Base;
using ChatApp.Presentation.WebMvc.Models;
using ChatApp.Presentation.WebMvc.Services;

namespace ChatApp.Presentation.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQueryBus _queryBus;
        private readonly IAuthenticationWithoutPassword _authentication;

        public HomeController(IQueryBus queryBus, IAuthenticationWithoutPassword authentication)
        {
            _queryBus = queryBus;
            _authentication = authentication;
        }

        public ActionResult Chat()
        {
            if (!_authentication.IsAuthenticated())
                return RedirectToAction("Login");

            @ViewBag.Username = _authentication.GetUsername();
            return View();
        }

        public ActionResult Login()
        {
            return View(new LoginUser());
        }

        [HttpPost]
        public ActionResult Login(LoginUser user)
        {
            if (!ModelState.IsValid)
                return View(user);

            if (UsersLimitReached(user.Username))
            {
                ModelState.AddModelError("", "User limit reached");
                return View(user);
            }

            _authentication.SignIn(user.Username);
            return RedirectToAction("Chat");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            _authentication.SignOut();
            return RedirectToAction("Login");
        }

        private bool UsersLimitReached(string username)
        {
            return new UsersLimitReachedGetQuery(username)
                .Get(_queryBus).Value;
        }
    }
}