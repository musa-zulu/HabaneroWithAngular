using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using TestHabanero.BO;
using TestHabenaro.Db.Interfaces;

namespace TestHabenaro.DB
{
    public class CarRepository : ICarRepository
    {
        
        public  void Save(Car car)
        {
            car.Save();
        }

        public  List<Car> GetCars()
        {
         
            return new List<Car>(Broker.GetBusinessObjectCollection<Car>("", "CarId"));
        }

        public  Car GetCarBy(Guid id)
        {
            return Broker.GetBusinessObject<Car>(new Criteria("CarId", Criteria.ComparisonOp.Equals, id));
        }

        public  void Update(Car existingCar, Car newCar)
        {
            existingCar.Make = newCar.Make;
            existingCar.Model = newCar.Model;
            existingCar.Save();
        }

        public  void Delete(Car car)
        {
            car.MarkForDelete();
            car.Save();
        }
    }
}