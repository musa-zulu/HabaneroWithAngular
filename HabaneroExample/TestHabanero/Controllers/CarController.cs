using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using TestHabanero.BO;
using TestHabanero.Models;
using TestHabenaro.Db.Interfaces;

namespace TestHabanero.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IMappingEngine _mapperEngine;


        public CarController(ICarRepository carRepository, IMappingEngine mapperEngine)
        {
            if (carRepository == null) throw new ArgumentNullException(nameof(carRepository));
            if (mapperEngine == null) throw new ArgumentNullException(nameof(mapperEngine));
            _carRepository = carRepository;
            _mapperEngine = mapperEngine;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllCars()
        {
            var cars = _carRepository.GetCars();
            var viewModel = new List<CarViewModel>();
            if (cars != null)
            {
                viewModel = _mapperEngine.Map<List<Car>, List<CarViewModel>>(cars);
            }
            return new JsonResult { Data = viewModel, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
        /*
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                carViewModel.CarId = Guid.NewGuid();
                var car = _mapperEngine.Map<Car>(carViewModel);
                _carRepository.Save(car);
                return RedirectToAction("Index");
            }
            return View(carViewModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var car = _carRepository.GetCarBy(id);
            var carViewModel = _mapperEngine.Map<CarViewModel>(car);
            return View(carViewModel);
        }

        [HttpPost]
        public ActionResult Edit(CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingCar = _carRepository.GetCarBy(carViewModel.CarId);
                var car = _mapperEngine.Map<Car>(carViewModel);
                _carRepository.Update(existingCar, car);
                return RedirectToAction("Index");
            }
            return View(carViewModel);
        }*/


        public ActionResult Delete(Guid id)
        {
            var car = _carRepository.GetCarBy(id);
            _carRepository.Delete(car);
            return RedirectToAction("Index");
        }
    }
}