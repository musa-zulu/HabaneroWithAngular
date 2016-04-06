using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using TestHabanero.BO;
using TestHabenaro.Db.Interfaces;

namespace TestHabenaro.DB
{
    public class PartRepository:IPartRepository
    {
        public void Save(Part part)
        {
            part.Save();
        }

        public List<Part> GetParts()
        {
            return new List<Part>(Broker.GetBusinessObjectCollection<Part>("", "PartId"));
        }

        public Part GetPartBy(Guid id)
        {
            return Broker.GetBusinessObject<Part>(new Criteria("PartId", Criteria.ComparisonOp.Equals, id));
        }

        public void Update(Part existingPart, Part newPart)
        {
            existingPart.Name = newPart.Name;
            existingPart.Description = newPart.Description;
            existingPart.Price = newPart.Price;
        }

        public void Delete(Part part)
        {
            part.MarkForDelete();
            part.Save();
        }
    }
}