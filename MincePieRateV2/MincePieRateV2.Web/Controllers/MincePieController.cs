using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var imageId = await _imageManager.AddImageAsync(mincePieImage);
            mincePie.ImageId = imageId;

            _mincePieRepository.Add(mincePie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Details/{Id:int}")]
        [AllowAnonymous]
        public IActionResult Details(int Id)
        {
            return View(_mincePieRepository.GetEntity(m => m.Id == Id));
        }

        [HttpGet("Edit/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Edit(int Id)
        {
            return View(_mincePieRepository.GetEntity(m => m.Id == Id));
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
        public IActionResult Delete(int Id)
        {
            return View(_mincePieRepository.GetEntity(m => m.Id == Id));
        }

        [HttpPost("Delete/{Id:int}")]
        [Authorize(Roles = RoleConstants.AdministratorRoleName)]
        public IActionResult Delete(MincePie mincePie)
        {
            _mincePieRepository.Delete(mincePie);
            return RedirectToAction(nameof(Index));
        }
    }
}
