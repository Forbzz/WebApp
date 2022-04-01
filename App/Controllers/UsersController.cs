using Data;
using Data.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using App.ViewModels.User;
using App.ViewModels.Address;
using System.Threading;
using App.ViewModels.Home;

namespace App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IUserStore<AppUser> _userStore;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IDoctorService _doctorService;
        private readonly IUserService _userService;

        public UsersController(IIdentityService identityService, IUserStore<AppUser> userStore,
            ApplicationDbContext dbContext, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager,
            IDoctorService doctorService, IUserService userService)
        {
            _identityService = identityService;
            _userStore = userStore;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _doctorService = doctorService;
            _userService = userService;
        }

        // GET: UsersController/Details/5
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadUsers()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = HttpContext.Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault()
                    + "][name]"].FirstOrDefault();

                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;

                var recordsTotal = 0;

                var data = _identityService.GetUsers();

                var debugg = await data.ToListAsync();

                if (!(string.IsNullOrEmpty(sortColumn)) && !(string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data = data.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => (m.Email.Contains(searchValue) || m.Role.Contains(searchValue)));
                }

                recordsTotal = data.Count();

                var result = await data.Skip(skip).Take(pageSize).ToListAsync();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = result });
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                IdentityResult result = default;
                if (model.Role == "Admin")
                {
                    result = await _identityService.CreateUser<AppUser>(model.Email, model.Password, model.Role);
                }
                else if (model.Role == "Doctor")
                {
                    result = await _identityService.CreateUser<AppUser>(model.Email, model.Password, model.Role);
                }
                else if (model.Role == "Pacient")
                {
                    result = await _identityService.CreateUser<AppUser>(model.Email, model.Password, model.Role);
                }

                if (result.Succeeded)
                {
                    return LocalRedirect(model.ReturnUrl);
                }

                model.Roles = await GetRoles();

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string id, string returlUrl = null)
        {
            var user = await _userManager.FindByIdAsync(id);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            EditUserViewModel model = null;

            if (role == "Admin")
            {
                model = new EditUserViewModel(user, role);
            }

            if (role == "Doctor")
            {
                var doc = await _doctorService.GetDoctorByIdAsync(id);
                var specs = await _doctorService.GetSpecialities().Select(x => x.Name).ToListAsync();
                var branches = await _doctorService.GetBranches().Select(x => x.Name).ToListAsync();
                model = new EditUserViewModel(doc, role, branches, specs);
                model.returnUrl = returlUrl;
            }

            if (!(role == "Pacient"))
            {
                return View(model);
            }

            return RedirectToAction("Edit", "Pacients", new
            {
                id = id,
                returlUrl = returlUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Role == "Admin")
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                user.Name1 = model.Name1;
                user.Name2 = model.Name2;
                user.Name3 = model.Name3;

                var result = await _userStore.UpdateAsync(user, CancellationToken.None);
                if (!result.Succeeded)
                {
                    foreach (var e in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, e.Description);
                    }
                    return View(model);
                }
            }

            if (model.Role == "Doctor")
            {
                await _doctorService.UpdateAsync(model.Id, model.Name1, model.Name2,
                    model.Name3, model.Branch, model.Speciality, model.ContactEmail,
                    model.ContactPhoneNumber, model.CabinetNumber);
            }

            if (!string.IsNullOrEmpty(model.returnUrl))
                return LocalRedirect(model.returnUrl);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> ChangeSchedule(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
                id = _userManager.GetUserId(User);
            var viewModel = new ChangeScheduleViewModel
            {
                ReturnUrl = returnUrl,
                DoctorId = id
            };
            viewModel.Schedules = await _doctorService.GetScheduleByDocId(id).ToArrayAsync();
            viewModel.Doctor = await _doctorService.GetDoctorByIdAsync(id);
            return View(viewModel);
        }

        public async Task<IActionResult> AddSchedule(string docId, Schedule schedule, string returnUrl)
        {
            await _doctorService.AddToSchedules(docId, schedule);
            return RedirectToAction(nameof(ChangeSchedule), new { id = docId, returnUrl = returnUrl });
        }

        public async Task<IActionResult> EditSchedule(int id, string docId, string returnUrl)
        {
            var vm = new EditScheduleViewModel
            {
                DocId = docId,
                returnUrl = returnUrl,
                Schedule = await _doctorService.GetScheduleById(id)
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditSchedule(EditScheduleViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await _doctorService.UpdateSchedule(viewModel.Schedule);
            return LocalRedirect(viewModel.returnUrl);
        }

        public async Task<IActionResult> DeleteSchedule(int id, string docId, string returnUrl)
        {
            await _doctorService.DeleteScheduleAsync(id);
            return RedirectToAction(nameof(ChangeSchedule), new { id = docId, returnUrl = returnUrl });
        }

        // TODO Удалять все данные связанные с user
        public async Task<IActionResult> Remove(string id)
        {
            var user = await _userStore.FindByIdAsync(id, CancellationToken.None);
            if (user != null && user.Email != "admin@gmail.com")
            {
                var count = await _dbContext.UserTokens.Where(u => u.UserId == id).CountAsync();
                _dbContext.UserTokens.RemoveRange(_dbContext.UserTokens.Where(u => u.UserId == id));

                IdentityResult result = default;

                if (await _userManager.IsInRoleAsync(user, "Pacient"))
                {
                    result = await _userService.DeletePacient(user);
                }
                else if (await _userManager.IsInRoleAsync(user, "Doctor"))
                {
                    result = await _userService.DeleteDoctor(user);
                }
                else if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    result = await _userService.DeleteAdmin(user);
                }

                if (!result.Succeeded)
                {
                    return Json(new
                    {
                        status = "Error",
                        errors = result.Errors,
                        message = "Can't delete user"
                    });
                }
            }

            return Ok(new
            {
                status = "Success"
            });
        }

        public async Task<IActionResult> Create(string returnUrl = null)
        {
            var model = new CreateUserModel { ReturnUrl = returnUrl };
            model.Roles = await GetRoles();

            return View(model);
        }

        public async Task<IActionResult> Details(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = _userManager.GetUserId(User);
            }
            var user = await _userManager.FindByIdAsync(id);

            if (await _userManager.IsInRoleAsync(user, "Pacient"))
            {
                return RedirectToAction("Details", "Pacients", new
                {
                    id = id,
                    returnUrl = returnUrl
                });
            }

            if (await _userManager.IsInRoleAsync(user, "Doctor"))
            {
                return RedirectToAction("Details", "Doctors", new
                {
                    id = id,
                    returnUrl = returnUrl
                });
            }

            return View(new AdminViewModel
            {
                User = user,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAddress(AddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await _userService.EditUserAddress(viewModel.UserId, viewModel.ApartmentNumber, viewModel.Country,
                viewModel.District, viewModel.HomeNumber, viewModel.PostalCode, viewModel.Region,
                viewModel.Street, viewModel.Town);

            return LocalRedirect(viewModel.returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeAddress(string Id, string returnUrl)
        {
            var address = await _userService.GetUserAddress(Id);
            var vm = new AddressViewModel
            {
                UserId = Id,
                returnUrl = returnUrl,
                Country = address.Country,
                ApartmentNumber = address?.ApartmentNumber,
                District = address?.District,
                HomeNumber = address?.HomeNumber,
                //PostalCode = address?.PostalCode,
                //Region = address?.Region?.Name,
                Street = address?.Street,
                Town = address?.Street
            };
            return View(vm);
        }

        private async Task<List<SelectListItem>> GetRoles()
        {
            return await _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToListAsync();
        }
    }
}
