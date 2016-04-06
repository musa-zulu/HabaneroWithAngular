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
    public class TestCarPartController
    {
        private CarPartControllerBuilder CreateBuilder()
        {
            return new CarPartControllerBuilder();
        }

        [Test]
        public void Construct()
        {
            Assert.DoesNotThrow(() => new CarPartsController(Substitute.For<ICarPartRepository>(), Substitute.For<ICarRepository>(), Substitute.For<IPartRepository>(), Substitute.For<IMappingEngine>()));
        }

        [Test]
        public void Construct_GivenICarPartRepositoryIsNull_ShouldThrow()
        {
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarPartsController(null, Substitute.For<ICarRepository>(), Substitute.For<IPartRepository>(), Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("carPartRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMappingEngineIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarPartsController(Substitute.For<ICarPartRepository>(), Substitute.For<ICarRepository>(), Substitute.For<IPartRepository>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("mappingEngine", ex.ParamName);
        }

        [Test]
        public void Construct_GivenICarRepositoryIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarPartsController(Substitute.For<ICarPartRepository>(), null, Substitute.For<IPartRepository>(), Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("carRepository", ex.ParamName);
        }


        [Test]
        public void Construct_GivenIPartRepositoryIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarPartsController(Substitute.For<ICarPartRepository>(), Substitute.For<ICarRepository>(), null, Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("partRepository", ex.ParamName);
        }

     

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallSave()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<CarPartsViewModel>();
            var repository = Substitute.For<ICarPartRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            var car = new CarPartBuilder().WithNewId().Build();
            mapperEngine.Map<CarPart>(viewModel).Returns(car);
            var carController = CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mapperEngine)
                .Build();
            repository.Save(car);
            //---------------Assert Precondition----------------
            Assert.IsTrue(carController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = carController.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            repository.Received().Save(car);
        }

        [Test]
        public void Create_ShouldReturnViewModel()
        {
            //---------------Set up test pack-------------------
            var partRepository = Substitute.For<IPartRepository>();
            var carRepository = Substitute.For<ICarRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            
            var part = new PartBuilder().Build();
            var parts = new List<TestHabanero.BO.Part> { part };
            var car = new CarBuilder().Build();
            var cars = new List<Car> { car };
            partRepository.GetParts().Returns(parts);
            carRepository.GetCars().Returns(cars);
            var partController = CreateBuilder()
                .WithMappingEngine(mapperEngine)
                .WithPartRepository(partRepository)
                .WithCarRepository(carRepository)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = partController.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Index_GivenAllUsersReturnedFromRepository_ShouldReturnViewModel()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().Build();
            var part=new PartBuilder().WithNewId().Build();
            var carPart = new CarPartBuilder().WithNewId().WithCar(car).WithPart(part).Build();
            var carParts = new List<CarPart> { carPart };
            var repository = Substitute.For<ICarPartRepository>();
            var mappingEngine = ResolveMapper();

            repository.GetCarPart().Returns(carParts);

            var partController = CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = partController.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model as List<CarPartsIndexViewModel>;
            Assert.IsInstanceOf<List<CarPartsIndexViewModel>>(model);
            Assert.AreEqual(carPart.CarPartId, model.FirstOrDefault().CarPartId);
            Assert.AreEqual(carPart.CarId, model.FirstOrDefault().CarId);
            Assert.AreEqual(carPart.PartId, model.FirstOrDefault().PartId);
            Assert.AreEqual(carPart.Quantity, model.FirstOrDefault().Quantity);
        }

        [Test]
        public void Index_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var part = new CarPartBuilder().WithNewId().Build();
            var parts = new List<CarPart> { part };
            var partRepository = Substitute.For<ICarPartRepository>();
            partRepository.GetCarPart().Returns(parts);
            var mappingEngine = Substitute.For<IMappingEngine>();

            var partController = CreateBuilder()
                .WithCarPartRepository(partRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = partController.Index() as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<List<CarPart>, List<CarPartsIndexViewModel>>(parts);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var partController = CreateBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = partController.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_POST_ShouldReturnToAction()
        {//---------------Set up test pack-------------------
            var viewModel = Substitute.For<CarPartsViewModel>();
            var repository = Substitute.For<ICarPartRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            var car = new CarPartBuilder().WithNewId().Build();
            mapperEngine.Map<CarPart>(viewModel).Returns(car);
            var carPartsController = CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mapperEngine)
                .Build();
            repository.Save(car);
            //---------------Assert Precondition----------------
            Assert.IsTrue(carPartsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = carPartsController.Create(viewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var actionName = result.RouteValues["action"];
            Assert.AreEqual("Index", actionName);
        }

        [Test]
        public void Create_POST_GivenModelStateIsInValid_ShouldReturnViewModel()
        {
            //---------------Set up test pack-------------------
            var partRepository = Substitute.For<IPartRepository>();
            var carRepository = Substitute.For<ICarRepository>();
            var viewModel = Substitute.For<CarPartsViewModel>();

            var part = new PartBuilder().Build();
            var car = new CarBuilder().Build();
            var parts = new List<TestHabanero.BO.Part> { part };
            var cars = new List<Car> { car }; 
            partRepository.GetParts().Returns(parts);
            carRepository.GetCars().Returns(cars);
            var carController = CreateBuilder()
                .WithCarRepository(carRepository)
                .WithPartRepository(partRepository)
                .Build();
            carController.ModelState.AddModelError("IdNotValid", "Car Id is not valid");
            //---------------Assert Precondition----------------
            Assert.IsFalse(carController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = carController.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model as CarPartsViewModel;
            Assert.IsInstanceOf<CarPartsViewModel>(model);
            Assert.AreEqual(viewModel, model);
        }

        [Test]
        public void Edit_Get_GivenValidCarId_ShouldCallGetCarByIdFromRepo()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().Build();
            var part = new PartBuilder().WithNewId().Build();
            var carPart = new CarPartBuilder().WithNewId().WithCar(car).WithPart(part).Build();
            var id = carPart.CarPartId;
       

            var carPartRepository = Substitute.For<ICarPartRepository>();
            var partRepository = Substitute.For<IPartRepository>();
            var carRepository = Substitute.For<ICarRepository>();
            var mapper = ResolveMapper();
            
            carPartRepository.GetCarPartBy(id).Returns(carPart);

            var parts = new List<TestHabanero.BO.Part> { part };
            var cars = new List<Car> { car };
           
           
            partRepository.GetParts().Returns(parts);
            carRepository.GetCars().Returns(cars);
          

            var carController = CreateBuilder()
                .WithCarPartRepository(carPartRepository)
                .WithCarRepository(carRepository)
                .WithPartRepository(partRepository)
                .WithMappingEngine(mapper)
                .Build();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = carController.Edit(id);
            //---------------Test Result -----------------------
            carPartRepository.Received().GetCarPartBy(id);
        }

        [Test]
        public void Edit_Get_GivenValidCarId_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().Build();
            var part = new PartBuilder().WithNewId().Build();
            var carPart = new CarPartBuilder().WithNewId().WithCar(car).WithPart(part).Build();
            var id = carPart.CarPartId;
            var carPartRepository = Substitute.For<ICarPartRepository>();
            var partRepository = Substitute.For<IPartRepository>();
            var carRepository = Substitute.For<ICarRepository>();
            var mapper = Substitute.For<IMappingEngine>();
            var viewModel = Substitute.For<CarPartsViewModel>();
            carPartRepository.GetCarPartBy(id).Returns(carPart);
            var parts = new List<TestHabanero.BO.Part> { part };
            var cars = new List<Car> { car };
            partRepository.GetParts().Returns(parts);
            carRepository.GetCars().Returns(cars);
            mapper.Map<CarPartsViewModel>(carPart).Returns(viewModel);

            var carController = CreateBuilder()
                .WithCarPartRepository(carPartRepository)
                .WithCarRepository(carRepository)
                .WithPartRepository(partRepository)
                .WithMappingEngine(mapper)
                .Build();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = carController.Edit(id);
            //---------------Test Result -----------------------
            mapper.Received().Map<CarPartsViewModel>(carPart);
        }

        [Test]
        public void Edit_Get_ShouldReturnViewWithCarViewModel()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().Build();
            var part = new PartBuilder().WithNewId().Build();
            var carPart = new CarPartBuilder().WithNewId().WithCar(car).WithPart(part).Build();
            var id = carPart.CarPartId;
            var carPartRepository = Substitute.For<ICarPartRepository>();
            var partRepository = Substitute.For<IPartRepository>();
            var carRepository = Substitute.For<ICarRepository>();
            var mapper = Substitute.For<IMappingEngine>();
            var viewModel = Substitute.For<CarPartsViewModel>();
            carPartRepository.GetCarPartBy(id).Returns(carPart);
            var parts = new List<TestHabanero.BO.Part> { part };
            var cars = new List<Car> { car };
            partRepository.GetParts().Returns(parts);
            carRepository.GetCars().Returns(cars);
            mapper.Map<CarPartsViewModel>(carPart).Returns(viewModel);

            var carController = CreateBuilder()
                .WithCarPartRepository(carPartRepository)
                .WithCarRepository(carRepository)
                .WithPartRepository(partRepository)
                .WithMappingEngine(mapper)
                .Build();
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = carController.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model as CarPartsViewModel;
            Assert.IsNotNull(model);
            Assert.IsInstanceOf<CarPartsViewModel>(model);
        }


        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallGetCarBy()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<CarPartsViewModel>();
            var repository = Substitute.For<ICarPartRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            var carPart = new CarPartBuilder().WithNewId().Build();
            mapperEngine.Map<CarPart>(viewModel).Returns(carPart);
            repository.GetCarPartBy(viewModel.CarPartId).Returns(carPart);

            var carPartsController= CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mapperEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(carPartsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = carPartsController.Edit(viewModel);
            //---------------Test Result -----------------------
            repository.Received().GetCarPartBy(viewModel.CarId);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallUpdate()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<CarPartsViewModel>();
            var repository = Substitute.For<ICarPartRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            var carPart1 = new CarPartBuilder().WithNewId().Build();
            var carPart = new CarPartBuilder().WithNewId().Build();
            mapperEngine.Map<CarPart>(viewModel).Returns(carPart);
            repository.GetCarPartBy(viewModel.CarPartId).Returns(carPart);
            repository.Update(carPart,carPart1);
            var carPartsController= CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mapperEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(carPartsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = carPartsController.Edit(viewModel);
            //---------------Test Result -----------------------
            repository.Received().Update(carPart, carPart1);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldRedirectToRouteResult()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<CarPartsViewModel>();
            var repository = Substitute.For<ICarPartRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            var carPart1 = new CarPartBuilder().WithNewId().Build();
            var carPart = new CarPartBuilder().WithNewId().Build();
            mapperEngine.Map<CarPart>(viewModel).Returns(carPart);
            repository.GetCarPartBy(viewModel.CarPartId).Returns(carPart);
            repository.Update(carPart,carPart1);
            var carPartsController= CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mapperEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(carPartsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = carPartsController.Edit(viewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var routeValue = result.RouteValues["action"];
            Assert.AreEqual("Index", routeValue);
        }

      [Test]
        public void Edit_POST_GivenModelStateIsInValid_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<CarPartsViewModel>();
            var repository = Substitute.For<ICarPartRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            var partRepository = Substitute.For<IPartRepository>();
            var carRepository = Substitute.For<ICarRepository>();
            var part = new PartBuilder().Build();
            var car = new CarBuilder().Build();
            var parts = new List<TestHabanero.BO.Part> { part };
            var cars = new List<Car> { car };
            partRepository.GetParts().Returns(parts);
            carRepository.GetCars().Returns(cars);
            var carPart1 = new CarPartBuilder().WithNewId().Build();
            var carPart = new CarPartBuilder().WithNewId().Build();
            mapperEngine.Map<CarPart>(viewModel).Returns(carPart);
            repository.GetCarPartBy(viewModel.CarPartId).Returns(carPart);
            repository.Update(carPart,carPart1);
            var carPartsController= CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mapperEngine)
                .WithCarRepository(carRepository)
                .WithPartRepository(partRepository)
                .Build();
            carPartsController.ModelState.AddModelError("IdNotValid", "Car Id is not valid");
            //---------------Assert Precondition----------------
            Assert.IsFalse(carPartsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = carPartsController.Edit(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model as CarPartsViewModel;
            Assert.IsInstanceOf<CarPartsViewModel>(model);
            Assert.AreEqual(viewModel, model);
        }

        [Test]
        public void Delete_GivenValidCarId_ShouldCallGetCarPartById()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomGuid();
            var carRepository = Substitute.For<ICarPartRepository>();
            var carController = CreateBuilder()
                .WithCarPartRepository(carRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = carController.Delete(id);
            //---------------Test Result -----------------------
            carRepository.Received().GetCarPartBy(id);
        }


        [Test]
        public void Delete_GivenValidCarId_ShouldCallDelete()
        {
            //---------------Set up test pack-------------------
            var car = new CarPartBuilder().Build();
            var id = car.CarPartId;
            var carRepository = Substitute.For<ICarPartRepository>();
            carRepository.GetCarPartBy(id).Returns(car);
            var carController = CreateBuilder()
                .WithCarPartRepository(carRepository)
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
            var car = new CarPartBuilder().Build();
            var id = car.CarPartId;
            var carRepository = Substitute.For<ICarPartRepository>();
            carRepository.GetCarPartBy(id).Returns(car);
            var carController = CreateBuilder()
                .WithCarPartRepository(carRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = carController.Delete(id) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var actionName = result.RouteValues["action"];
            Assert.AreEqual("Index", actionName);
        }
        private static IMappingEngine ResolveMapper()
        {
            return ResolveMappingWith(
                new CarPartMapping()
                );
        }

        private static IMappingEngine ResolveMappingWith(params Profile[] profiles)
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