using System;
using System.Collections.Generic;
using TestHabanero.BO;

namespace TestHabenaro.Db.Interfaces
{
    public interface ICarRepository
    {
          void Save(Car car);
          List<Car> GetCars();
          Car GetCarBy(Guid id);
          void Update(Car existingCar, Car newCar);
          void Delete(Car car);
    }
}
