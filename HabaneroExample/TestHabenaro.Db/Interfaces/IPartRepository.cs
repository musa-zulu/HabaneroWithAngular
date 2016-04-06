using System;
using System.Collections.Generic;
using TestHabanero.BO;

namespace TestHabenaro.Db.Interfaces
{
    public interface IPartRepository
    {
        void Save(Part part);
        List<Part> GetParts();
        Part GetPartBy(Guid id);
        void Update(Part existingPart, Part newPart);
        void Delete(Part part);
    }
}