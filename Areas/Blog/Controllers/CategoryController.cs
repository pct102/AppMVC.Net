using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models.Blog;
using MvcMovie.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppMvc.Net.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("admin/blog/category/{action}/{id?}")]
    public class CategoryController : Controller
    {
        private readonly MvcMovieContext _context;

        public CategoryController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Blog/Category
        public async Task<IActionResult> Index()
        {
            // var mvcMovieContext = _context.Categories.Include(c => c.ParentCategory);
            // return View(await mvcMovieContext.ToListAsync());

            // var query = _context.Categories
            //     .Include(x => x.ParentCategory)
            //     .Include(x => x.CategoryChildren);

            // var categories = (await query.ToListAsync())
            //                 .Where(x => x.ParentCategory == null)
            //                 .ToList();

            // return View(categories);

            var categories = await _context.Categories
                .Include(x => x.ParentCategory)
                .Include(x => x.CategoryChildren)
                    .ThenInclude(child => child.CategoryChildren)
                .Where(x => x.ParentCategory == null)
                .ToListAsync();

            return View(categories);
        }

        // GET: Blog/Category/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        private void CreateSelectList(List<Category> source, List<Category> des, int level){
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in source)
            {
                des.Add(new Category(){
                    Id = category.Id,
                    Title = prefix + " " + category.Title,
                    Slug = category.Slug,
                    Description = category.Description
                });
                if(category.CategoryChildren?.Count > 0){
                    CreateSelectList(category.CategoryChildren.ToList(), des, level + 1);
                }
            }
        }

        private async Task<SelectList> CreateParentSelectItems(){
            var categories = await _context.Categories
                .Include(x => x.ParentCategory)
                .Include(x => x.CategoryChildren)
                    .ThenInclude(child => child.CategoryChildren)
                .Where(x => x.ParentCategory == null)
                .ToListAsync();

            categories.Insert(0, new Category(){
                ParentCategoryId = null,
                Title = "No parent category",
                Description = "",
                Slug = ""
            });

            var items = new List<Category>();
            CreateSelectList(categories, items, 0);
            var selectLists = new SelectList(items, "Id", "Title");
            return selectLists;
        }

        // GET: Blog/Category/Create
        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            var selectLists = await CreateParentSelectItems();

            ViewData["ParentCategoryId"] = selectLists;
            ViewData["Id"] = Guid.NewGuid().ToString();
            return View();
        }

        // POST: Blog/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentCategoryId,Title,Description,Slug")] Category category)
        {
            Console.WriteLine("AAA " + category.Id);
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Any())
                    {
                        var fieldName = entry.Key;
                        var errorMessage = entry.Value.Errors.First().ErrorMessage;
                        // Xử lý thông tin lỗi, ví dụ: đưa vào ViewBag để hiển thị ở view
                        ViewBag.ErrorMessage = errorMessage;
                        // hoặc bạn có thể thực hiện các hành động xử lý khác
                        Console.WriteLine("EEE " + errorMessage);
                    }
                }
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentCategoryId);
            return View(category);
        }

        // GET: Blog/Category/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            
            var selectLists = await CreateParentSelectItems();
            ViewData["ParentCategoryId"] = selectLists;
            return View(category);
        }

        // POST: Blog/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ParentCategoryId,Title,Description,Slug")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if(category.ParentCategoryId == id){
                ModelState.AddModelError(string.Empty, "ParentCategory should diff with current Category");
            }

            if (ModelState.IsValid && category.ParentCategoryId != id)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var selectLists = await CreateParentSelectItems();
            ViewData["ParentCategoryId"] = selectLists;
            return View(category);
        }

        // GET: Blog/Category/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Blog/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var category = await _context.Categories
                            .Include(c => c.CategoryChildren)
                            .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            
            foreach (Category cCategory in category.CategoryChildren)
            {
                cCategory.ParentCategoryId = category.ParentCategoryId;
            }
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
