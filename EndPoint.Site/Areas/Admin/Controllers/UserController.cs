using GymTest.Application.Services.Users.Command;
using GymTest.Application.Services.Users.Query;
using GymTest.Application.Services.Users.Query.GetDataForUpdateUser;
using GymTest.Application.Services.Users.Query.GetUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IGetUsers _getUsers;
        private readonly IDeleteUser _deleteUser;

        private readonly IUpdateUser _updateUser;
        private readonly IGetDataForUpdateUser _getDataForUpdateUser;

        public UserController(
            IGetUsers getUsers,
            IDeleteUser deleteUser,
            
            IUpdateUser updateUser,
            IGetDataForUpdateUser getDataForUpdateUser
            
            )
        {
            _getUsers = getUsers;
            _deleteUser = deleteUser;

            _updateUser = updateUser;
            _getDataForUpdateUser = getDataForUpdateUser;


        }


        // GET: UserController1
        
        public ActionResult Index(string searchKey = "", int page = 1)
        {
            var request = new RequestGetUserDto()
            {
                PageNumber = page,
                SearchKey = searchKey
            };
            var result = _getUsers.Execute(request);
            return View(result);
        }

        // GET: UserController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController1/Edit/5
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var result = _getDataForUpdateUser.Execute(id);
            var data = result.Data;

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return View(data);
        }

        // POST: UserController1/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit( RequestUpdateDto request)
        {
            var result = _updateUser.Execute(request);

            return Json(result);
            
        }

        // GET: UserController1/Delete/5
        /*
        public ActionResult Delete(int id)
        {
            return View();
        }
        */
        // POST: UserController1/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        
        public ActionResult Delete(long id)
        {

                var result = _deleteUser.Execute(new RequestDeleteDto()
                {
                    UserId = id
                });

                return Json(result);

        }
    }
}
