﻿using GameBlog.Data;
using GameBlog.Data.Models;
using GameBlog.Models;
using GameBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameBlog.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        //GET
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userViewModel = await usersService.GetUserProfile();

            return View(userViewModel);
        }

        [Authorize]
        public IActionResult All(int pageNumber=1, string searchText="")
        {
            var allUsers = usersService.GetAllUsers(pageNumber, searchText);

            return View(allUsers);
        }

        [Authorize]
        public IActionResult Profile(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var userViewModel = usersService.Profile(id);

            return View(userViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Rate(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await usersService.Rate(id);

            return RedirectToAction("All");
        }
    }
}
