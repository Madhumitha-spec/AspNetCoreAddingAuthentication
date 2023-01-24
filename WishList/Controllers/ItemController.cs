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
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            
                var model= _context.Items.Where(x=>x.User.Id==user.Id).ToList();
            
            
            
                return View("Index", model);
            
           
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            var user = this._userManager.GetUserAsync(HttpContext.User).Result;
            item.User=user;
            _context.Items.Add(item);            
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            
                var item = _context.Items.FirstOrDefault(e => e.Id == id);
                if(item.User!=user) {
                return Unauthorized();
                }
                else{
                _context.Items.Remove(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
                }
            
           

            
            
        }
    }
}
