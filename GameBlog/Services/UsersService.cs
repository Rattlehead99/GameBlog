

namespace GameBlog.Services
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPaginationService paginationService;

        public UsersService(GameBlogDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IPaginationService paginationService)
        {
            this.db = db;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.paginationService = paginationService;
        }

        public AllUsersViewModel GetAllUsers(int pageNumber=1, string searchText="")
        {
            var usersQuery = db.Users
                .AsQueryable();

            int newPageNumber = paginationService
                .PageCorrection(pageNumber, usersQuery);

            usersQuery = paginationService
                .Pagination(newPageNumber, usersQuery);

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

            return new AllUsersViewModel
            {
                Users = users,
                PageNumber = newPageNumber
            };
        }

        public async Task<UserViewModel> GetUserProfile()
        {
            var userContext = httpContextAccessor.HttpContext?.User;
            var user = await userManager.GetUserAsync(userContext);

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

            return userViewModel;
        }

        public User GetUserById(Guid id)
        {
            User? user = db.Users.SingleOrDefault(u => u.Id == id);
            
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user;
        }

        public UserViewModel Profile(Guid id)
        {
            User? user = this.GetUserById(id);

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

            return userViewModel;
        }

        public async Task Rate(Guid id)
        {
            var userContext = httpContextAccessor.HttpContext?.User;
            var loggedUser = await userManager.GetUserAsync(userContext);

            User? user = db.Users
                .Include(ur => ur.UserReputations)
                .SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!user.UserReputations.Any(u => u.LikedUserId == loggedUser.Id))
            {
                user.Reputation++;
                user.UserReputations.Add(new UserReputations { UserId = user.Id, User = user, LikedUserId = loggedUser.Id, LikedUser = loggedUser });
                db.SaveChanges();
            }

        }
    }
}
