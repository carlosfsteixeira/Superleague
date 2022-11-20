using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Helpers;
using Superleague.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Controllers
{
    public class StaffsController : Controller
    {
        private readonly IStaffRepository _staffRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StaffsController(IStaffRepository staffRepository,
                                    ITeamRepository teamRepository,
                                    ICountryRepository countryRepository,
                                    IFunctionRepository functionRepository,
                                    IWebHostEnvironment hostEnvironment)
        {
            _staffRepository = staffRepository;
            _teamRepository = teamRepository;
            _countryRepository = countryRepository;
            _functionRepository = functionRepository;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Staffs
        public IActionResult Index()
        {
            return View(_staffRepository.GetAll().OrderBy(e => e.Name).Include(p => p.Country).Include(p => p.Function).Include(p => p.Team));
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            StaffViewModel staffViewModel = new()
            {
                Staff = new(),

                TeamList = _teamRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),

                FunctionList = _functionRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Description,
                    Value = i.Id.ToString(),
                }),

                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
            };

            return View(staffViewModel);
        }

        // POST: Staffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffViewModel model, IFormFile? file, int id)
        {
            model.Staff.TeamId = id;

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var upload = Path.Combine(wwwRootPath, @"images\staff");

                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    model.Staff.ImageURL = @"\images\staff\" + fileName + extension;
                }

                var functions = _functionRepository.GetAll();

                foreach (var function in functions)
                {
                    if (model.Staff.FunctionId == function.Id)
                    {
                        if (function.Description == "Manager")
                        {
                            var managerExists = _staffRepository.GetAll().Where(t => t.TeamId == model.Staff.TeamId).Where(t => t.Function.Id == function.Id);

                            if (managerExists.Any())
                            {
                                ModelState.AddModelError("Staff.FunctionId", "There is already a manager in this team");

                                StaffViewModel staffViewModel = new()
                                {
                                    Staff = new(),

                                    TeamList = _teamRepository.GetAll().Select(i => new SelectListItem
                                    {
                                        Text = i.Name,
                                        Value = i.Id.ToString(),
                                    }),

                                    FunctionList = _functionRepository.GetAll().Select(i => new SelectListItem
                                    {
                                        Text = i.Description,
                                        Value = i.Id.ToString(),
                                    }),

                                    CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                                    {
                                        Text = i.Name,
                                        Value = i.Id.ToString(),
                                    }),
                                };

                                return View(staffViewModel);
                            }
                        }
                        else if (function.Description == "President")
                        {
                            var presidentExists = _staffRepository.GetAll().Where(t => t.Function.Id == function.Id);

                            if (presidentExists.Any())
                            {
                                ModelState.AddModelError("Staff.FunctionId", "There is already a president in this team");

                                StaffViewModel staffViewModel = new()
                                {
                                    Staff = new(),

                                    TeamList = _teamRepository.GetAll().Select(i => new SelectListItem
                                    {
                                        Text = i.Name,
                                        Value = i.Id.ToString(),
                                    }),

                                    FunctionList = _functionRepository.GetAll().Select(i => new SelectListItem
                                    {
                                        Text = i.Description,
                                        Value = i.Id.ToString(),
                                    }),

                                    CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                                    {
                                        Text = i.Name,
                                        Value = i.Id.ToString(),
                                    }),
                                };

                                return View(staffViewModel);
                            }
                        }
                    }
                }

                await _staffRepository.CreateAsync(model.Staff);

                TempData["success"] = $"New staff member added";

                return RedirectToAction("Edit", "Teams", new { id = model.Staff.TeamId });
            }

            return View(model);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("StaffNotFound");
            }

            StaffViewModel staffViewModel = new()
            {
                Staff = new(),

                FunctionList = _functionRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Description,
                    Value = i.Id.ToString(),
                }),
                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
            };

            staffViewModel.Staff = await _staffRepository.GetByIdAsync(id.Value);

            if (staffViewModel == null)
            {
                return new NotFoundViewResult("StaffNotFound");
            }

            return View(staffViewModel);
        }

        // POST: Staffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StaffViewModel model, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();

                        var upload = Path.Combine(wwwRootPath, @"images\staff");

                        var extension = Path.GetExtension(file.FileName);

                        if (model.Staff.ImageURL != null)
                        {
                            var currentImagePath = Path.Combine(wwwRootPath, model.Staff.ImageURL.TrimStart('\\'));

                            if (System.IO.File.Exists(currentImagePath))
                            {
                                System.IO.File.Delete(currentImagePath);
                            }
                        }

                        using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }

                        model.Staff.ImageURL = @"\images\staff\" + fileName + extension;
                    }

                    var staffFromBD = await _staffRepository.GetByIdAsync(model.Staff.Id);

                    if (staffFromBD.Name != model.Staff.Name)
                    {
                        staffFromBD.Name = model.Staff.Name;
                    }

                    if (staffFromBD.FunctionId != model.Staff.FunctionId)
                    {
                        var functions = _functionRepository.GetAll();

                        foreach (var function in functions)
                        {
                            if (model.Staff.FunctionId == function.Id)
                            {
                                if (function.Description == "Manager")
                                {
                                    var managerExists = _staffRepository.GetAll().Where(t => t.Function.Id == function.Id);

                                    if (managerExists.Any())
                                    {
                                        ModelState.AddModelError("Staff.FunctionId", "There is already a manager in this team");

                                        StaffViewModel staffViewModel = new()
                                        {
                                            Staff = new(),

                                            TeamList = _teamRepository.GetAll().Select(i => new SelectListItem
                                            {
                                                Text = i.Name,
                                                Value = i.Id.ToString(),
                                            }),

                                            FunctionList = _functionRepository.GetAll().Select(i => new SelectListItem
                                            {
                                                Text = i.Description,
                                                Value = i.Id.ToString(),
                                            }),

                                            CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                                            {
                                                Text = i.Name,
                                                Value = i.Id.ToString(),
                                            }),
                                        };

                                        return View(staffViewModel);
                                    }
                                }
                                else if (function.Description == "President")
                                {
                                    var presidentExists = _staffRepository.GetAll().Where(t => t.Function.Id == function.Id);

                                    if (presidentExists.Any())
                                    {
                                        ModelState.AddModelError("Staff.FunctionId", "There is already a president in this team");

                                        StaffViewModel staffViewModel = new()
                                        {
                                            Staff = new(),

                                            TeamList = _teamRepository.GetAll().Select(i => new SelectListItem
                                            {
                                                Text = i.Name,
                                                Value = i.Id.ToString(),
                                            }),

                                            FunctionList = _functionRepository.GetAll().Select(i => new SelectListItem
                                            {
                                                Text = i.Description,
                                                Value = i.Id.ToString(),
                                            }),

                                            CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                                            {
                                                Text = i.Name,
                                                Value = i.Id.ToString(),
                                            }),
                                        };

                                        return View(staffViewModel);
                                    }
                                }
                            }
                        }

                        staffFromBD.FunctionId = model.Staff.FunctionId;
                    }

                    if (staffFromBD.CountryId != model.Staff.CountryId)
                    {
                        staffFromBD.FunctionId = model.Staff.FunctionId;
                    }

                    //UPDATE DB WITH CHANGES
                    await _staffRepository.UpdateAsync(model.Staff);

                    TempData["success"] = $"{model.Staff.Name} updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _staffRepository.ExistAsync(model.Staff.Id))
                    {
                        return new NotFoundViewResult("StaffNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Edit", "Teams", new { id = model.Staff.TeamId });
            }
            return View(model);
        }

        // POST: Staffs/Delete/5
        public async Task<IActionResult> Delete(int staffid)
        {
            var staff = await _staffRepository.GetByIdAsync(staffid);

            await _staffRepository.DeleteAsync(staff);

            TempData["success"] = $"{staff.Name} removed";

            return RedirectToAction("Edit", "Teams", new { id = staff.TeamId });
        }

        public async Task<IActionResult> DeleteFromTable(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);

            await _staffRepository.DeleteAsync(staff);

            TempData["success"] = $"{staff.Name} removed";

            return RedirectToAction("Index");
        }

        public IActionResult StaffNotFound()
        {
            return View();
        }

    }
}
