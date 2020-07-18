using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MincePieRateV2.DAL.Repositories;
using MincePieRateV2.Models.Domain;
using MincePieRateV2.ViewModels.Domain;
using MincePieRateV2.Web.Authorization.Constants;

namespace MincePieRateV2.Web.Controllers
{
    [Route("Review")]
    public class ReviewController : Controller
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<MincePie> _mincePieRepository;
        private readonly IMapper _mapper;

        public ReviewController(IRepository<Review> reviewRepository, IRepository<MincePie> mincePieRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mincePieRepository = mincePieRepository;
            _mapper = mapper;
        }

        private void SetupDropdowns(ViewDataDictionary viewData)
        {
            viewData["MincePies"] = _mapper.Map<IEnumerable<MincePieDetailsViewModel>>(_mincePieRepository.GetEntities());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var reviews = _reviewRepository.GetEntities();
            return View(_mapper.Map<IEnumerable<ReviewViewModel>>(reviews));
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
        public IActionResult Add(ReviewViewModel reviewViewModel)
        {
            var review = _mapper.Map<Review>(reviewViewModel);
            _reviewRepository.Add(review);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Details/{Id:int}")]
        [AllowAnonymous]
        public IActionResult Details(int Id)
        {
            var review = _reviewRepository.GetEntity(m => m.Id == Id);
            return View(_mapper.Map<ReviewViewModel>(review));
        }

        [HttpGet("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(int Id)
        {
            SetupDropdowns(ViewData);
            var review = _reviewRepository.GetEntity(m => m.Id == Id);
            return View(_mapper.Map<ReviewViewModel>(review));
        }

        [HttpPost("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(ReviewViewModel reviewViewModel)
        {
            var review = _mapper.Map<Review>(reviewViewModel);
            _reviewRepository.Update(review);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Delete(int Id)
        {
            var review = _reviewRepository.GetEntity(m => m.Id == Id);
            return View(_mapper.Map<ReviewViewModel>(review));
        }

        [HttpPost("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Delete(ReviewViewModel reviewViewModel)
        {
            var review = _mapper.Map<Review>(reviewViewModel);
            _reviewRepository.Delete(review);
            return RedirectToAction(nameof(Index));
        }
    }
}
