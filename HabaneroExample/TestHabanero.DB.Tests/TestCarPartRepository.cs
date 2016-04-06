using System.Collections.Generic;
using System.Linq;
using Habanero.BO;
using NUnit.Framework;
using TestHabanero.BO;
using TestHabanero.BO.Tests.Util;
using TestHabanero.Tests.Commons;
using TestHabenaro.DB;

namespace TestHabanero.DB.Tests
{
    public class TestCarPartRepository
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            TestUtils.SetupFixture();
        }

        [SetUp]
        public void SetUp()
        {
            Habanero.BO.BORegistry.DataAccessor = new DataAccessorInMemory();

        }

        [Test]
        public void GetCarParts_GivenOneCarPart_ShouldReturnCarPart()
        {
            //---------------Set up test pack-------------------
            var car = new CarPartBuilder().WithNewId().BuildSaved();
            var userRepository = new CarPartRepository();
            var cars = new List<CarPart> { car };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetCarPart();
            //---------------Test Result -----------------------
            Assert.AreEqual(result.Count, cars.Count);
            var actual = cars.First();
            Assert.AreSame(car, actual);
        }

        [Test]
        public void GetCarParts_GivenTwoCarParts_ShouldReturnCarParts()
        {
            //---------------Set up test pack-------------------
            var car1 = new CarPartBuilder().WithNewId().BuildSaved();
            var car2 = new CarPartBuilder().WithNewId().BuildSaved();

            var userRepository = new CarPartRepository();
            var cars = new List<CarPart> { car1, car2 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetCarPart();
            //---------------Test Result -----------------------
            Assert.AreEqual(result.Count, cars.Count);

        }

        [Test]
        public void GetParts_GivenThreeCarParts_ShouldReturnCarParts()
        {
            //---------------Set up test pack-------------------
            var part1 = new CarPartBuilder().WithNewId().BuildSaved();
            var part2 = new CarPartBuilder().WithNewId().BuildSaved();
            var part3 = new CarPartBuilder().WithNewId().BuildSaved();
            var userRepository = new CarPartRepository();
            var cars = new List<CarPart> { part1, part2, part3 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetCarPart();
            //---------------Test Result -----------------------
            Assert.AreEqual(result.Count, cars.Count);

        }
        
        [Test]
        public void GetCarPartBy_GivenCarPartId_ShouldReturnCarPart()
        {
            //---------------Set up test pack-------------------
            var part = new CarPartBuilder().WithNewId().BuildSaved();
            var userRepository = new CarPartRepository();
            var parts = new List<CarPart> { part };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetCarPartBy(part.CarPartId);
            //---------------Test Result -----------------------
            Assert.AreEqual(result.CarPartId, parts.FirstOrDefault().CarPartId);
            var actual = parts.First();
            Assert.AreSame(part, actual);
        }

        [Test]
        public void Save_GivenNewCarPart_ShouldSave()
        {
            //---------------Set up test pack-------------------
            var car = new CarPartBuilder().WithNewId().BuildSaved();
            var userRepository = new CarPartRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Save(car);
            //---------------Test Result -----------------------
           
        }

        [Test]
        public void Update_GivenExistingCarPart_ShouldUpdateAndSave()
        {
            //---------------Set up test pack-------------------
            var existingPart = new CarPartBuilder().WithNewId().BuildSaved();
            var userRepository =new CarPartRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Update(existingPart, existingPart);
            var parts = Broker.GetBusinessObjectCollection<CarPart>("");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, parts.Count);
        }

        [Test]
        public void Delete_GivenExistingCarPart_ShouldDeleteAndSave()
        {
            //---------------Set up test pack-------------------
            var carPart = new CarPartBuilder().WithNewId().BuildSaved();
            var carParts = Broker.GetBusinessObjectCollection<CarPart>("");
            var userRepository = new CarPartRepository();
            Assert.AreEqual(1, carParts.Count);
            //---------------Execute Test ----------------------
            userRepository.Delete(carPart);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, carParts.Count);
        }
    }
}