using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;

namespace AppMvc.Net.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("database-manage/{action}")]
    public class DbManageController : Controller
    {
        private readonly MvcMovieContext _dbContext;

        public DbManageController(MvcMovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: DbManageController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteDb(){
            return View();
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync(){
            var success = await _dbContext.Database.EnsureDeletedAsync();

            StatusMessage = success ? "Delete Db success" : "Delete Db failure";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Migrate(){
            await _dbContext.Database.MigrateAsync();

            StatusMessage = "Update Migration success!";

            return RedirectToAction(nameof(Index));
        }

    }
}
