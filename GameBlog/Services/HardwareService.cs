

namespace GameBlog.Services
{
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HardwareService : IHardwareService
    {
        private readonly GameBlogDbContext db;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPaginationService paginationService;

        public HardwareService(IPaginationService paginationService, GameBlogDbContext db)
        {
            this.paginationService = paginationService;
            this.db = db;
        }

        public void CreateHardware(HardwareViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("The hardware model doesn't exist.");
            }
            if (model.Type != "GPU" && model.Type != "CPU")
            {
                throw new ArgumentException("Hardware type must be GPU or CPU");
            }

            var hardware = new Hardware()
            {
                Id = model.Id,
                PerformanceScore = model.PerformanceScore,
                Type = model.Type,
                Name = model.Name,
            };

            db.Hardware.Add(hardware);
            db.SaveChanges();
        }

        public void EditHardware(HardwareViewModel model)
        {
            var hardwareData = db.Hardware.Find(model.Id);

            if (hardwareData == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            hardwareData.Type = model.Type;
            hardwareData.PerformanceScore = model.PerformanceScore;
            hardwareData.Name = model.Name;

            db.Hardware.Update(hardwareData);
            db.SaveChanges();
            
        }

        public void DeleteHardware([FromForm]Guid id)
        {
            var hardwareData = db.Hardware.Find(id);

            if (hardwareData == null)
            {
                throw new ArgumentNullException("Hardware NOT Found");
            }

            db.Hardware.Remove(hardwareData);
            db.SaveChanges();

        }

        public AllHardwareViewModel GetAllHardware(int pageNumber, string searchText)
        {
            var hardware = db.Hardware
                .OrderByDescending(h => h.PerformanceScore)
                .AsQueryable();

            int newPageNumber = paginationService.PageCorrection(pageNumber, hardware);
            hardware = paginationService.Pagination(newPageNumber, hardware);

            if (!String.IsNullOrEmpty(searchText))
            {
                hardware = hardware
                    .Where(h => h.Name.Contains(searchText) || h.Type.Equals(searchText));
            }

            var hardwareData = hardware.Select(h => new HardwareViewModel
            {
                Id = h.Id,
                Name = h.Name,
                PerformanceScore = h.PerformanceScore,
                Type = h.Type
            })
            .ToList();

            var allHardware = new AllHardwareViewModel
            {
                HardwareModels = hardwareData,
                PageNumber = newPageNumber
            };

            return allHardware;
        }

        public HardwareViewModel GetHardwareById(Guid id)
        {
            var hardware = db.Hardware.SingleOrDefault(h => h.Id == id);

            var hardwareModel = new HardwareViewModel
            {
                Id=hardware.Id,
                Name=hardware.Name,
                PerformanceScore=hardware.PerformanceScore,
                Type = hardware.Type
            };

            return hardwareModel;
        }
    }
}
