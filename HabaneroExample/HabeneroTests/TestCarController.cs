using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Habanero.Testability;
using NSubstitute;
using NUnit.Framework;
using TestHabanero.Bootstrap.Mappings;
using TestHabanero.BO;
using TestHabanero.Controllers;
using TestHabanero.Models;
using TestHabanero.Tests.Commons;
using TestHabenaro.Db.Interfaces;

namespace TestHabenero.Tests
{
    [TestFixture]
    public class TestCarController
    {
        private CarControllerBuilder CreateBuilder()
        {
            return new CarControllerBuilder();
        }

        [Test]
        public void Construct()
        {
            Assert.DoesNotThrow(
                () => new CarController(Substitute.For<ICarRepository>(), Substitute.For<IMappingEngine>()));
        }

        [Test]
        public void Construct_GivenICarRepositoryIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarController(null, Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("carRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMappingEngineIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarController(Substitute.For<ICarRepository>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("mapperEngine", ex.ParamName);
        }

        //[Test]
        //public void Create_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
        //{
        //    //---------------Set up test pack-------------------
        //    var viewModel = Substitute.For<CarViewModel>();
        //    var mappingEngine = Substitute.For<IMappingEngine>();
        //    var carController = CreateBuilder()
        //        .WithMappingEngine(mappingEngine)
        //        .Build();
        //    //---------------Assert Precondition----------------
        //    Assert.IsTrue(carController.ModelState.IsValid);
        //    //---------------Execute Test ----------------------
        //    var result = carController.Create(viewModel) as ViewResult;
        //    //---------------Test Result -----------------------
        //    mappingEngine.Received().Map<Car>(viewModel);
        //}

        //[Test]
        //public void Create_POST_GivenModelStateIsValid_ShouldCallSave()
        //{
        //    //---------------Set up test pack-------------------
        //    var viewModel =Substitute.For<CarViewModel>();

        //    var repository = Substitute.For<ICarRepository>();
        //    var mapperEngine = Substitute.For<IMappingEngine>();
        //    var car = new CarBuilder().WithNewId().Build();
        //    mapperEngine.Map<Car>(viewModel).Returns(car);
        //    var carController = CreateBuilder()
        //        .WithCarRepository(repository)
        //        .WithMappingEngine(mapperEngine)
        //        .Build();
        //    //---------------Assert Precondition----------------
        //    Assert.IsTrue(carController.ModelState.IsValid);
        //    //---------------Execute Test ----------------------
        //    var result = carController.Create(viewModel) as ViewResult;
        //    //---------------Test Result -----------------------
        //    repository.Received().Save(car);
        //}

        //[Test]
        //public void Create_ShouldReturnViewModel()
        //{
        //    //---------------Set up test pack-------------------
        //    var carController = CreateBuilder()
        //        .Build();
        //    //---------------Assert Precondition----------------
        //    //---------------Execute Test ----------------------
        //    var result = carController.Create() as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //}

        [Test]
        public void Index_GivenAllUsersReturnedFromRepository_ShouldReturnViewModel()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().Build();         
            var cars = new List<Car> { car };
            var repository = Substitute.For<ICarRepository>();
            var mappingEngine = ResolveMapper();
            repository.GetCars().Returns(cars);
            var carController = CreateBuilder()
                .WithCarRepository(repository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = carController.Index() as JsonResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Data as List<CarViewModel>;
            Assert.IsInstanceOf<List<CarViewModel>>(model);
            Assert.AreEqual(car.CarId, model.FirstOrDefault().CarId);
            Assert.AreEqual(car.Model, model.FirstOrDefault().Model);
            Assert.AreEqual(car.Make, model.FirstOrDefault().Make);
        }

        [Test]
        public void Index_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().Build();
            var cars = new List<Car> { car };
            var carRepository = Substitute.For<ICarRepository>();
            carRepository.GetCars().Returns(cars);
            var mappingEngine = Substitute.For<IMappingEngine>();

            var carController = CreateBuilder()
                .WithCarRepository(carRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = carController.Index() as JsonResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<List<Car>, List<CarViewModel>>(cars);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var carController = CreateBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = carController.Index() as JsonResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        //[Test]
        //public void Create_POST_ShouldReturnToAction()
        //{
        //    //---------------Set up test pack-------------------
        //    var viewModel = Substitute.For<CarViewModel>();
        //    var carController = CreateBuilder().Build();
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = carController.Create(viewModel) as RedirectToRouteResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var actionName = result.RouteValues["action"];
        //    Assert.AreEqual("Index", actionName);
        //}

        //[Test]
        //public void Create_POST_GivenModelStateIsInValid_ShouldReturnViewModel()
        //{
        //    //---------------Set up test pack-------------------
        //    var viewModel = Substitute.For<CarViewModel>();
        //    var carController = CreateBuilder().Build();
        //    carController.ModelState.AddModelError("IdNotValid", "Car Id is not valid");
        //    //---------------Assert Precondition----------------
        //    Assert.IsFalse(carController.ModelState.IsValid);
        //    //---------------Execute Test ----------------------
        //    var result = carController.Create(viewModel) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as CarViewModel;
        //    Assert.IsInstanceOf<CarViewModel>(model);
        //    Assert.AreEqual(viewModel,model);
        //}

        //[Test]
        //public void Edit_Get_GivenValidCarId_ShouldCallGetCarByIdFromRepo()
        //{
        //    //---------------Set up test pack-------------------
        //    var id = RandomValueGen.GetRandomGuid();
        //    var carRepository = Substitute.For<ICarRepository>();
        //    var carController = CreateBuilder()
        //        .WithCarRepository(carRepository)
        //        .Build();
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = carController.Edit(id);
        //    //---------------Test Result -----------------------
        //    carRepository.Received().GetCarBy(id);
        //}

        //[Test]
        //public void Edit_Get_GivenValidCarId_ShouldCallMappingEngine()
        //{
        //    //---------------Set up test pack-------------------
        //    var car = new CarBuilder().Build();
        //    var id = car.CarId;
        //    var mappingEngine = Substitute.For<IMappingEngine>();
        //    var carRepository = Substitute.For<ICarRepository>();
        //    carRepository.GetCarBy(id).Returns(car);
        //    var carController = CreateBuilder()
        //        .WithMappingEngine(mappingEngine)
        //        .WithCarRepository(carRepository)
        //        .Build();
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = carController.Edit(id);
        //    //---------------Test Result -----------------------
        //    mappingEngine.Received().Map<CarViewModel>(car);
        //}

        //[Test]
        //public void Edit_Get_ShouldReturnViewWithCarViewModel()
        //{
        //    //---------------Set up test pack-------------------
        //    var car = new CarBuilder().Build();
        //    var id = car.CarId;
        //    var mappingEngine = ResolveMapper();

        //    var carRepository = Substitute.For<ICarRepository>();
        //    carRepository.GetCarBy(id).Returns(car);
        //    var carController = CreateBuilder()
        //        .WithMappingEngine(mappingEngine)
        //        .WithCarRepository(carRepository)
        //        .Build();
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = carController.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as CarViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.IsInstanceOf<CarViewModel>(model);

        //}

        //[Test]
        //public void Edit_POST_GivenModelStateIsValid_ShouldCallGetCarBy()
        //{
        //    //---------------Set up test pack-------------------
        //    var carViewModel =Substitute.For<CarViewModel>();
        //    var carRepository = Substitute.For<ICarRepository>();

        //    var carController = CreateBuilder()
        //        .WithCarRepository(carRepository)
        //        .Build();
        //    //---------------Assert Precondition----------------
        //    Assert.IsTrue(carController.ModelState.IsValid);
        //    //---------------Execute Test ----------------------
        //    var result = carController.Edit(carViewModel);
        //    //---------------Test Result -----------------------
        //    carRepository.Received().GetCarBy(carViewModel.CarId);
        //}

        //[Test]
        //public void Edit_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
        //{
        //    //---------------Set up test pack-------------------
        //    var carViewModel = Substitute.For<CarViewModel>();
        //    var car = new CarBuilder().Build();
        //    var carRepository = Substitute.For<ICarRepository>();
        //    var mappingEngine = Substitute.For<IMappingEngine>();
        //    carRepository.GetCarBy(carViewModel.CarId).Returns(car);
        //    var carController = CreateBuilder()
        //        .WithCarRepository(carRepository)
        //        .WithMappingEngine(mappingEngine)
        //        .Build();
        //    //---------------Assert Precondition----------------
        //    Assert.IsTrue(carController.ModelState.IsValid);
        //    //---------------Execute Test ----------------------
        //    var result = carController.Edit(carViewModel);
        //    //---------------Test Result -----------------------
        //    mappingEngine.Received().Map<Car>(carViewModel);
        //}

        //[Test]
        //public void Edit_POST_GivenModelStateIsValid_ShouldCallUpdate()
        //{
        //    //---------------Set up test pack-------------------
        //    var carViewModel = Substitute.For<CarViewModel>();
        //    var car = new CarBuilder().Build();
        //    var newCar = new CarBuilder().Build();
        //    var carRepository = Substitute.For<ICarRepository>();
        //    var mappingEngine = Substitute.For<IMappingEngine>();
        //    carRepository.GetCarBy(carViewModel.CarId).Returns(car);
        //    mappingEngine.Map<Car>(carViewModel).Returns(newCar);

        //    var carController = CreateBuilder()
        //        .WithCarRepository(carRepository)
        //        .WithMappingEngine(mappingEngine)
        //        .Build();
        //    //---------------Assert Precondition----------------
        //    Assert.IsTrue(carController.ModelState.IsValid);
        //    //---------------Execute Test ----------------------
        //    var result = carController.Edit(carViewModel);
        //    //---------------Test Result -----------------------
        //    carRepository.Received().Update(car, newCar);
        //}


        //[Test]
        //public void Edit_POST_GivenModelStateIsValid_ShouldRedirectToRouteResult()
        //{
        //    //---------------Set up test pack-------------------
        //    var carViewModel =Substitute.For<CarViewModel>();
        //    var car = new CarBuilder().Build();
        //    var newCar = new CarBuilder().Build();
        //    var carRepository = Substitute.For<ICarRepository>();
        //    var mappingEngine = Substitute.For<IMappingEngine>();
        //    carRepository.GetCarBy(carViewModel.CarId).Returns(car);
        //    mappingEngine.Map<Car>(carViewModel).Returns(newCar);

        //    var carController = CreateBuilder()
        //        .WithCarRepository(carRepository)
        //        .WithMappingEngine(mappingEngine)
        //        .Build();
        //    //---------------Assert Precondition----------------
        //    Assert.IsTrue(carController.ModelState.IsValid);
        //    //---------------Execute Test ----------------------
        //    var result = carController.Edit(carViewModel) as RedirectToRouteResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var routeValue = result.RouteValues["action"];
        //    Assert.AreEqual("Index", routeValue);
        //}

        //[Test]
        //public void Edit_POST_GivenModelStateIsInValid_ShouldReturnView()
        //{
        //    //---------------Set up test pack-------------------
        //    var carViewModel = Substitute.For<CarViewModel>();
        //    var carController = CreateBuilder().Build();
        //    carController.ModelState.AddModelError("AnyKey", "Error");
        //    //---------------Assert Precondition----------------
        //    Assert.IsFalse(carController.ModelState.IsValid);
        //    //---------------Execute Test ----------------------
        //    var result = carController.Edit(carViewModel) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    Assert.IsInstanceOf<CarViewModel>(result.Model);
        //}

        [Test]
        public void Delete_GivenValidCarId_ShouldCallGetCarById()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomGuid();
            var carRepository = Substitute.For<ICarRepository>();
            var carController = CreateBuilder()
                .WithCarRepository(carRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = carController.Delete(id);
            //---------------Test Result -----------------------
            carRepository.Received().GetCarBy(id);
        }

        [Test]
        public void Delete_GivenValidCarId_ShouldCallDelete()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().Build();
            var id = car.CarId;
            var carRepository = Substitute.For<ICarRepository>();
            carRepository.GetCarBy(id).Returns(car);
            var carController = CreateBuilder()
                .WithCarRepository(carRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = carController.Delete(id);
            //---------------Test Result -----------------------
            carRepository.Received().Delete(car);
        }
        [Test]
        public void Delete_GivenValidCarId_ShouldRedirectToIndex()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().Build();
            var id = car.CarId;
            var carRepository = Substitute.For<ICarRepository>();
            carRepository.GetCarBy(id).Returns(car);
            var carController = CreateBuilder()
                .WithCarRepository(carRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = carController.Delete(id) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var actionName = result.RouteValues["action"];
            Assert.AreEqual("Index",actionName);
        }

        private static IMappingEngine ResolveMapper()
        {
            return ResolveMappingWith(
                new CarMappings()
                );
        }

        public static IMappingEngine ResolveMappingWith(params Profile[] profiles)
        {
            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            return Mapper.Engine;
        }

    }

   

}