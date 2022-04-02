﻿
namespace GameBlog.Controllers
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using static Data.DataConstants.Role;

    public class AdministrationController : Controller
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public AdministrationController(GameBlogDbContext db, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string searchText)
        {
            var usersQuery = db.Users.AsQueryable();

            if (!String.IsNullOrEmpty(searchText))
            {
                usersQuery = usersQuery
                    .Where(uq => uq.UserName.Contains(searchText) || uq.Email.Contains(searchText));
            }

            var users = usersQuery.Select(u => new UserViewModel
            {
                UserName = u.UserName,
                Articles = u.Articles,
                Email = u.Email,
                Id = u.Id,
                Ratings = u.Ratings,
                Reputation = u.Reputation
            })
             .ToList();

            return View(new AllUsersViewModel
            {
                Users = users
            });
        }

        //GET:
        public IActionResult AdministeredUser(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            User? user = db.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return BadRequest();
            }

            List<Article> userArticles = db.Articles
               .Where(a => a.UserId == user.Id)
               .ToList();

            List<Guid> gameIds = db.Ratings
                .Where(x => x.UserId == user.Id)
                .Select(x => x.GameId)
                .Distinct()
                .ToList();

            List<Game> userGames = db.Games
                .Include(r => r.Ratings)
                .Where(g => gameIds.Contains(g.Id))
                .ToList();

            UserViewModel userViewModel = new UserViewModel
            {
                Email = user.Email,
                Articles = userArticles,
                Id = user.Id,
                Ratings = user.Ratings,
                Reputation = user.Reputation,
                UserName = user.UserName
            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User? user = db.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(id);
            }

            IdentityUserRole<Guid>? userRoles = db.UserRoles.SingleOrDefault(ur => ur.UserId == user.Id);

            if (userRoles != null)
            {
                await userManager.RemoveFromRoleAsync(user, Administrator);
                return RedirectToAction("Index", "Administration");
            }

            await userManager.AddToRoleAsync(user, Administrator);

            db.SaveChanges();

            return RedirectToAction("Index", "Administration");
        }
    }
}