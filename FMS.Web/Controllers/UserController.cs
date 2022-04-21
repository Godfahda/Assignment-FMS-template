using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using FMS.Data.Services;
using FMS.Data.Models;
using FMS.Web.Models;

namespace FMS.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IFleetService _svc;

        public UserController()
        {
            _svc = new FleetServiceDb();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel m)
        {        
            // call service to Authenticate User
            var user = _svc.Authenticate(m.Email, m.Password);
            // user not authenticated so manually add validation errors for email and password
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid Login Credentials");
                ModelState.AddModelError("Password", "Invalid Login Credentials");
                return View(m);
            }
           
            // authenticated so sign user in using cookie authentication to store principal
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                BuildClaimsPrincipal(user)
            );
            return RedirectToAction("Index","Home");
        }

        //  public IActionResult Register(UserRegisterViewModel m)
        //  {
        //     var exists = _svc.GetUserByEmail(m.Email);
        //     if (exists != null)
            
        //         {
        //          ModelState.AddModelError(nameof(m.Email),"This email is already in use");
        //         }

        //      if (ModelState.IsValid)
            
        //         {
        //          _svc.Register(m.Name,m.Email,m.Password,m.Role);
        //          Alert("User Registered",AlertType.success);

        //          return RedirectToAction(nameof(Login));
        //         }

        //      return View();
        // }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Name,Email,Password,PasswordConfirm,Role")]UserRegisterViewModel m)
        {
             // check if email address is already in use - replaced by use of remote validator in UserRegisterViewModel
             if (_svc.GetUserByEmail(m.Email) != null) {
                 ModelState.AddModelError(nameof(m.Email),"This email address is already in use. Choose another");
             }

             // check validation
             if (!ModelState.IsValid)
            {
                 return View(m);
            }

             // register user
             var user = _svc.Register(m.Name, m.Email, m.Password, m.Role);               
           
             // registration successful now redirect to login page
             Alert("Registration Successful - Now Login", AlertType.info);
             return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ErrorNotAuthorised()
        {   
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorNotAuthenticated()
        {
            return RedirectToAction("Login", "User"); 
        }        

        // used by remote validator to verify email address is available for registration
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmailAddress(string email)
        {
            if (_svc.GetUserByEmail(email) != null)
            {
                return Json($"Email Address {email} is already in use. Please choose another");
            }
            return Json(true);
        }

        // ==================================== Build Claims Principle =================================
        // https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-5.0
        // https://andrewlock.net/introduction-to-authentication-with-asp-net-core/
        
        // return claims principal based on authenticated user
        private  ClaimsPrincipal BuildClaimsPrincipal(User user)
        { 
            // define user claims - you can add as many as required
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())                              
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            // build principal using claims
            return  new ClaimsPrincipal(claims);
        }

    }
}
