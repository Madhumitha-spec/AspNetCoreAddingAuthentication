using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WishList.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WishList.Models;
using System;
using System.Collections.Generic;

namespace WishList.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var User = _userManager.GetUserAsync(HttpContext.User);
            List<Item> model = new List<Item>();
                model= _context.Items.Where(x=>x.Id==User.Id).ToList<Item>();
            
            if (model!=null && model.Count()==1)
            {
                return View("Index", model);
            }
            else
            {
                return View("Index", model);
            }
           
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Models.Item item)
        {
            var user = this._userManager.GetUserAsync(HttpContext.User);
            
            _context.Items.Add(item);            
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
            if(user.Id==id) {
                var item = _context.Items.FirstOrDefault(e => e.Id == id);
                _context.Items.Remove(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }

            
            
        }
    }
}
