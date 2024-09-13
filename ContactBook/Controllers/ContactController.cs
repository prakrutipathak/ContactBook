using ContactBook.Data;
using ContactBook.Models;
using ContactBook.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Controllers
{
 
    public class ContactController : Controller
    {
       
        private readonly IContactService _contactService;
        public ContactController( IContactService contactService)
        {
            _contactService = contactService;
        }
        //public IActionResult Index()
        //{
        //    var contacts = _contactService.GetContacts();
        //    if (contacts != null && contacts.Any())
        //    {
        //        return View(contacts);
        //    }
        //    return View(new List<Contact>());
        //}
        public IActionResult Index(char? letter,int page = 1, int pageSize = 2)
        {
            ViewBag.CurrentPage = page; // Pass the current page number to the ViewBag
                                        // Get total count of categories
            var totalCount = _contactService.TotalContacts(letter);
            // Calculate total number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            if (page > totalPages)
            {
                return RedirectToAction("Index", new { page = totalPages, pageSize });
            }
            var contacts = letter == null

       ? _contactService.GetPaginatedContacts(page, pageSize)
       : _contactService.GetPaginatedContacts(page, pageSize, letter);
            
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.Letter = letter;
            return View(contacts);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contact = _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                var message = _contactService.ModifyContact(contact);
                if (message == "Contact Exists!" || message == "Something went wrong please try after sometime")
                {
                    TempData["ErrorMessage"] = message;
                }

                else
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");

                }
            }
            return View(contact);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                var result = _contactService.AddContact(contact);
                if (result == "Contact already exists." || result == "Something went wrong after sometime")
                {
                    TempData["ErrorMessage"] = result;
                }
                else if (result == "Contact saved successfully")
                {
                    TempData["SuccessMessage"] = result;
                    return RedirectToAction("Index");
                }
            }
            return View(contact);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var contact = _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);


        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var contact = _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int ContactId)
        {
            var result = _contactService.RemoveContact(ContactId);
            if (result == "Contact Deleted Successfully")
            {
                TempData["SuccessMessage"] = result;
            }
            else
            {
                TempData["ErrorMessage"] = result;
            }
            return RedirectToAction("Index");

        }
       
    }
}
