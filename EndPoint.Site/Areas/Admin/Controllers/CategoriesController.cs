using GymTest.Application.Services.Categories.Command;
using GymTest.Application.Services.Categories.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IGetCategories _getCategories;
        private readonly IAddNewCategory _addNewCategory;
        private readonly IRemoveCategory _removeCategory;
        private readonly IEditCategory _editCategory;
        private readonly IGetDataForEditCategory _getDataForEditCategory;
        


        public CategoriesController(
            
            IGetCategories getCategories,
            IAddNewCategory addNewCategory,
            IRemoveCategory removeCategory,
            IEditCategory editCategory,
            IGetDataForEditCategory getDataForEditCategory
            )
        {

            _getCategories = getCategories;
            _addNewCategory = addNewCategory;
            _removeCategory = removeCategory;
            _editCategory = editCategory;
            _getDataForEditCategory = getDataForEditCategory;
        }
        // GET: CategoryController
        public ActionResult Index(string searchKey = "", int pageNumber = 1)
        {
            var request = new RequestGetCategoriesDto()
            {
                PageNumber = pageNumber,
                SearchKey = searchKey
            };
            var actionResult = _getCategories.Execure(request);
            return View(actionResult);
        }

        // GET: CategoryController/Create
        [HttpGet]
        public ActionResult Create()
        {

            // ۱. نتیجه کامل را از سرویس بگیر
            var categoriesResult = _getCategories.Execure(new RequestGetCategoriesDto { PageNumber = 1, SearchKey = "" });

            // ۲. لیست دسته‌بندی‌ها را مستقیماً از پراپرتی Categories بخوان
             var parentCategories = categoriesResult.Categories;

            // ۳. SelectList را بساز
            //    این کد همچنان خطا می‌دهد چون GetCategoryDto شما Id ندارد!
            ViewBag.Categories = new SelectList(parentCategories, "Id", "Name");

                
            
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(long? parentId , String name )
        {

            var request = new RequestAddNewCategory()
            {
                Name = name,
                ParentId = parentId
            };
            var result = _addNewCategory.Execute(new RequestAddNewCategory()
            {
                Name = request.Name,
                ParentId = request.ParentId

            });
            return Json(result);
        }





        // POST: CategoryController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var request = new RequestRemoveCategory()
            {
                CategoryId = id
            };

            var result = _removeCategory.Execute(request);
            return Json(result);
        }







        // GET: CategoryController/Edit/5
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var request = new RequestDataForEditCategory()
            {
                CategoryId = id
            };

            var result = _getDataForEditCategory.Execute(request);
            if (result.IsSuccess == false)
            {
                return NotFound();
            }

            var allCategories = _getCategories.Execure(new RequestGetCategoriesDto { PageNumber = 1, SearchKey = "" }).Categories; //{ PageNumber = 1, SearchKey = "" });
            ViewBag.Categories = new SelectList(allCategories, "Id", "Name", result.Data.ParentId);



            return View(result.Data);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(RequestEditCategoryDto request)
        {
            var result = _editCategory.Execute(request);

            return Json(result);
        }




        /*
        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        

        // به دیلیت گت نیازی نیست فقط پست
        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }


        */
    }
}
