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
    [TestFixture]
    public class TestPartRepository
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
        public void GetParts_GivenOnePart_ShouldReturnPart()
        {
            //---------------Set up test pack-------------------
            var car = new PartBuilder().WithNewId().BuildSaved();
            var userRepository = new PartRepository();
            var cars = new List<Part> { car };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetParts();
           //---------------Test Result -----------------------
            Assert.AreEqual(result.Count, cars.Count);
            var actual = cars.First();
            Assert.AreSame(car, actual);
        }

        [Test]
        public void GetParts_GivenTwoPart_ShouldReturnPart()
        {
            //---------------Set up test pack-------------------
            var car1 = new PartBuilder().WithNewId().BuildSaved();
            var car2 = new PartBuilder().WithNewId().BuildSaved();

            var userRepository = new PartRepository();
            var cars = new List<Part> { car1, car2 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetParts();
            //---------------Test Result -----------------------
            Assert.AreEqual(result.Count, cars.Count);

        }

        [Test]
        public void GetParts_GivenThreePart_ShouldReturnPart()
        {
            //---------------Set up test pack-------------------
            var part1 = new PartBuilder().WithNewId().BuildSaved();
            var part2 = new PartBuilder().WithNewId().BuildSaved();
            var part3 = new PartBuilder().WithNewId().BuildSaved();
            var userRepository = new PartRepository();
            var cars = new List<Part> { part1, part2, part3 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetParts();
            //---------------Test Result -----------------------
            Assert.AreEqual(result.Count, cars.Count);

        }


        [Test]
        public void GetPartBy_GivenPartId_ShouldReturnPart()
        {
            //---------------Set up test pack-------------------
            var part = new PartBuilder().WithNewId().BuildSaved();
            var userRepository = new PartRepository();
            var parts = new List<Part> { part };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = userRepository.GetPartBy(part.PartId);
            //---------------Test Result -----------------------
            Assert.AreEqual(result.PartId, parts.FirstOrDefault().PartId);
            var actual = parts.First();
            Assert.AreSame(part, actual);
        }

        [Test]
        public void Save_GivenNewPart_ShouldSave()
        {
            //---------------Set up test pack-------------------
            var car = new PartBuilder().WithNewId().BuildSaved();
            var userRepository = new PartRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Save(car);
            //---------------Test Result -----------------------
          
        }

        [Test]
        public void Update_GivenExistingPart_ShouldUpdateAndSave()
        {
            //---------------Set up test pack-------------------
            var existingPart = new PartBuilder().WithNewId().BuildSaved();
            var userRepository = new PartRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Update(existingPart, existingPart);
            var parts = Broker.GetBusinessObjectCollection<Part>("");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, parts.Count);
           
        }

        [Test]
        public void Delete_GivenExistingPart_ShouldDeleteAndSave()
        {
            //---------------Set up test pack-------------------
            var part = new PartBuilder().WithNewId().BuildSaved();
            var parts = Broker.GetBusinessObjectCollection<Part>("");
            var userRepository = new PartRepository();
            Assert.AreEqual(1,parts.Count);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Delete(part);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, parts.Count);
        }

    }
}