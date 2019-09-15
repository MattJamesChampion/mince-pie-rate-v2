using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MincePieRateV2.DAL.Repositories;
using MincePieRateV2.Web.Models.Domain;

namespace MincePieRateV2.Web.Controllers
{
    [Route("MincePie")]
    public class MincePieController : Controller
    {
        private readonly IRepository<MincePie> _mincePieRepository;

        public MincePieController(IRepository<MincePie> mincePieRepository)
        {
            _mincePieRepository = mincePieRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_mincePieRepository.GetEntities());
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public IActionResult Add(MincePie mincePie)
        {
            _mincePieRepository.Add(mincePie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Details/{Id:int}")]
        public IActionResult Details(int Id)
        {
            return View(_mincePieRepository.GetEntity(m => m.Id == Id));
        }
    }
}
