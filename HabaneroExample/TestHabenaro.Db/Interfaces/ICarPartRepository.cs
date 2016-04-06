using System;
using System.Collections.Generic;
using TestHabanero.BO;

namespace TestHabenaro.Db.Interfaces
{
    public interface ICarPartRepository
    {

        void Save(CarPart car);
        List<CarPart> GetCarPart();
        CarPart GetCarPartBy(Guid id);
        void Update(CarPart existingCarPart, CarPart newCarPart);
        void Delete(CarPart carPart);
    }
}