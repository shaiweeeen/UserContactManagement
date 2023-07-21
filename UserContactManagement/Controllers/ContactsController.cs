using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserContactManagement.Areas.Identity.Data;
using UserContactManagement.Models;

namespace UserContactManagement.Controllers
{
    public class ContactsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;


        public ContactsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var associatedContacts = _context.Contacts
                   .Where(c => c.UserId == currentUser.Id)
                   .ToList();

                if (associatedContacts != null)
                    return View(associatedContacts);
                else return View();
            }
            else return RedirectToAction("Login", "Identity//Account");


        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
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

        // GET: Contacts/Create
        public IActionResult Create()
        {

            return View();

        }


        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName, DeliveryAddress, BillingAddress")] Contact contact)
        {


            try
            {
                User currentUser = await _userManager.GetUserAsync(User);
                contact.UserId = currentUser.Id;

                // Band-aid: Fix this later! Create ViewModels
                ModelState.Remove(nameof(Contact.User)); 
                ModelState.Remove(nameof(Contact.UserId));

                if (ModelState.IsValid)
                {

                    _context.Add(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the contact.");
                
                return View(contact);
            }




            return View();
        }
        // GET: Contacts/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var editContact = await _context.Contacts.FindAsync(id);
            if (editContact != null)
            {
                return View(editContact);
            }

            else
                return NotFound();

        }


        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Id,FirstName,LastName,DeliveryAddress,BillingAddress")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }
            else
            {
                // Band-aid: Fix this later!
                ModelState.Remove(nameof(Contact.User));

                if (ModelState.IsValid)
                {
                    var existingContact = await _context.Contacts.FindAsync(id);
                    if (existingContact == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing contact
                    existingContact.FirstName = contact.FirstName;
                    existingContact.LastName = contact.LastName;
                    existingContact.DeliveryAddress = contact.DeliveryAddress;
                    existingContact.BillingAddress = contact.BillingAddress;

                    // Make sure to set the UserId property as well
                    existingContact.UserId = contact.UserId;

                    // Save the changes to the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                return View(contact);
            }

        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
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

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'AppDbContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
