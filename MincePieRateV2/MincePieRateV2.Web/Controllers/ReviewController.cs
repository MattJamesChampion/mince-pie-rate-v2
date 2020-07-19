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
            return View(_mapper.Map<IEnumerable<ReviewDetailsViewModel>>(reviews));
        }

        [HttpGet("Add")]
        [Authorize]
        public IActionResult Add()
        {
            var reviewViewModel = new ReviewCreateEditViewModel
            {
                MincePies = _mapper.Map<IEnumerable<MincePieDetailsViewModel>>(_mincePieRepository.GetEntities())
            };
            return View(reviewViewModel);
        }

        [HttpPost("Add")]
        [Authorize]
        public IActionResult Add(ReviewCreateEditViewModel reviewViewModel)
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
            return View(_mapper.Map<ReviewDetailsViewModel>(review));
        }

        [HttpGet("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(int Id)
        {
            var review = _reviewRepository.GetEntity(m => m.Id == Id);
            var reviewViewModel = _mapper.Map<ReviewCreateEditViewModel>(review);
            reviewViewModel.MincePies = _mapper.Map<IEnumerable<MincePieDetailsViewModel>>(_mincePieRepository.GetEntities());
            return View(reviewViewModel);
        }

        [HttpPost("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(ReviewCreateEditViewModel reviewViewModel)
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
            return View(_mapper.Map<ReviewDetailsViewModel>(review));
        }

        [HttpPost("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Delete(ReviewDetailsViewModel reviewViewModel)
        {
            var review = _mapper.Map<Review>(reviewViewModel);
            _reviewRepository.Delete(review);
            return RedirectToAction(nameof(Index));
        }
    }
}
