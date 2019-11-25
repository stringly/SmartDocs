using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
    [Authorize(Roles = "User, Administrator")]
    public class AwardController : Controller
    {
        private IDocumentRepository _repository;


        public AwardController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(string NomineeName, string ClassTitle, string Division, string Agency = "Prince George's County Police Department")
        {
            int UserId = 0;
            if(User.HasClaim(x => x.Type == "UserId"))
            {
                UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);            
                AwardFormViewModel vm = new AwardFormViewModel();
                vm.AuthorUserId = UserId;
                vm.AgencyName = Agency;
                vm.NomineeName = NomineeName;
                vm.ClassTitle = ClassTitle;
                vm.Division = Division;
                vm.Components = _repository.Components.ToList();
                vm.Users = _repository.Users.ToList();

                ViewData["Title"] = "Create Award Form";
                return View(vm);
            }
            else
            {
                return RedirectToAction("Access Denied", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AwardFormViewModel form)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Create Award Form: Error";
                return View(form);
            }
            else
            {
                // do Award Form Factory stuff
            }
        }

        public IActionResult GetAwardFormViewComponent(int awardId)
        {
            AwardType award;

            switch (awardId)
            {
                case 1:
                    award = new GoodConductAward();
                    break;
                case 2:
                    award = new OutstandingPerformanceAward();
                    break;
                default:
                    return NotFound();
            }
            if(award != null)
            {
                // return the ViewComponent
                return ViewComponent("AwardTypeForm", award);
            }
            else
            {
                return NotFound();
            }
        }
    }
}