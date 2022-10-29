using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using Superleague.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ITeamRepository _teamRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AccountController(IUserHelper userHelper, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ITeamRepository teamRepository, IWebHostEnvironment hostEnvironment)
        {
            _userHelper = userHelper;
            _roleManager = roleManager;
            _userManager = userManager;
            _teamRepository = teamRepository;
            _hostEnvironment = hostEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);

                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i,
                }),

                ClubList = _teamRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var upload = Path.Combine(wwwRootPath, @"images\archive\users");

                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    model.ImageURL = @"\images\archive\users" + fileName + extension;
                }

                var user = await _userHelper.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.Email,
                        ImageURL = model.ImageURL,
                        Role = model.Role,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to create new user");

                        return View(model);
                    }

                    if (model.Role == "Club")
                    {
                        user.TeamId = model.TeamId;
                    }

                    await _userHelper.AddUserToRoleAsync(user, model.Role);

                    TempData["success"] = $"New user created";

                    return RedirectToAction("ListUsers");
                }
            }

            ModelState.AddModelError(string.Empty, "This email address is already in use by another account");

            return View(model);
        }

        [HttpGet]
        public IActionResult GetListUsers()
        {
            var users = _userManager.Users;

            return Json(new { data = users });
            //return View(users);
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            return View();
        }

        public async Task<IActionResult> UserProfile()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            var model = new UserProfileViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.ImageURL = user.ImageURL;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.ImageURL = model.ImageURL;

                    var response = await _userHelper.UpdateUserAsync(user);

                    if (response.Succeeded)
                    {
                        TempData["success"] = $"Profile updated";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        TempData["success"] = $"Password Updated";

                        return this.RedirectToAction("UserProfile");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }
    }
}
