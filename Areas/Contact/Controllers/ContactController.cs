using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using Microsoft.AspNetCore.Authorization;
using ContactModel = App.Models.ContactInfo.Contact;

namespace AppMvc.Net.Areas.Contact.Controllers
{
    [Area("Contact")]
    public class ContactController : Controller
    {
        private readonly MvcMovieContext _context;

        public ContactController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Contact/Contact
        [HttpGet("/admin/contact")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.ToListAsync());
        }

        // GET: Contact/Contact/Details/5
        [HttpGet("/admin/contact/detail/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contact/Contact/Create
        [HttpGet("/contact/")]
        [AllowAnonymous]
        public IActionResult SendContact()
        {
            return View();
        }

        [TempData]
        string StatusMessage { get; set; }

        // POST: Contact/Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/contact/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendContact([Bind("FullName,Email,DateSent,Message,Phone")] ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("AAAA");
                contact.DateSent = DateTime.Now;
                _context.Add(contact);
                await _context.SaveChangesAsync();

                StatusMessage = "Your contact was sent";
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine("BBBB");
            return View(contact);
        }

        // GET: Contact/Contact/Delete/5
        [HttpGet("/admin/contact/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contact/Contact/Delete/5
        [HttpPost("/admin/contact/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
