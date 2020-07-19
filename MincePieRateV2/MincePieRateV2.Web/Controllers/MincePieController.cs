using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MincePieRateV2.DAL.Managers;
using MincePieRateV2.DAL.Repositories;
using MincePieRateV2.Models.Domain;
using MincePieRateV2.ViewModels.Domain;
using MincePieRateV2.Web.Authorization.Constants;

namespace MincePieRateV2.Web.Controllers
{
    [Route("MincePie")]
    public class MincePieController : Controller
    {
        private readonly IRepository<MincePie> _mincePieRepository;
        private readonly IImageManager _imageManager;
        private readonly IMapper _mapper;

        public MincePieController(IRepository<MincePie> mincePieRepository, IImageManager imageManager, IMapper mapper)
        {
            _mincePieRepository = mincePieRepository;
            _imageManager = imageManager;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var mincePies = _mincePieRepository.GetEntities();
            return View(_mapper.Map<IEnumerable<MincePieDetailsViewModel>>(mincePies));
        }

        [HttpGet("Add")]
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> Add(MincePieCreateViewModel mincePieCreateViewModel)
        {
            Guid imageId;
            var mincePie = _mapper.Map<MincePie>(mincePieCreateViewModel);
            try
            {
                imageId = await _imageManager.AddImageAsync(mincePieCreateViewModel.Image);
                mincePie.ImageId = imageId;
            }
            catch(ArgumentException)
            {
                ModelState.AddModelError(nameof(mincePieCreateViewModel.Image), "Provided file must be an image");
                return View(mincePieCreateViewModel);
            }

            _mincePieRepository.Add(mincePie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Details/{Id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int Id)
        {
            var mincePie = _mincePieRepository.GetEntity(m => m.Id == Id);
            var mincePieViewModel = _mapper.Map<MincePieDetailsViewModel>(mincePie);
            mincePieViewModel.ImagePath = await SetupImagePath(mincePie);
            return View(mincePieViewModel);
        }

        [HttpGet("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int Id)
        {
            var mincePie = _mincePieRepository.GetEntity(m => m.Id == Id);
            var mincePieViewModel = _mapper.Map<MincePieDetailsViewModel>(mincePie);
            mincePieViewModel.ImagePath = await SetupImagePath(mincePie);
            return View(mincePieViewModel);
        }

        [HttpPost("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(MincePieDetailsViewModel mincePieViewModel)
        {
            var mincePie = _mapper.Map<MincePie>(mincePieViewModel);
            _mincePieRepository.Update(mincePie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int Id)
        {
            var mincePie = _mincePieRepository.GetEntity(m => m.Id == Id);
            var mincePieViewModel = _mapper.Map<MincePieDetailsViewModel>(mincePie);
            mincePieViewModel.ImagePath = await SetupImagePath(mincePie);
            return View(mincePieViewModel);
        }

        [HttpPost("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Delete(MincePieDetailsViewModel mincePieViewModel)
        {
            var mincePie = _mapper.Map<MincePie>(mincePieViewModel);
            _mincePieRepository.Delete(mincePie);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SetupImagePath(MincePie mincePie)
        {
            return await _imageManager.GetImagePathAsync(mincePie.ImageId);
        }
    }
}
