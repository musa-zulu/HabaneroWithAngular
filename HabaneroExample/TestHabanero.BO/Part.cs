using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.Smooth;

namespace TestHabanero.BO
{
    public class Part : BusinessObject
    {
        public Guid PartId
        {
            get { return ((Guid) GetPropertyValue("PartId")); }
            set { base.SetPropertyValue("PartId", value); }
        }

        public string Name
        {
            get { return ((string) GetPropertyValue("Name")); }
            set { base.SetPropertyValue("Name", value); }
        }
         public string Description  
        {
            get { return ((string) GetPropertyValue("Description")); }
            set { base.SetPropertyValue("Description", value); }
        }

        [AutoMapDefault("0")]
        public int Price
        {
            get { return ((int) GetPropertyValue("Price")); }
            set { base.SetPropertyValue("Price", value); }
        }

        [AutoMapOneToMany(RelationshipType.Association, ReverseRelationshipName = "Part")]
        public virtual BusinessObjectCollection<CarPart> CarParts
        {
            get { return Relationships.GetRelatedCollection<CarPart>("CarParts"); }
        }

    }
}