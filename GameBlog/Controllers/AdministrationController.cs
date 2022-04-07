﻿
namespace GameBlog.Controllers
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using GameBlog.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using static Data.DataConstants.Role;

    public class AdministrationController : Controller
    {
        private readonly IAdministrationService administrationService;

        public AdministrationController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public IActionResult Index(string searchText)
        {
            var users = administrationService.AllUsers(searchText);

            return View(users);

        }

        //GET:
        public IActionResult AdministeredUser(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var userViewModel = administrationService.AdministratedUser(id);

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await administrationService.ChangeUserRole(id);

            return RedirectToAction("Index", "Administration");
        }
    }
}
