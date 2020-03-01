using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Core;
using HrPayroll.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrPayroll.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<AppUser> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly SignInManager<AppUser> signInManager;
        public readonly IPasswordHasher<AppUser> passwordHasher;
        public readonly IPasswordValidator<AppUser> passwordValidator;
        public readonly PayrollDbContext payrollDb;

        public AccountController(UserManager<AppUser> _userManager
                                   , RoleManager<IdentityRole> _roleManager
                                    , SignInManager<AppUser> _signInManager
                                      , IPasswordHasher<AppUser> _passwordHasher
                                       , IPasswordValidator<AppUser> _passwordValidator
                                        , PayrollDbContext _payrollDb)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
            passwordHasher = _passwordHasher;
            passwordValidator = _passwordValidator;
            payrollDb = _payrollDb;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async  Task<ActionResult> Login(UserLoginModel model)
        {
            if(ModelState.IsValid)
            {
                AppUser user = null;
               user = await userManager.FindByEmailAsync(model.UserNameOrEmail);
                if(user == null)
                {
                    user = await userManager.FindByNameAsync(model.UserNameOrEmail);
                }
                
                if (user!=null)
                {
         PasswordVerificationResult  verificationResult  =passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
                    if (PasswordVerificationResult.Success == verificationResult)
                   {
                     IList<string> str =  await  userManager.GetRolesAsync(user);
                        
                            SessionUserModel sessionUser = new SessionUserModel()
                            {
                                UserName  = user.UserName,
                                Email = user.Email,
                                Id = user.Id,
                                Photo = user.Photo,
                                Role =str[0]
                            };
                            HttpContext.Session.SetObjectAsJson("UserData", sessionUser);
                            await signInManager.SignInAsync(user,true);
                            return RedirectToAction("AllEmployee", "Employee",new { area= "Admin"});
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password incorrect");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Email incorrect");
                }
            }
           
            return View();
        }



        public ActionResult ChangePass()
        {
            return View();
        }
    }
}