﻿using E_commerce.Models;
using E_commerce.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_commerce.Models.ViewModels;
using E_commerce.ViewModel;

namespace e_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
       private IRepository<User> users;
       private IRepository<Place> places;


        public UsersController(IRepository<User> usersRepository, IRepository<Place> placesRepository) {
           this.users = usersRepository; 
              this.places = placesRepository;
        } 
        public IActionResult Index(int page=1)
        {
            const int pageSize = 10;
            if (page < 1)
                page = 1;
            int Count = users.entities.Count();
            var pagingInfo = new PagingInfo(Count, page, pageSize);
            pagingInfo.PageName = "Users";
          int  recSkip = (page - 1) * pageSize;
            var data = users.entities.Skip(recSkip).Take(pagingInfo.ItemsPerPage).ToList();
            this.ViewBag.PagingInfo = pagingInfo;
            return View(data);
        }
        public IActionResult Create()
        {
            var model = new UserViewModel {           
                places = places.entities.ToList()
            };
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(UserViewModel userViewModel)
        {           
            // try{
                    users.Add(userViewModel.user);
                    return RedirectToAction("Index");
              
            // }
            // catch(Exception ex)
            // {
            //     return View("Error", new ErrorViewModel { RequestId = ex.Message });
            // }
        }
        public IActionResult getUser(string q)
        {
            IEnumerable<SelectListItem> usersList = Enumerable.Empty<SelectListItem>();
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
                usersList = users.entities.Where(u => u.Name.Contains(q)).Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id
                    }
                    );
            return Json(new { items = usersList });
        }
    }
}
