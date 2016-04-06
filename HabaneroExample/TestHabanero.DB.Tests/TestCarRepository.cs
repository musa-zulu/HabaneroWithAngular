using System.Collections.Generic;
using System.Linq;
using Habanero.BO;
using NSubstitute;
using NUnit.Framework;
using TestHabanero.BO;
using TestHabanero.BO.Tests.Util;
using TestHabanero.Tests.Commons;
using TestHabenaro.Db.Interfaces;
using TestHabenaro.DB;

namespace TestHabanero.DB.Tests
{
    [TestFixture]
    public class TestCarRepository
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
        public void GetCars_GivenOneCar_ShouldReturnCar()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().BuildSaved();
            var userRepository = new CarRepository();
            var cars = new List<Car> { car };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetCars();
            //---------------Test Result -----------------------
            Assert.AreEqual(cars.Count, result.Count);
            var actual = cars.First();
            Assert.AreSame(car, actual);
        }

        [Test]
        public void GetCars_GivenTwoCars_ShouldReturnCar()
        {
            //---------------Set up test pack-------------------
            var car1 = new CarBuilder().WithNewId().BuildSaved();
            var car2 = new CarBuilder().WithNewId().BuildSaved();
            
            var userRepository = new CarRepository();
            var cars = new List<Car> {car1, car2};
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetCars();
            //---------------Test Result -----------------------
            Assert.AreEqual(cars.Count, result.Count);

        }

        [Test]
        public void GetCars_GivenThreeCars_ShouldReturnCar()
        {
            //---------------Set up test pack-------------------
            var car1 = new CarBuilder().WithNewId().BuildSaved();
            var car2 = new CarBuilder().WithNewId().BuildSaved();
            var car3 = new CarBuilder().WithNewId().BuildSaved();
            var userRepository = new CarRepository();
            var cars = new List<Car> { car1,car2,car3 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetCars();
            //---------------Test Result -----------------------
            Assert.AreEqual(cars.Count, result.Count);
           
        }

        [Test]
        public void GetCarBy_GivenCarId_ShouldReturnCar()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().BuildSaved();
            var userRepository = new CarRepository();
            var cars = new List<Car> { car };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetCarBy(car.CarId);
            //---------------Test Result -----------------------
            Assert.AreEqual(cars.FirstOrDefault().CarId,result.CarId);
           var actual = cars.First();
            Assert.AreSame(car, actual);
        }

        [Test]
        public void Save_GivenNewCar_ShouldSave()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().BuildSaved();
            var userRepository = new CarRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Save(car);
            //---------------Test Result -----------------------
           
        }

        [Test]
        public void Update_GivenExistingCar_ShouldUpdateAndSave()
        {
            //---------------Set up test pack-------------------
            var car1 = new CarBuilder().WithNewId().BuildSaved();
            var car2 =new CarBuilder().Build();
            var userRepository = new CarRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            userRepository.Update(car1, car2);
            var cars = Broker.GetBusinessObjectCollection<Car>("");
            //---------------Test Result -----------------------
            Assert.AreEqual(1,cars.Count);
            Assert.AreEqual(car2.Make,cars.FirstOrDefault().Make);
            Assert.AreEqual(car2.Model,cars.FirstOrDefault().Model);
        }

        [Test]
        public void Delete_GivenExistingCar_ShouldDeleteAndSave()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().BuildSaved();
            var cars = Broker.GetBusinessObjectCollection<Car>("");
            var userRepository = new CarRepository();
            Assert.AreEqual(1, cars.Count);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Delete(car);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cars.Count);
        }

        
    }
}