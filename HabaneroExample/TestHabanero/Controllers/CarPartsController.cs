using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TestHabanero.BO;
using TestHabanero.Models;
using TestHabenaro.Db.Interfaces;
using TestHabenaro.DB;

namespace TestHabanero.Controllers
{
    public class CarPartsController : Controller
    {

        private readonly ICarPartRepository _carPartRepository;
        private readonly ICarRepository _carRepository;
        private readonly IPartRepository _PartRepository;
        private readonly IMappingEngine _mappingEngine;

        public CarPartsController(ICarPartRepository carPartRepository,ICarRepository carRepository,IPartRepository partRepository, IMappingEngine mappingEngine)
        {
            if (carPartRepository == null) throw new ArgumentNullException(nameof(carPartRepository));
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
            if (partRepository == null) throw new ArgumentNullException(nameof(partRepository));
            if (carRepository == null) throw new ArgumentNullException(nameof(carRepository));
            _carPartRepository = carPartRepository;
            _PartRepository = partRepository;
            _carRepository = carRepository;
            _mappingEngine = mappingEngine;
        }

        private List<SelectListItem> GetAllParts(Guid? partId = null)
        {
            var selectListItems = _PartRepository.GetParts()
                .Select(t => new SelectListItem { Value = t.PartId.ToString(), Text = t.Description, Selected = t.PartId == partId.GetValueOrDefault() });
            return selectListItems.ToList();
        }
        private List<SelectListItem> GetAllCars(Guid? carId = null)
        {
            var selectListItems = _carRepository.GetCars()
                .Select(t => new SelectListItem { Value = t.CarId.ToString(), Text = t.Make, Selected = t.CarId == carId.GetValueOrDefault() });
            return selectListItems.ToList();
        }
        // GET: CarParts
        public ActionResult Index()
        {
            var carParts = _carPartRepository.GetCarPart();
            var viewModel = new List<CarPartsIndexViewModel>();
            if (carParts != null)
            {
                viewModel = _mappingEngine.Map<List<CarPart>, List<CarPartsIndexViewModel>>(carParts);
            }
            return View(viewModel);
        }

      

        // GET: CarParts/Create
        public ActionResult Create()
        {
            var viewModel = new CarPartsViewModel()
            {
                CarSelectListItems = GetAllCars(),
                PartsSelectListItems = GetAllParts(),
            };

            return View(viewModel);
        }

        // POST: CarParts/Create
        [HttpPost]
        public ActionResult Create(CarPartsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var carPart = new CarPart
                {
                    CarId = viewModel.CarId,
                    PartId = viewModel.PartId,
                    Quantity = viewModel.Quantity,
                    CarPartId = Guid.NewGuid()
                };
                _carPartRepository.Save(carPart);
                return RedirectToAction("Index");
            }

            viewModel.CarSelectListItems = GetAllCars();
            viewModel.PartsSelectListItems = GetAllParts();
           
            return View(viewModel);
        }

        // GET: CarParts/Edit/5
        public ActionResult Edit(Guid id)
        {
            var carPart = _carPartRepository.GetCarPartBy(id);
            var carPartsViewModel = _mappingEngine.Map<CarPartsViewModel>(carPart);
            carPartsViewModel.CarSelectListItems = GetAllCars(carPartsViewModel.CarId);
            carPartsViewModel.PartsSelectListItems = GetAllParts(carPartsViewModel.PartId);
            return View(carPartsViewModel);
        }

        // POST: CarParts/Edit/5
        [HttpPost]
        public ActionResult Edit(CarPartsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var carPart = _carPartRepository.GetCarPartBy(viewModel.CarPartId);
                var newCar = new CarPart
                {
                    CarId = viewModel.CarId,
                    PartId = viewModel.PartId,
                    Quantity = viewModel.Quantity,
                    CarPartId = Guid.NewGuid()
                };
                _carPartRepository.Update(carPart, newCar);
                return RedirectToAction("Index");
            }
            viewModel.CarSelectListItems = GetAllCars(viewModel.CarId);
            viewModel.PartsSelectListItems = GetAllParts(viewModel.PartId);
            return View(viewModel);

        }



        // POST: CarParts/Delete/5
        public ActionResult Delete(Guid id)
        {
            var car = _carPartRepository.GetCarPartBy(id);
            _carPartRepository.Delete(car);
            return RedirectToAction("Index");
        }
    }
}
