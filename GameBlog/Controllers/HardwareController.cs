namespace GameBlog.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using GameBlog.Data;
    using GameBlog.Data.Models;
    using GameBlog.Models;
    using GameBlog.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using static GameBlog.Data.DataConstants.Role;

    [AutoValidateAntiforgeryToken]
    public class HardwareController : Controller
    {
        private readonly IHardwareService hardwareService;

        public HardwareController(IHardwareService hardwareService)
        {
            this.hardwareService = hardwareService;
        }

        [AllowAnonymous]
        public IActionResult Index(int pageNumber, string searchText)
        {
            var hardware = hardwareService.GetAllHardware(pageNumber, searchText);

            return View(hardware);
        }

        [HttpGet]
        [Authorize(Roles = Administrator)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HardwareViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            hardwareService.CreateHardware(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = Administrator)]
        public IActionResult Edit(Guid id)
        {
            var hardware = hardwareService.GetHardwareById(id);

            return View(hardware);
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HardwareViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            hardwareService.EditHardware(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = Administrator)]
        public IActionResult Delete(Guid id)
        {
            var hardware = hardwareService.GetHardwareById(id);

            return View(hardware);
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteForm(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            hardwareService.DeleteHardware(id);

            return RedirectToAction("Index");
        }

    }
}
