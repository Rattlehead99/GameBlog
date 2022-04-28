using GameBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameBlog.Services
{
    public interface IHardwareService
    {
        public void CreateHardware(HardwareViewModel model);
        
        public void EditHardware(HardwareViewModel model);

        public void DeleteHardware([FromForm] Guid id);

        public AllHardwareViewModel GetAllHardware(int pageNumber, string searchText);
        
        public HardwareViewModel GetHardwareById(Guid id);
    }
}
