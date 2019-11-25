using System;
using System.Collections.Generic;
using System.Linq;
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
            AwardFormViewModel vm = new AwardFormViewModel();
            vm.AgencyName = Agency;
            vm.NomineeName = NomineeName;
            vm.ClassTitle = ClassTitle;
            vm.Division = Division;
            vm.Components = _repository.Components.ToList();
            vm.Users = _repository.Users.ToList();

            ViewData["Title"] = "Create Award Form";
            return View(vm);
        }
    }
}