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
    public class TestPartController
    {
        private PartControllerBuilder CreateBuilder()
        {
            return new PartControllerBuilder();
        }

        [Test]
        public void Construct ()
        {
            Assert.DoesNotThrow(() => new PartController(Substitute.For<IPartRepository>(), Substitute.For<IMappingEngine>()));
        }

        [Test]
        public void Construct_GivenIPartRepositoryIsNull_ShouldThrow()
        {
            var ex =
                Assert.Throws<ArgumentNullException>(() => new PartController(null, Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("partRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMappingEngineIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new PartController(Substitute.For<IPartRepository>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("mappingEngine", ex.ParamName);
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<PartViewModel>();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var partController = CreateBuilder()
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(partController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = partController.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<Part>(viewModel);
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallSave()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<PartViewModel>();

            var repository = Substitute.For<IPartRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            var car = new PartBuilder().WithNewId().Build();
            mapperEngine.Map<Part>(viewModel).Returns(car);
            var carController = CreateBuilder()
                .WithCarRepository(repository)
                .WithMappingEngine(mapperEngine)
                .Build();
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
            var partController = CreateBuilder()
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
            var part = new PartBuilder().WithNewId().Build();
            var parts = new List<Part> { part };
            var repository = Substitute.For<IPartRepository>();
            var mappingEngine = ResolveMapper();
            repository.GetParts().Returns(parts);
            var partController = CreateBuilder()
                .WithCarRepository(repository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = partController.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model as List<PartViewModel>;
            Assert.IsInstanceOf<List<PartViewModel>>(model);
            Assert.AreEqual(part.PartId, model.FirstOrDefault().PartId);
            Assert.AreEqual(part.Name, model.FirstOrDefault().Name);
            Assert.AreEqual(part.Description, model.FirstOrDefault().Description);
            Assert.AreEqual(part.Price, model.FirstOrDefault().Price);
        }

        [Test]
        public void Index_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var part = new PartBuilder().WithNewId().Build();
            var parts = new List<Part> { part };
            var partRepository = Substitute.For<IPartRepository>();
            partRepository.GetParts().Returns(parts);
            var mappingEngine = Substitute.For<IMappingEngine>();

            var partController = CreateBuilder()
                .WithCarRepository(partRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = partController.Index() as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<List<Part>, List<PartViewModel>>(parts);
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
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<PartViewModel>();
            var partController = CreateBuilder().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = partController.Create(viewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var actionName = result.RouteValues["action"];
            Assert.AreEqual("Index", actionName);
        }

        [Test]
        public void Create_POST_GivenModelStateIsInValid_ShouldReturnViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<PartViewModel>();
            var partController = CreateBuilder().Build();
            partController.ModelState.AddModelError("IdNotValid", "Part Id is not valid");
            //---------------Assert Precondition----------------
            Assert.IsFalse(partController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = partController.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model as PartViewModel;
            Assert.IsInstanceOf<PartViewModel>(model);
            Assert.AreEqual(viewModel, model);
        }

        [Test]
        public void Edit_Get_GivenValidCarId_ShouldCallGetCarByIdFromRepo()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomGuid();
            var partRepository = Substitute.For<IPartRepository>();
            var partController = CreateBuilder()
                .WithCarRepository(partRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = partController.Edit(id);
            //---------------Test Result -----------------------
            partRepository.Received().GetPartBy(id);
        }

        [Test]
        public void Edit_Get_GivenValidCarId_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var part = new PartBuilder().Build();
            var id = part.PartId;
            var mappingEngine = Substitute.For<IMappingEngine>();
            var repository = Substitute.For<IPartRepository>();
            repository.GetPartBy(id).Returns(part);
            var controller = CreateBuilder()
                .WithMappingEngine(mappingEngine)
                .WithCarRepository(repository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Edit(id);
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<PartViewModel>(part);
        }

        [Test]
        public void Edit_Get_ShouldReturnViewWithPartViewModel()
        {
            //---------------Set up test pack-------------------
            var part = new PartBuilder().Build();
            var id = part.PartId;
            var mappingEngine = ResolveMapper();
           var repository = Substitute.For<IPartRepository>();
            repository.GetPartBy(id).Returns(part);
            var controller = CreateBuilder()
                .WithMappingEngine(mappingEngine)
                .WithCarRepository(repository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model as PartViewModel;
            Assert.IsNotNull(model);
            Assert.IsInstanceOf<PartViewModel>(model);

        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallGetCarBy()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<PartViewModel>();
            var repository = Substitute.For<IPartRepository>();

            var partController = CreateBuilder()
                .WithCarRepository(repository)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(partController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = partController.Edit(viewModel);
            //---------------Test Result -----------------------
            repository.Received().GetPartBy(viewModel.PartId);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<PartViewModel>();
            var part = new PartBuilder().Build();
            var repository = Substitute.For<IPartRepository>();
            var mappingEngine = Substitute.For<IMappingEngine>();
            repository.GetPartBy(viewModel.PartId).Returns(part);
            var controller = CreateBuilder()
                .WithCarRepository(repository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<Part>(viewModel);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallUpdate()
        {
            //---------------Set up test pack-------------------
            var model = Substitute.For<PartViewModel>();
            var existingPart = new PartBuilder().Build();
            var newPart = new PartBuilder().Build();
            var partRepository = Substitute.For<IPartRepository>();
            var mappingEngine = Substitute.For<IMappingEngine>();
            partRepository.GetPartBy(model.PartId).Returns(existingPart);
            mappingEngine.Map<Part>(model).Returns(newPart);

            var controller = CreateBuilder()
                .WithCarRepository(partRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = controller.Edit(model);
            //---------------Test Result -----------------------
            partRepository.Received().Update(existingPart, newPart);
        }


        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldRedirectToRouteResult()
        {
            //---------------Set up test pack-------------------
            var partViewModel = Substitute.For<PartViewModel>();
            var part = new PartBuilder().Build();
            var newPart = new PartBuilder().Build();
            var repository = Substitute.For<IPartRepository>();
            var mappingEngine = Substitute.For<IMappingEngine>();
            repository.GetPartBy(partViewModel.PartId).Returns(part);
            mappingEngine.Map<Part>(partViewModel).Returns(newPart);

            var controller = CreateBuilder()
                .WithCarRepository(repository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = controller.Edit(partViewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var routeValue = result.RouteValues["action"];
            Assert.AreEqual("Index", routeValue);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsInValid_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var partViewModel = Substitute.For<PartViewModel>();
            var partController = CreateBuilder().Build();
            partController.ModelState.AddModelError("AnyKey", "Error");
            //---------------Assert Precondition----------------
            Assert.IsFalse(partController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = partController.Edit(partViewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PartViewModel>(result.Model);
        }

        [Test]
        public void Delete_GivenValidCarId_ShouldCallGetCarById()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomGuid();
            var repository = Substitute.For<IPartRepository>();
            var controller = CreateBuilder()
                .WithCarRepository(repository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Delete(id);
            //---------------Test Result -----------------------
            repository.Received().GetPartBy(id);
        }

        [Test]
        public void Delete_GivenValidCarId_ShouldCallDelete()
        {
            //---------------Set up test pack-------------------
            var part = new PartBuilder().Build();
            var id = part.PartId;
            var repository = Substitute.For<IPartRepository>();
            repository.GetPartBy(id).Returns(part);
            var controller = CreateBuilder()
                .WithCarRepository(repository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Delete(id);
            //---------------Test Result -----------------------
            repository.Received().Delete(part);
        }
        [Test]
        public void Delete_GivenValidCarId_ShouldRedirectToIndex()
        {
            //---------------Set up test pack-------------------
            var part = new PartBuilder().Build();
            var id = part.PartId;
            var repository = Substitute.For<IPartRepository>();
            repository.GetPartBy(id).Returns(part);
            var partController = CreateBuilder()
                .WithCarRepository(repository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = partController.Delete(id) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var actionName = result.RouteValues["action"];
            Assert.AreEqual("Index", actionName);
        }

        private static IMappingEngine ResolveMapper()
        {
            return ResolveMappingWith(
                new PartMapping()
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