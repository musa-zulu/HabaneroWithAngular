using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using TestHabanero.BO;
using TestHabanero.Models;
using TestHabenaro.Db.Interfaces;

namespace TestHabanero.Controllers
{
    public class PartController : Controller
    {
        private readonly IPartRepository _partRepository;
        private readonly IMappingEngine _mapperEngine;


        public PartController(IPartRepository partRepository, IMappingEngine mappingEngine)
        {
            if (partRepository == null) throw new ArgumentNullException(nameof(partRepository));
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
            _partRepository = partRepository;
            _mapperEngine = mappingEngine;
        }

        public ActionResult Index()
        {
            var parts = _partRepository.GetParts();
            var viewModel = new List<PartViewModel>();
            if (parts != null)
            {
                viewModel = _mapperEngine.Map<List<Part>, List<PartViewModel>>(parts);
            }
            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PartViewModel partViewModel)
        {
            if (ModelState.IsValid)
            {
                partViewModel.PartId = Guid.NewGuid();
                var car = _mapperEngine.Map<Part>(partViewModel);
                _partRepository.Save(car);
                return RedirectToAction("Index");
            }
            return View(partViewModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var car = _partRepository.GetPartBy(id);
            var carViewModel = _mapperEngine.Map<PartViewModel>(car);
            return View(carViewModel);
        }

        [HttpPost]
        public ActionResult Edit(PartViewModel partViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingPart = _partRepository.GetPartBy(partViewModel.PartId);
                var part = _mapperEngine.Map<Part>(partViewModel);
                _partRepository.Update(existingPart, part);
                return RedirectToAction("Index");
            }
            return View(partViewModel);
        }


        public ActionResult Delete(Guid id)
        {
            var car = _partRepository.GetPartBy(id);
            _partRepository.Delete(car);
            return RedirectToAction("Index");
        }

    }
}