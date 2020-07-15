﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MincePieRateV2.DAL.Managers;
using MincePieRateV2.DAL.Repositories;
using MincePieRateV2.Models.Domain;
using MincePieRateV2.Web.Authorization.Constants;

namespace MincePieRateV2.Web.Controllers
{
    [Route("MincePie")]
    public class MincePieController : Controller
    {
        private readonly IRepository<MincePie> _mincePieRepository;
        private readonly IImageManager _imageManager;

        public MincePieController(IRepository<MincePie> mincePieRepository, IImageManager imageManager)
        {
            _mincePieRepository = mincePieRepository;
            _imageManager = imageManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_mincePieRepository.GetEntities());
        }

        [HttpGet("Add")]
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> Add(MincePie mincePie, [FromForm] IFormFile mincePieImage)
        {
            Guid imageId;
            try
            {
                imageId = await _imageManager.AddImageAsync(mincePieImage);
                mincePie.ImageId = imageId;
            }
            catch(ArgumentException)
            {
                ModelState.AddModelError(string.Empty, "Provided file must be an image");
                return View(mincePie);
            }

            _mincePieRepository.Add(mincePie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Details/{Id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int Id)
        {
            var mincePie = _mincePieRepository.GetEntity(m => m.Id == Id);
            ViewData["ImagePath"] = await SetupImagePath(mincePie);
            return View(mincePie);
        }

        [HttpGet("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int Id)
        {
            var mincePie = _mincePieRepository.GetEntity(m => m.Id == Id);
            ViewData["ImagePath"] = await SetupImagePath(mincePie);
            return View(mincePie);
        }

        [HttpPost("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(MincePie mincePie)
        {
            _mincePieRepository.Update(mincePie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int Id)
        {
            var mincePie = _mincePieRepository.GetEntity(m => m.Id == Id);
            ViewData["ImagePath"] = await SetupImagePath(mincePie);
            return View(mincePie);
        }

        [HttpPost("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Delete(MincePie mincePie)
        {
            _mincePieRepository.Delete(mincePie);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SetupImagePath(MincePie mincePie)
        {
            return await _imageManager.GetImagePathAsync(mincePie.ImageId);
        }
    }
}
