using System;
using Habanero.BO;
using Habanero.Smooth;

namespace TestHabanero.BO
{
    public class CarPart: BusinessObject
    {
        public Guid CarPartId
        {
            get { return ((Guid)GetPropertyValue("CarPartId")); }
            set { base.SetPropertyValue("CarPartId", value); }
        }
        public Guid CarId
        {
            get { return ((Guid)GetPropertyValue("CarId")); }
            set { base.SetPropertyValue("CarId", value); }
        }
        public Guid PartId
        {
            get { return ((Guid)GetPropertyValue("PartId")); }
            set { base.SetPropertyValue("PartId", value); }
        }
        [AutoMapDefault("0")]
        public int Quantity
        {
            get { return ((int)GetPropertyValue("Quantity")); }
            set { base.SetPropertyValue("Quantity", value); }
        }

        [AutoMapCompulsory]
        [AutoMapManyToOne]
        public Car Car
        {
            get { return Relationships.GetRelatedObject<Car>("Car"); }
            set { Relationships.SetRelatedObject("Car", value); }
        }

        [AutoMapCompulsory]
        [AutoMapManyToOne]
        public Part Part
        {
            get { return Relationships.GetRelatedObject<Part>("Part"); }
            set { Relationships.SetRelatedObject("Part", value); }
        }

    }
}