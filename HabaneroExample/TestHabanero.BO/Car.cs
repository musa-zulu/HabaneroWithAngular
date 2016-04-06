using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.Smooth;

namespace TestHabanero.BO
{
    public enum Color
    {
        Red,
        Blue,
        Green,
        Orange,
        White,
        Black
    }

    public class Car : BusinessObject
    {
        public Guid CarId
        {
            get { return ((Guid)GetPropertyValue("CarId")); }
            set { base.SetPropertyValue("CarId", value); }
        }
        public string Make
        {
            get { return ((string)GetPropertyValue("Make")); }
            set { base.SetPropertyValue("Make", value); }
        }
        public Color Color
        {
            get { return ((Color)GetPropertyValue("Color")); }
            set { base.SetPropertyValue("Color", value); }
        }
        public string Model
        {
            get { return ((string)GetPropertyValue("Model")); }
            set { base.SetPropertyValue("Model", value); }
        }

        [AutoMapOneToMany(RelationshipType.Association, ReverseRelationshipName = "Car")]
        public virtual BusinessObjectCollection<CarPart> CarParts
        {
            get { return Relationships.GetRelatedCollection<CarPart>("CarParts"); }
        }

    }
}