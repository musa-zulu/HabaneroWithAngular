using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using TestHabanero.BO;
using TestHabenaro.Db.Interfaces;

namespace TestHabenaro.DB
{
    public class CarPartRepository:ICarPartRepository
    {
        public void Save(CarPart car)
        {
            car.Save();
        }

        public List<CarPart> GetCarPart()
        {
            return new List<CarPart>(Broker.GetBusinessObjectCollection<CarPart>("", "CarPartId"));
        }

        public CarPart GetCarPartBy(Guid id)
        {
            return Broker.GetBusinessObject<CarPart>(new Criteria("CarPartId", Criteria.ComparisonOp.Equals, id));
        }

        public void Update(CarPart existingCarPart, CarPart newCarPart)
        {
            existingCarPart.Quantity = newCarPart.Quantity;
            existingCarPart.PartId = newCarPart.PartId;
            existingCarPart.CarId = newCarPart.CarId;
            existingCarPart.Save();
        }

        public void Delete(CarPart carPart)
        {
          carPart.MarkForDelete();
            carPart.Save();
        }
    }
}