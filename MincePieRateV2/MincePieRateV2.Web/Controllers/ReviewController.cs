using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MincePieRateV2.DAL.Repositories;
using MincePieRateV2.Models.Domain;
using MincePieRateV2.Web.Authorization.Constants;

namespace MincePieRateV2.Web.Controllers
{
    [Route("Review")]
    public class ReviewController : Controller
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<MincePie> _mincePieRepository;

        public ReviewController(IRepository<Review> reviewRepository, IRepository<MincePie> mincePieRepository)
        {
            _reviewRepository = reviewRepository;
            _mincePieRepository = mincePieRepository;
        }

        private void SetupDropdowns(ViewDataDictionary viewData)
        {
            viewData["MincePies"] = _mincePieRepository.GetEntities();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_reviewRepository.GetEntities());
        }

        [HttpGet("Add")]
        [Authorize]
        public IActionResult Add()
        {
            SetupDropdowns(ViewData);
            return View();
        }

        [HttpPost("Add")]
        [Authorize]
        public IActionResult Add(Review review)
        {
            _reviewRepository.Add(review);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Details/{Id:int}")]
        [AllowAnonymous]
        public IActionResult Details(int Id)
        {
            return View(_reviewRepository.GetEntity(m => m.Id == Id));
        }

        [HttpGet("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(int Id)
        {
            SetupDropdowns(ViewData);
            return View(_reviewRepository.GetEntity(m => m.Id == Id));
        }

        [HttpPost("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(Review review)
        {
            _reviewRepository.Update(review);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Delete(int Id)
        {
            return View(_reviewRepository.GetEntity(m => m.Id == Id));
        }

        [HttpPost("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Delete(Review review)
        {
            _reviewRepository.Delete(review);
            return RedirectToAction(nameof(Index));
        }
    }
}
