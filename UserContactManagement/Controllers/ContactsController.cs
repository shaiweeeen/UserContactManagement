using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using UserContactManagement.Areas.Identity.Data;
using UserContactManagement.Models;
using UserContactManagement.ViewModels;

namespace UserContactManagement.Controllers
{
    public class ContactsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager; 
        private string CurrentUserId
        {
            get
            {
                return _userManager.GetUserId(User);
            }
        }
        private static readonly Random random = new Random();
        private static readonly string[] avatarUrls = new string[]
        {
            "/images/avatars/1.png",
            "/images/avatars/2.png",
            "/images/avatars/3.png",
            "/images/avatars/4.png",
            "/images/avatars/5.png"
        };


        public ContactsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //get random avatar
        private string GetRandomAvatarUrl()
        {
            int randomNumber = random.Next(avatarUrls.Length);
            return avatarUrls[randomNumber];
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
           
            var contactViewModel = new ContactViewModel();
            if (CurrentUserId != null)
            {
                var associatedContacts = _context.Contacts
                   .Where(c => c.UserId == CurrentUserId)
                   .ToList();

                if (associatedContacts != null)
                {
                    var contactViewModels = associatedContacts.Select(contact => new ContactViewModel
                    {
                        Id = contact.Id,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber =contact.ContactNumber,
                        DeliveryAddress = contact.DeliveryAddress,
                        BillingAddress = contact.BillingAddress,
                        UserId = contact.UserId,
                        AvatarUrl = contact.AvatarUrl
                    }).ToList();

                    return View(contactViewModels);
                }
                else
                {
                    return View();
                }
            }
            else return Redirect("/Identity/Account/Login");


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
        public async Task<IActionResult> Create([Bind("FirstName,LastName,ContactNumber, DeliveryAddress,  BillingAddress")] ContactViewModel contact)
        {


            try
            {
                
                contact.UserId = CurrentUserId;

         

                
                contact.AvatarUrl = GetRandomAvatarUrl();
                if (ModelState.IsValid)
                {
                    Contact newContact = new Contact
                    {
                        Id = contact.Id,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        DeliveryAddress = contact.DeliveryAddress,
                        BillingAddress = contact.BillingAddress,
                        UserId = contact.UserId,
                        AvatarUrl = contact.AvatarUrl
                        
                    };
                    _context.Add(newContact);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Contact added successfully.";
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
            var contact = await _context.Contacts.FindAsync(id);
           

            if (contact != null)
            {
                var viewModel = new ContactViewModel
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    ContactNumber = contact.ContactNumber,
                    DeliveryAddress = contact.DeliveryAddress,
                    BillingAddress = contact.BillingAddress,
                    UserId = contact.UserId,
                    AvatarUrl = contact.AvatarUrl
                };
                return View(viewModel);
            }

            else
                return NotFound();

        }


        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Id,FirstName,LastName,ContactNumber,DeliveryAddress,BillingAddress")] ContactViewModel contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }
            else
            {
                
                // Band-aid: Fix this later!
               // ModelState.Remove(nameof(Contact.User));

                if (ModelState.IsValid)
                {
                    var existingContact = await _context.Contacts.FindAsync(id);
                    if (existingContact == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing contact
                    existingContact.Id = contact.Id;
                    existingContact.FirstName = contact.FirstName;
                    existingContact.LastName = contact.LastName;
                    existingContact.ContactNumber = contact.ContactNumber;
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
