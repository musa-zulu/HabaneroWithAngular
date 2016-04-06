using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.Testability;
using TestHabanero.BO;
using TestHabanero.Models;

namespace TestHabanero.Tests.Commons
{
    public class CarViewModelBuilder
    {
        private readonly CarViewModel _car;
        public CarViewModelBuilder()
        {
//            _car = BuildValid();
            _car.Make = RandomValueGen.GetRandomString();
            _car.Model = RandomValueGen.GetRandomString();
        }

//        private BOTestFactory<T> CreateFactory<T>()
//        {
//            return new BOTestFactory<T>();
//        }

//        private CarViewModel BuildValid()
//        {
//            var car = CreateFactory<CarViewModel>()
//                 .SetValueFor(c => c.Model)
//                .CreateValidBusinessObject();
//            return car;
//        }

        public CarViewModelBuilder WithMake(string make)
        {
            _car.Make = make;
            return this;
        }

        public CarViewModelBuilder WithRandomId()
        {
            _car.CarId = Guid.NewGuid();
            return this;
        }

        public CarViewModel Build()
        {
            return _car;
        }
    }
}
