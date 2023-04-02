using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBookWeb.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;



        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
      
        }

        public IActionResult Index()
        {

            return View();
        }
        //GET
        public IActionResult Upsert(int? id)
        {
            Company company = new();
           
            ViewBag.Id = id;

            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }        
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
            }
            if (company.Id == 0)
            {
                _unitOfWork.Company.Add(company);

            }
            else
            {
                _unitOfWork.Company.Update(company);
                TempData["success"] = company.Id == 0 ? "Company created successfully" : "Company updated successfully";
                return RedirectToAction("Index");
            }

            _unitOfWork.Save();
            return View(company);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });

        }
        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {

                return Json(new { success = false, message = "Error while deleting" });
            }
           
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });

        }
        #endregion
    }
}
